using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GridGenerator : MonoBehaviour {

    public LevelHolder levels;
    public List<GridHolder> grids;
    //index de la grid qu'on regarde
    public Transform cellPrefab;
    public Transform gridPrefab;
    private static string levelDataFileName = "levelDataVariant";
    public string json { get; set; }
    public int gridIndex;
    public int variant;
    List<Coord> allCellCoords;
    GridHolder currentGrid;

    /// <summary>
    /// Appelé lors d'une modification d'un paramètre d'une cell dans l'editor
    /// </summary>
    public void GenerateFromValidate () {
        SaveCells ();
        DisplayFromSave ();
    }

    /// <summary>
    /// Appelé par l'éditeur de façon automatique
    /// 1) Génère la grille depuis les infos du gridHolder
    /// 2) Update le nombre de cellules lors d'un changement de gridSize
    /// 3) Popule les cellules avec leur contenu
    /// </summary>
    public void GenerateEditor () {
        GenerateGrid ();
        UpdateCountCells ();
        CheckAvailability ();
        DisplayFromSave ();
    }

    /// <summary>
    /// Version manuelle de la génération de grille (cf GenerateEditor)
    /// </summary>
    public void GenerateGridButton () {
        GenerateGrid ();
        UpdateCountCells ();
        DisplayFromSave ();
    }
    

    /// <summary>
    /// Génère un string json des grilles contenues dans l'editor
    /// </summary>
    public void GenerateJson () {
        string levelDataFileNameGeneration = levelDataFileName + variant.ToString () + ".json";
        string filePath = Path.Combine (Application.streamingAssetsPath, levelDataFileNameGeneration);

        levels.grids = grids;
        json = JsonUtility.ToJson(levels, false);
        print ("Json généré");
        
        if (File.Exists (filePath)) {
            // Write the json to the file at the path
            File.WriteAllText (filePath, json);
            print ("Fichier sauvegardé");
        }
        else {
            File.Create (Application.streamingAssetsPath);
            GenerateJson ();
        }
        
    }

    /// <summary>
    /// 1) Load le json des niveaux
    /// </summary>
    public void LoadLevelDataSoft () {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string levelDataFileNameLoading = levelDataFileName + variant.ToString () + ".json";
        string filePath = Path.Combine (Application.streamingAssetsPath, levelDataFileNameLoading);

        if (File.Exists (filePath)) {
            // Read the json from the file into a string
            json = File.ReadAllText (filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            levels = JsonUtility.FromJson<LevelHolder> (json);
            print ("Json chargé");
            grids = levels.grids;
        }
        else {
            Debug.LogError ("Cannot load game data!");
        }
    }

    /// <summary>
    /// 1) Load le json des niveaux
    /// 2) Génère la grille correspondante
    /// 3) Popule la grille avec le contenu des cells (indiqué dans le json)
    /// </summary>
    public void LoadLevelData () {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string levelDataFileNameLoading = levelDataFileName + variant.ToString () + ".json";
        string filePath = Path.Combine (Application.streamingAssetsPath, levelDataFileNameLoading);

        if (File.Exists (filePath)) {
            // Read the json from the file into a string
            json = File.ReadAllText (filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            levels = JsonUtility.FromJson<LevelHolder> (json);
            print ("Json chargé");
            grids = levels.grids;
            GenerateGrid ();
            print ("Grille générée");   
            DisplayFromSave ();
            print ("Grille populée");
        }
        else {
            Debug.LogError ("Cannot load game data!");
        }
    }

    /// <summary>
    /// Si la gridSize est différent du nombre de cellules dans le gridHodler de la grille, les cellules sont regénérées.
    /// </summary>
    public void UpdateCountCells () {
        if (grids[gridIndex].gridSize.x * grids[gridIndex].gridSize.y != grids[gridIndex].cells.Count) {
            SaveCells ();
        }
    }

    /// <summary>
    /// Génère la grille depuis la grille et les cases (mais pas leur contenu !) depuis la grille virtuelle
    /// </summary>
    public void GenerateGrid () {
        //on setup l'index de la grid qu'on modifie
        currentGrid = grids[gridIndex];
        allCellCoords = new List<Coord> ();
        // Vérifier que l'object n'existe pas déjà, en cas, le détruire
        string holderName = "Generated Grid(Clone)";
        if (transform.Find (holderName)) {
            DestroyImmediate(transform.Find (holderName).gameObject);
        }
        // instantiation du gridPrefab qui va host le script Grid et les Cells filles.
        Transform newGrid = Instantiate (gridPrefab, Vector3.zero, Quaternion.identity) as Transform; 
        newGrid.parent = transform;
        Grid gridScript = newGrid.GetComponent<Grid> ();
        gridScript.gridSize = currentGrid.gridSize;

        // Spawning cells
        for (int x = 0; x < currentGrid.gridSize.x; x++) {
            for (int y = 0; y < currentGrid.gridSize.y; y++) {
                Vector3 cellPosition = CoordToPosition (x, y);
                Transform newCell = Instantiate (cellPrefab, cellPosition, Quaternion.identity) as Transform;
                newCell.parent = newGrid;
                Cell cellScript = newCell.GetComponent<Cell> ();

                //ajoute les coordonnées de chacune des Cells
                cellScript.coordinates = new Coord (x, y);
                allCellCoords.Add (new Coord (x, y));
            }
        }

        // Setup de l'origine : dépend de la parité de x et y
        if (currentGrid.gridSize.x % 2 != 0 ) {
            newGrid.Translate(Vector3.left * 0.5f);
        }
        if (currentGrid.gridSize.y % 2 != 0 ) {
            newGrid.Translate(Vector3.down * 0.5f);
        }
    }

    /// <summary>
    /// sauvegarde des modifications des cells de la grille physique dans la grille virtuelle
    /// </summary>
    public void SaveCells () {
        grids [gridIndex].cells.Clear ();
        Grid grid = transform.Find ("Generated Grid(Clone)").gameObject.GetComponent<Grid> ();
        foreach (Transform cellChild in grid.GetComponent<Transform> ()) {

            cellChild.GetComponent<Cell> ().available = !cellChild.GetComponent<Cell> ().full && !cellChild.GetComponent<Cell> ().pawn.isPawn;

            grids[gridIndex].cells.Add (
                //on attribue les carac de la cell physiques à une nouvelle cell virtuelle
                new CellHolder {
                    coordinates = cellChild.GetComponent<Cell> ().coordinates,
                    walls = cellChild.GetComponent<Cell> ().walls,
                    nets = cellChild.GetComponent<Cell> ().nets,
                    triggers = cellChild.GetComponent<Cell> ().trigger,
                    pawns = cellChild.GetComponent<Cell> ().pawn,
                    available = cellChild.GetComponent<Cell> ().available,
                    hidden = cellChild.GetComponent<Cell> ().hidden,
                    full = cellChild.GetComponent<Cell> ().full
                }
            );
        }
    }

    /// <summary>
    /// Ajuste le paramètre available de chaque cell en fonction du paramètre full.
    /// </summary>
    public void CheckAvailability () {
        foreach (CellHolder cell in currentGrid.cells) {
            cell.available = !cell.full && !cell.pawns.isPawn;
        }
    }

    /// <summary>
    /// Parcourt la grille virtuelle et applique le contenu dans la grid physique
    /// </summary>
    public void DisplayFromSave () {
        int i = 0;
        Grid grid = transform.Find ("Generated Grid(Clone)").gameObject.GetComponent<Grid> ();
        List<string> contentNames = new List<string> () {
            "WallX(Clone)", "WallY(Clone)", "WallXY(Clone)",
            "NetX(Clone)", "NetY(Clone)",
            "TurnRight(Clone)", "TurnLeft(Clone)", "TurnUpsideDown(Clone)", "TurnChoice(Clone)",
            "GravityReset(Clone)", "RandomTrigger(Clone)",
            "TranslateRight(Clone)", "TranslateLeft(Clone)", "TranslateUp(Clone)", "TranslateDown(Clone)",
            "PawnNeutral(Clone)", "PawnPlayer1(Clone)", "PawnPlayer2(Clone)", "PawnCommon(Clone)",
            "CellHidden(Clone)", "CellCover(Clone)"
        };
        //parcourt de toute la grille physique
        foreach (Transform cellChild in grid.GetComponent<Transform> ()) {
            foreach (string content in contentNames) {
                //on supprime le contenu des cells pour ne pas avoir de doublon
                DeleteExistingCellChild (cellChild, content);
            }
            //on attribue les carac des cells virtuelles aux cells de la grille
            cellChild.GetComponent<Cell> ().coordinates = currentGrid.cells[i].coordinates;
            cellChild.GetComponent<Cell> ().walls = currentGrid.cells[i].walls;
            cellChild.GetComponent<Cell> ().nets = currentGrid.cells[i].nets;
            cellChild.GetComponent<Cell> ().trigger = currentGrid.cells[i].triggers;
            cellChild.GetComponent<Cell> ().pawn = currentGrid.cells[i].pawns;
            cellChild.GetComponent<Cell> ().hidden = currentGrid.cells [i].hidden;
            cellChild.GetComponent<Cell> ().full = currentGrid.cells [i].full;
            cellChild.GetComponent<Cell> ().available = currentGrid.cells [i].available;
            //cell invisible ?
            if (cellChild.GetComponent<Cell> ().hidden) {
                // GameObject newCellCover = Instantiate (grid.cellCover);
                // newCellCover.transform.SetParent (cellChild);
                // newCellCover.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -12);
                GameObject newCellHidden = Instantiate (grid.cellHidden);
                newCellHidden.transform.SetParent (cellChild);
                newCellHidden.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -15);
            }
            //cell bloquée ?
            if (cellChild.GetComponent<Cell> ().full) {
                GameObject newCellCover = Instantiate (grid.cellCover);
                newCellCover.transform.SetParent (cellChild);
                newCellCover.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -12);
            }
            //spawn des murs et des triggers
            SpawnWalls (cellChild, cellChild.GetComponent<Cell> ().walls, grid);
            SpawnNets (cellChild, cellChild.GetComponent<Cell> ().nets, grid);
            SpawnTriggers (cellChild, cellChild.GetComponent<Cell> ().trigger, grid);
            SpawnPawns (cellChild, cellChild.GetComponent<Cell> ().pawn, grid);
            i++;
        }
    }

    /// <summary>
    /// spawn des gameobjects wall de la cell
    /// </summary>
    /// <param name="cellTransform"> cell to modify </param>
    /// <param name="walls"> walls to add and display</param>
    /// <param name="grid"> grid object </param>
    void SpawnWalls (Transform cellTransform, Cell.Walls walls, Grid grid) {
        //check if wallx/wally/wallxy exists and instanciate them at the correct place
        if (walls.wallx) {
            GameObject newWallX = Instantiate (grid.wallX);
            newWallX.transform.SetParent (cellTransform);
            newWallX.GetComponent<Transform> ().localPosition = new Vector3(0.5f, -0.5f, -20f);
        }

        if (walls.wally) {
            GameObject newWallY = Instantiate (grid.wallY);
            newWallY.transform.SetParent (cellTransform);
            newWallY.GetComponent<Transform> ().localPosition = new Vector3(-0.5f, -0.5f, -20f);
        }

        if (walls.wallxy) {
            GameObject newWallXY = Instantiate (grid.wallXY);
            newWallXY.transform.SetParent (cellTransform);
            newWallXY.GetComponent<Transform> ().localPosition = new Vector3(0.5f, -0.5f, -22f);
        }
    }

    /// <summary>
    /// spawn des gameobjects nets de la cell
    /// </summary>
    /// <param name="cellTransform"> cell to modify </param>
    /// <param name="nets"> nets to add and display</param>
    /// <param name="grid"> grid object </param>
    void SpawnNets (Transform cellTransform, Cell.Nets nets, Grid grid) {
        //check if netx/nety/netxy exists and instanciate them at the correct place
        if (nets.netx) {
            GameObject newNetX = Instantiate (grid.netX);
            newNetX.transform.SetParent (cellTransform);
            newNetX.GetComponent<Transform> ().localPosition = new Vector3(0.5f, -0.5f, -15f);
        }

        if (nets.nety) {
            GameObject newNetY = Instantiate (grid.netY);
            newNetY.transform.SetParent (cellTransform);
            newNetY.GetComponent<Transform> ().localPosition = new Vector3(-0.5f, -0.5f, -15f);
        }
    }


    /// <summary>
    /// spawn des gameobjects triggers de la cell
    /// </summary>
    /// <param name="cellTransform"> Cell to modify </param>
    /// <param name="trigger"> trigger to add and display </param>
    /// <param name="grid"> grid object </param>
    void SpawnTriggers (Transform cellTransform, Cell.Trigger trigger, Grid grid) {
        //is there a trigger ?
        if (trigger.isTrigger) {
            //what trigger ?
            switch (trigger.triggerType) {
            case -1: //trigger random
                GameObject newTriggerRandom = Instantiate (grid.randomTrigger);
                newTriggerRandom.transform.SetParent (cellTransform);
                newTriggerRandom.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 0: //trigger right
                GameObject newTriggerR = Instantiate (grid.rotateR);
                newTriggerR.transform.SetParent (cellTransform);
                newTriggerR.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 1: //trigger left
                GameObject newTriggerL = Instantiate (grid.rotateL);
                newTriggerL.transform.SetParent (cellTransform);
                newTriggerL.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 2: //trigger upside down
                GameObject newTriggerUD = Instantiate (grid.rotateUD);
                newTriggerUD.transform.SetParent (cellTransform);
                newTriggerUD.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 3: //trigger gravity reset
                GameObject newTriggerGravity = Instantiate (grid.gravityReset);
                newTriggerGravity.transform.SetParent (cellTransform);
                newTriggerGravity.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 4: //trigger rotation choice
                GameObject newTriggerTurnChoice = Instantiate (grid.rotateChoice);
                newTriggerTurnChoice.transform.SetParent (cellTransform);
                newTriggerTurnChoice.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 5: //trigger translate Right
                GameObject newTriggerTranslateRight = Instantiate (grid.translationR);
                newTriggerTranslateRight.transform.SetParent (cellTransform);
                newTriggerTranslateRight.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 6: //trigger translate Left
                GameObject newTriggerTranslateLeft = Instantiate (grid.translationL);
                newTriggerTranslateLeft.transform.SetParent (cellTransform);
                newTriggerTranslateLeft.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 7: //trigger translate Up
                GameObject newTriggerTranslateUp = Instantiate (grid.translationU);
                newTriggerTranslateUp.transform.SetParent (cellTransform);
                newTriggerTranslateUp.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 8: //trigger translate Down
                GameObject newTriggerTranslateDown = Instantiate (grid.translationD);
                newTriggerTranslateDown.transform.SetParent (cellTransform);
                newTriggerTranslateDown.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            default:
                break;
            }
        }
    }

    /// <summary>
    /// spawn des gameobjects triggers de la cell
    /// </summary>
    /// <param name="cellTransform"> Cell to modify </param>
    /// <param name="pawn"> pawn to add and display </param>
    /// <param name="grid"> grid object </param>
    void SpawnPawns (Transform cellTransform, Cell.Pawn pawn, Grid grid) {
        //is there a pawn ?
        if (pawn.isPawn) {
            //what pawn ?
            switch (pawn.pawnType) {
            case -1: //pawn neutral
                GameObject newPawnNeutral = Instantiate (grid.pawnNeutral);
                newPawnNeutral.transform.SetParent (cellTransform);
                newPawnNeutral.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 0: //pawn player1
                GameObject newPawnPlayer1 = Instantiate (grid.pawnPlayer1);
                newPawnPlayer1.transform.SetParent (cellTransform);
                newPawnPlayer1.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 1: //pawn player2
                GameObject newPawnPlayer2 = Instantiate (grid.pawnPlayer2);
                newPawnPlayer2.transform.SetParent (cellTransform);
                newPawnPlayer2.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            case 2: //pawn common
                GameObject newPawnCommon = Instantiate (grid.pawnCommon);
                newPawnCommon.transform.SetParent (cellTransform);
                newPawnCommon.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -10);
                break;
            default:
                break;
            }
        }
    }

    /// <summary>
    /// Delete existing wall or trigger or pawn
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="holderName"></param>
    void DeleteExistingCellChild (Transform cell, string holderName) {
        if (cell.Find (holderName)) {
             #if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall+=()=> {
                DestroyImmediate (cell.Find (holderName).gameObject);
            };
            #endif
        }
    }

    /// <summary>
    /// Transforme les coordonnées en position
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    Vector3 CoordToPosition (int x, int y) {
        return new Vector3 (-currentGrid.gridSize.x / 2 + 0.5f + x, -currentGrid.gridSize.y / 2 + 0.5f + y, 0);
    }
}