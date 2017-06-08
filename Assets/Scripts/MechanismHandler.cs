using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// INPUTS : GameObject grille, les coordonnées de la case sur lequel le joueur a décidé de jouer
/// TRAITEMENTS : Analyse la chute du pion, déclenche tous les triggers, détecte les puissance 4
/// OUTPUTS : fin de tour auprès du GameHandler, avec info puissance 4
/// </summary>

/// IMPORTANT : TO SWITCH FROM A VARIANT TO ANOTHER CHECK GAMEHANDLER
public class MechanismHandler : MonoBehaviour {
    public Grid myGrid { get; set; }
    public Atlas gridAtlas;
    public GameObject currentPawn;
    public GameObject winningLine;
    public GameObject loadingPanel;
    public GameObject overlayPanel;
    public int gravity { get; set; } // direction de chute des pions
    
    //0: down | 1: left | 2: up | 3: right | 4: upLeft | 5: upRight
    public Dictionary<int, Coord> fallIntegers = new Dictionary<int, Coord> () {
        { 0, new Coord (0,-1) },
        { 1, new Coord (-1,0) },
        { 2, new Coord (0,1) },
        { 3, new Coord (1,0) },
        { 4, new Coord (-1,1) },
        { 5, new Coord (1,1) }
    };

    // Use this for initialization
    IEnumerator Start () {
        gravity = 0; //0: down | 1: left | 2: up | 3: right 

        //attente que la grille d'editor soit détruite puis que la grille de jeu soit chargée.

        overlayPanel.SetActive (true);
        loadingPanel.SetActive (true);
        // à changer pour attendre que la grille soit chargée plutôt que juste "2s"
        
        yield return StartCoroutine (GameObject.Find ("GridHolder").GetComponent <GridGenerator> ().GenerateGrid ());
        yield return StartCoroutine (GameObject.Find ("GridHolder").GetComponent <GridGenerator> ().DisplayFromSave ());
        //LoadingPanel.SetActive (false);
        myGrid = GameObject.Find ("Generated Grid(Clone)").GetComponent<Grid> ();
        gridAtlas = GenerateAtlas ();
        GameObject.Find("IAHandler").GetComponent<IAMain>().myAtlas = gridAtlas;
        GameObject.Find("IAHandler").GetComponent<IAMain>().settingGrid(GameObject.Find("Generated Grid(Clone)").GetComponent<Grid>().gridSize);
        
        currentPawn = Resources.Load ("Prefabs/PawnPlayer1") as GameObject;
        yield return null;
        overlayPanel.SetActive (false);
        loadingPanel.SetActive (false);
    }

    /// <summary>
    /// Generate the atlas of the current grid displayed
    /// </summary>
    /// <returns></returns>
    public Atlas GenerateAtlas () {
        Atlas gridAtlas = new Atlas ();
        gridAtlas.gridDictionary = new Dictionary<Coord, Cell> ();
        // On parcourt l'ensemble des cases et on crée un Dictionary avec comme key les coordonnées de chacune des Cells
        foreach (Transform cellChild in myGrid.GetComponent<Transform> ()) {
            gridAtlas.gridDictionary.Add (cellChild.GetComponent<Cell> ().coordinates, cellChild.GetComponent<Cell> ());
        }
        return gridAtlas;
    }

    /// <summary>
    /// First step in making a pawn fall.
    /// Will call next cell to calculate the falling
    /// Will call PawnFallDo coroutine to animate and place the pawn at the correct place
    /// </summary>
    /// <param name="startCell"> Cell the player clicked</param>
    /// <param name="player"> for now an int but -> Player peut etre une structure qui contient les visuels des pions, les noms, les taunts, etc... </param>
    /// <param name="reset">true if called from a resetGravity trigger, else false </param>
    /// <returns></returns>
    public IEnumerator PawnFallCalculation (Cell startCell, int player, bool reset, bool click) {
        // Calcule où le pion va s'arrêter de chuter depuis les coordonnées ou il a été lâché.
        Coord interCoords = startCell.coordinates;
        Cell currentCell = startCell;
        bool falling = true;

        while (falling) {
            currentCell = gridAtlas.gridDictionary[interCoords] as Cell;
            Cell nextCell = NextCell (currentCell, gravity);
            if (nextCell.available &&
                !nextCell.Equals (currentCell) &&
                (gravity == 0 && !currentCell.walls.wally
                    || gravity == 1 && !nextCell.walls.wallx
                    || gravity == 2 && !nextCell.walls.wally
                    || gravity == 3 && !currentCell.walls.wallx
                )) {
                interCoords = nextCell.coordinates;
            } else {
                falling = false;
            }
        }

        //script plaçant le pion et lançant le visuel
        if (!reset) {
            Transform newPawn = Instantiate (currentPawn).GetComponent<Transform> ();
            newPawn.GetComponentInChildren <PawnShape> ().TurnPawnShape (gravity);
            yield return StartCoroutine (PawnFallDo (newPawn, currentCell, player, false, startCell));
        } else {
            Transform existingPawn = startCell.GetComponentInChildren<Pawn> ().transform;
            yield return StartCoroutine (PawnFallDo (existingPawn, currentCell, player, true, startCell));
        }
        //script de vérification de la puissance 4
        CheckAlign4 (currentCell, player);
    }

    /// <summary>
    /// Looks for the next cell for the falling pawn depending on gravity
    /// </summary>
    /// <param name="currentCell"></param>
    /// <param name="gravity"></param>
    /// <returns>the next cell if any, else returns calling cell</returns>
    public Cell NextCell (Cell currentCell, int gravity) {
        //0: down | 1: left | 2: up | 3: right 
        Coord currentCoordinates = currentCell.coordinates; 
        switch (gravity) {
        case 0:
            currentCoordinates.y = currentCoordinates.y-1;
            if (gridAtlas.gridDictionary.ContainsKey (currentCoordinates)) {
                break;
            } else return currentCell;
        case 1:
            currentCoordinates.x = currentCoordinates.x-1;
            if (gridAtlas.gridDictionary.ContainsKey (currentCoordinates)) {
                break;
            } else return currentCell;
        case 2:
            currentCoordinates.y = currentCoordinates.y+1;
            if (gridAtlas.gridDictionary.ContainsKey (currentCoordinates)) {
                break;
            } else return currentCell;
        case 3:
            currentCoordinates.x = currentCoordinates.x+1;
            if (gridAtlas.gridDictionary.ContainsKey (currentCoordinates)) {
                break;
            } else return currentCell;
        default:
            return currentCell;
        }
        return gridAtlas.gridDictionary[currentCoordinates];
    }

    /// <summary>
    /// Looks for the previous cell for the falling pawn depending on gravity
    /// </summary>
    /// <param name="currentCell"></param>
    /// <param name="gravity"></param>
    /// <returns>the next cell if any, else returns calling cell</returns>
    public Cell PreviousCell (Cell currentCell, int gravity) {
        //0: down | 1: left | 2: up | 3: right 
        Coord currentCoordinates = currentCell.coordinates; 
        switch (gravity) {
        case 0:
            currentCoordinates.y = currentCoordinates.y+1;
            if (gridAtlas.gridDictionary.ContainsKey (currentCoordinates)) {
                break;
            } else return currentCell;
        case 1:
            currentCoordinates.x = currentCoordinates.x+1;
            if (gridAtlas.gridDictionary.ContainsKey (currentCoordinates)) {
                break;
            } else return currentCell;
        case 2:
            currentCoordinates.y = currentCoordinates.y-1;
            if (gridAtlas.gridDictionary.ContainsKey (currentCoordinates)) {
                break;
            } else return currentCell;
        case 3:
            currentCoordinates.x = currentCoordinates.x-1;
            if (gridAtlas.gridDictionary.ContainsKey (currentCoordinates)) {
                break;
            } else return currentCell;
        default:
            return currentCell;
        }
        return gridAtlas.gridDictionary[currentCoordinates];
    }

    /// <summary>
    /// Place le pion, détecte et exécute tous les triggers
    /// </summary>
    /// <param name="pawn"></param>
    /// <param name="endCell"></param>
    /// <param name="player"> for now an int but -> Player peut etre une structure qui contient les visuels des pions, les noms, les taunts, etc.. </param>
    /// <param name="reset"> true if called from a resetGravity trigger, else false </param>
    /// <param name="startCell"></param>
    /// <returns></returns>
    public IEnumerator PawnFallDo (Transform pawn, Cell endCell, int player, bool reset, Cell startCell) {
        List<Cell.Trigger> triggers = new List<Cell.Trigger> ();

        //rend la case finale non disponible pour les futurs pions
        endCell.available = false;

        //Stockage de tous les triggers traversés
        Coord startCoords = endCell.coordinates;
        Cell topCell;

        if (reset) {
            topCell = startCell;
            startCoords = startCell.coordinates;
        } else {
            do {
                startCoords -= fallIntegers[gravity];
            } while (gridAtlas.gridDictionary.ContainsKey (startCoords)); // On a rejoint le bord du graphique, prêts à balayer en sens inverse.
            switch (gravity) {
            case 0:
                topCell = gridAtlas.gridDictionary [startCoords - new Coord (0, 1)];
                break;
            case 1:
                topCell = gridAtlas.gridDictionary [startCoords - new Coord (1, 0)];
                break;
            case 2:
                topCell = gridAtlas.gridDictionary [startCoords - new Coord (0, -1)];
                break;
            case 3:
                topCell = gridAtlas.gridDictionary [startCoords - new Coord (-1, 0)];
                break;
            default:
                topCell = gridAtlas.gridDictionary [startCoords - new Coord (0, 1)];
                break;
            }
        }
        //on ne check les triggers que pour les pions mis, pas lors du reset gravity
        if (!reset) {
            do {
                startCoords += fallIntegers[gravity];
                if (gridAtlas.gridDictionary[startCoords].trigger.isTrigger) {
                    triggers.Add (gridAtlas.gridDictionary[startCoords].trigger);
                }
            } while (!startCoords.Equals (endCell.coordinates)) ; // On a collecté tous les triggers traversés
        }
        pawn.SetParent (topCell.GetComponent<Transform> ()); // on part du haut
        pawn.localPosition = Vector3.zero;

        pawn.SetParent (endCell.transform);

        yield return StartCoroutine (AnimateFall(pawn, endCell, reset));
        
        if (topCell.coordinates != endCell.coordinates && PreviousCell(endCell,gravity).available) {
            pawn.GetComponentInChildren<Animation> ().Play("PawnBounceAnimation");
            yield return new WaitForSeconds (pawn.GetComponentInChildren<Animation> ()["PawnBounceAnimation"].length);
        }

        if (triggers.Count != 0 && !reset)
        {
            foreach (Cell.Trigger trigger in triggers)
            {
                yield return StartCoroutine(ExecuteTrigger(trigger.triggerType, 1.0f));
            }
        }

        //appelle l'IA pour mettre à jour son arbre.
        if (!reset)
            GameObject.Find("IAHandler").GetComponent<IAMain>().GetCurrentPlay(endCell.coordinates);
        
        if (reset) {
            CheckAlign4 (endCell, pawn.GetComponent<Pawn> ().player);
        }
    }

    /// <summary>
    /// Réalise l'animation de la chute du pion
    /// </summary>
    /// <param name="pawn"> Pion à animer </param>
    /// <param name="endCell"> cell finale à atteindre </param>
    /// <param name="reset"> true if called from a resetGravity trigger, else false </param>
    /// <returns></returns>
    IEnumerator AnimateFall (Transform pawn, Cell endCell, bool reset) {
        //continuous speed for now, will improve later
        float speed = 10f;

        switch (gravity) {
        case 0:
            while (pawn.position.y > endCell.GetComponent<Transform> ().position.y) {
                pawn.Translate (Vector3.down * Time.deltaTime * speed);
                yield return null;
            }
            break;
        case 1:
            while (pawn.position.x > endCell.GetComponent<Transform> ().position.x) {
                pawn.Translate (Vector3.left * Time.deltaTime * speed);
                yield return null;
            }
            break;
        case 2:
            while (pawn.position.y < endCell.GetComponent<Transform> ().position.y) {
                pawn.Translate (Vector3.up * Time.deltaTime * speed);
                yield return null;
            }
            break;
        case 3:
            while (pawn.position.x < endCell.GetComponent<Transform> ().position.x) {
                pawn.Translate (Vector3.right * Time.deltaTime * speed);
                yield return null;
            }
            break;
        default:
            break;
        }
        pawn.localPosition = Vector3.zero;
    }

    /// <summary>
    /// calcule et anime les actions des triggers
    /// </summary>
    /// <param name="triggerType">int, type du trigger à exécuter </param>
    /// <param name="time"> durée totale de l'animation </param>
    /// <returns></returns>
    IEnumerator ExecuteTrigger (int triggerType, float time) {
        int rotate = 0;
        float elapsedTime = 0.0f;
        switch (triggerType) {
        //gravity -> 0: down | 1: left | 2: up | 3: right 
        case 0: //90r
            rotate = 90;
            gravity = (gravity + 3) % 4;
            break;
        case 1: //90l
            rotate = -90;
            gravity = (gravity + 1) % 4;
            break;
        case 2: //180
            rotate = 180;
            gravity = (gravity + 2) % 4;
            break;
        case 3: //gravity reset
            ResetGravity ();
            break;
        default:
            rotate = 0;
            break;
        }
        GameObject mainCamera = GameObject.Find ("Main Camera");
        Quaternion startingRotation = mainCamera.GetComponent<Transform> ().rotation;
        Quaternion targetRotation = Quaternion.Euler ( new Vector3 ( 0.0f, 0.0f, startingRotation.eulerAngles.z + rotate ) );
        while (elapsedTime < time) {
            elapsedTime += Time.deltaTime; // <- move elapsedTime increment here 
            // Rotations
            mainCamera.GetComponent<Transform> ().rotation = Quaternion.Slerp(startingRotation, targetRotation,  (elapsedTime / time)  );
            yield return new WaitForEndOfFrame ();
        }
        foreach( GameObject pawnObject in GameObject.FindGameObjectsWithTag ("Pawn")) {
            pawnObject.GetComponent <PawnShape> ().TurnPawnShape (gravity);
        };
    }

    /// <summary>
    /// Parcours la grille de bas en haut (selon la gravité actuelle) et simule un nouveau lâché de pion pour tous ceux déjà en place
    /// </summary>
    public void ResetGravity () {
        int player;
        Cell cellToReset;
        
        switch (gravity) {
        case 0:
            for (int y = 1; y < myGrid.gridSize.y; y++) {
                for (int x = 0; x < myGrid.gridSize.x; x++) {
                    cellToReset = gridAtlas.gridDictionary[new Coord (x, y)];
                    if (cellToReset.GetComponentInChildren<Pawn> ()) {
                        cellToReset.available = true;
                        player = cellToReset.GetComponentInChildren<Pawn> ().player;
                        StartCoroutine (PawnFallCalculation (cellToReset, player, true, false));
                    }
                }
            }
            break;
        case 1:
            for (int x = 1; x < myGrid.gridSize.x; x++) {
                for (int y = myGrid.gridSize.y - 1; y >= 0; y--) {
                    cellToReset = gridAtlas.gridDictionary[new Coord (x, y)];
                    if (cellToReset.GetComponentInChildren<Pawn> ()) {
                        cellToReset.available = true;
                        player = cellToReset.GetComponentInChildren<Pawn> ().player;
                        StartCoroutine (PawnFallCalculation (cellToReset, player, true, false));
                    }
                }
            }
            break;
        case 2:
            for (int y = myGrid.gridSize.y - 2; y >= 0; y--) {
                for (int x = myGrid.gridSize.x - 1; x >= 0; x--) {
                    cellToReset = gridAtlas.gridDictionary[new Coord (x, y)];
                    if (cellToReset.GetComponentInChildren<Pawn> ()) {
                        cellToReset.available = true;
                        player = cellToReset.GetComponentInChildren<Pawn> ().player;
                        StartCoroutine (PawnFallCalculation (cellToReset, player, true, false));
                    }
                }
            }
            break;
        case 3:
            for (int x = myGrid.gridSize.x - 2; x >= 0; x--) {
                for (int y = 0; y < myGrid.gridSize.y; y++) {
                    cellToReset = gridAtlas.gridDictionary[new Coord (x, y)];
                    if (cellToReset.GetComponentInChildren<Pawn> ()) {
                        cellToReset.available = true;
                        player = cellToReset.GetComponentInChildren<Pawn> ().player;
                        StartCoroutine (PawnFallCalculation (cellToReset, player, true, false));
                    }
                }
            }
            break;
        default:
            break;
        }
    }

    /// <summary>
    /// Check un puissance 4 comprennant le pion venant d'être placé
    /// </summary>
    /// <param name="newFilled"> cell that just got filled </param>
    /// <param name="player"></param>
    public void CheckAlign4 (Cell newFilled, int player) {
        Coord currentCoords = newFilled.coordinates;
        Coord startCoords; 
        
        //test selon les 4 directions
        foreach (string i in new List<string> () {"right", "UR", "up", "UL"}) {
            startCoords = currentCoords;
            while(gridAtlas.gridDictionary.ContainsKey (startCoords - fallIntegers[directionConversionString(i)])){
                // On a rejoint le bord du graphique, prêts à balayer en sens inverse.
                startCoords -= fallIntegers[directionConversionString(i)];
            }
            int count = 0;
            // On compte les pions CONSECUTIFS du joueur suivant la direction. Arrivé à 4 c'est la victoire.
            while (gridAtlas.gridDictionary.ContainsKey (startCoords)) {
                if (gridAtlas.gridDictionary[startCoords].GetComponentInChildren<Pawn> () && gridAtlas.gridDictionary[startCoords].GetComponentInChildren<Pawn> ().player == player) {
                    if (count + 1 >= 4) {
                        Coord originCoords = new Coord ();
                        Coord destCoords = new Coord ();

                        originCoords = startCoords;
                        destCoords = originCoords - 3 * fallIntegers [directionConversionString (i)];

                        GameObject newLine = Instantiate (winningLine);
                        Vector3 originPosition = gridAtlas.gridDictionary[originCoords].GetComponent<Transform> ().position;
                        Vector3 destinationPosition = gridAtlas.gridDictionary[destCoords].GetComponent<Transform> ().position;
                        newLine.GetComponent<WinningLine> ().DisplayLine (originPosition, destinationPosition);

                        GameObject.Find ("GeneralHandler").GetComponent<GameHandler> ().GameOver (player);
                    }
                    else if (!IsBlocked (startCoords, i)) count++;
                    else count = 0;
                }
                else count = 0;
                startCoords += fallIntegers[directionConversionString(i)];
            }
        }
        return;
    }
    //vérifie si un mur ne bloque pas l'établissement de la puissance 4
    public bool IsBlocked (Coord coords, string direction) {
        switch (direction) {
        case "right":
            if (gridAtlas.gridDictionary [coords].walls.wallx) {
                return true;
            } else
                break;
        case "up":
            if (gridAtlas.gridDictionary.ContainsKey (coords + new Coord(0,1)) && gridAtlas.gridDictionary[coords + new Coord(0,1)].walls.wally) {
                return true;
            } else
                break;
        case "UR":
            if (gridAtlas.gridDictionary.ContainsKey (coords + new Coord(1,1)) && gridAtlas.gridDictionary[coords + new Coord(1,1)].walls.wallxy) {
                return true;
            } else
                break;
        case "UL":
            if (gridAtlas.gridDictionary.ContainsKey (coords + new Coord(-1,1)) &&gridAtlas.gridDictionary[coords + new Coord(-1,1)].walls.wallxy) {
                return true;
            } else
                break;
        default:
            break;
        }
            return false;
    }

    /// <summary>
    /// simple conversion method for old usage of strings for directions
    /// </summary>
    /// <param name="direction">direction as a string</param>
    /// <returns>direction as an int</returns>
    public int directionConversionString (string direction) {
        //0: down | 1: left | 2: up | 3: right | 4: upLeft | 5: upRight
        try {
            switch (direction.ToLower ()) {
            case "down":
                return 0;
            case "d":
                return 0;

            case "left":
                return 1;
            case "l":
                return 1;

            case "up":
                return 2;
            case "u":
                return 2;

            case "right":
                return 3;
            case "r":
                return 3;

            case "upleft":
                return 4;
            case "ul":
                return 4;

            case "upright":
                return 5;
            case "ur":
                return 5;

            default:
                return -1;
            }
        } catch (System.Exception ex) {
            throw (ex);
        }
    }
}
