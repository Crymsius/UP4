using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour {

    public GridHolder[] grids; //liste de tous les lvls
    public int gridIndex; //index de la grid qu'on regarde
    public Transform cellPrefab;
    public Transform gridPrefab;

    [Range(0,1)]
    public float outlinePercent;
    List<Coord> allCellCoords;
    GridHolder currentGrid;

    void Start () {
        gridIndex = LevelLoader.level;
    }

    /// <summary>
    /// factorisation de generate grid et display from save
    /// </summary>
    public void GenerateEditor () {
        StartCoroutine (GenerateGrid ());
        StartCoroutine (DisplayFromSave ());
    }

    /// <summary>
    /// Génère la grille depuis la grille et les cases (mais pas leur contenu !) depuis la grille virtuelle
    /// </summary>
    public IEnumerator GenerateGrid () {
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
                newCell.localScale = Vector3.one * (1 - outlinePercent);
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
        //DisplayFromSave ();
        yield return null;
    }

    /// <summary>
    /// Parcourt la grille physique et affiche le contenu
    /// </summary>
    public void DisplayFromCells () {
        Grid grid = transform.Find ("Generated Grid(Clone)").gameObject.GetComponent<Grid> ();
        List<string> contentNames = new List<string> () {"WallX(Clone)", "WallY(Clone)", "WallXY(Clone)", "TurnRight(Clone)", "TurnLeft(Clone)", "TurnUpsideDown(Clone)", "GravityReset(Clone)", "CellHidden(Clone)"};
        //parcourt de toute la grille physique
        foreach (Transform cellChild in grid.GetComponent<Transform> ()) {
            foreach (string content in contentNames) {
                DeleteExistingCellChild (cellChild, content);
            }
            StartCoroutine (SpawnWalls (cellChild, cellChild.GetComponent<Cell> ().walls, grid));
            StartCoroutine (SpawnTriggers (cellChild, cellChild.GetComponent<Cell> ().trigger, grid));

            //cell invisible ?
            if (cellChild.GetComponent<Cell> ().hidden) {
                cellChild.GetComponentInChildren<CellCover> ().HideCell ();
                GameObject newCellHidden = Instantiate (grid.cellHidden);
                newCellHidden.transform.SetParent (cellChild);
                newCellHidden.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -15);
            }
            //cell bloquée ?
            if (cellChild.GetComponent<Cell> ().full) {
                cellChild.GetComponentInChildren<CellCover> ().HideCell ();
            }
        }
    }

    /// <summary>
    /// sauvegarde des modifications des cells de la grille physique dans la grille virtuelle
    /// </summary>
    public void SaveCells () {
        grids [gridIndex].cells.Clear ();
        Grid grid = transform.Find ("Generated Grid(Clone)").gameObject.GetComponent<Grid> ();
        foreach (Transform cellChild in grid.GetComponent<Transform> ()) {
            grids[gridIndex].cells.Add (
                //on attribue les carac de la cell physiques à une nouvelle cell virtuelle
                new CellHolder {
                    walls = cellChild.GetComponent<Cell> ().walls, 
                    triggers = cellChild.GetComponent<Cell> ().trigger, 
                    available = cellChild.GetComponent<Cell> ().available,
                    hidden = cellChild.GetComponent<Cell> ().hidden,
                    full = cellChild.GetComponent<Cell> ().full
                }
            );
        }
    }

    /// <summary>
    /// Parcourt la grille virtuelle et applique le contenu dans la grid physique
    /// </summary>
    public IEnumerator DisplayFromSave () {
        int i = 0;
        Grid grid = transform.Find ("Generated Grid(Clone)").gameObject.GetComponent<Grid> ();
        List<string> contentNames = new List<string> () {"WallX(Clone)", "WallY(Clone)", "WallXY(Clone)", "TurnRight(Clone)", "TurnLeft(Clone)", "TurnUpsideDown(Clone)", "GravityReset(Clone)"};
        //parcourt de toute la grille physique
        foreach (Transform cellChild in grid.GetComponent<Transform> ()) {
            foreach (string content in contentNames) {
                //on supprime le contenu des cells pour ne pas avoir de doublon
                DeleteExistingCellChild (cellChild, content);
            }
            //on attribue les carac des cells virtuelles aux cells de la grille
            cellChild.GetComponent<Cell> ().walls = currentGrid.cells[i].walls;
            cellChild.GetComponent<Cell> ().trigger = currentGrid.cells[i].triggers;
            cellChild.GetComponent<Cell> ().hidden = currentGrid.cells [i].hidden;
            cellChild.GetComponent<Cell> ().full = currentGrid.cells [i].full;
            cellChild.GetComponent<Cell> ().available = currentGrid.cells [i].available;
            //cell invisible ?
            if (cellChild.GetComponent<Cell> ().hidden) {
                cellChild.GetComponentInChildren<CellCover> ().HideCell ();
                GameObject newCellHidden = Instantiate (grid.cellHidden);
                newCellHidden.transform.SetParent (cellChild);
                newCellHidden.GetComponent<Transform> ().localPosition = new Vector3 (0, 0, -15);
            }
            //cell bloquée ?
            if (cellChild.GetComponent<Cell> ().full) {
                cellChild.GetComponentInChildren<CellCover> ().HideCell ();
            }
            //spawn des murs et des triggers
            yield return StartCoroutine (SpawnWalls (cellChild, cellChild.GetComponent<Cell> ().walls, grid));
            yield return StartCoroutine (SpawnTriggers (cellChild, cellChild.GetComponent<Cell> ().trigger, grid));
            i++;
        }
        yield return null;
    }

    /// <summary>
    /// spawn des gameobjects wall de la cell
    /// </summary>
    /// <param name="cellTransform"> cell to modify </param>
    /// <param name="walls"> walls to add and display</param>
    /// <param name="grid"> grid object </param>
    IEnumerator SpawnWalls (Transform cellTransform, Cell.Walls walls, Grid grid) {
        //check if wallx/wally/wallxy exists and instanciate them at the correct place
        if (walls.wallx) {
            GameObject newWallX = Instantiate (grid.firstWallX);
            newWallX.transform.SetParent (cellTransform);
            newWallX.GetComponent<Transform> ().localPosition = new Vector3(0.5f, -0.5f, -20f);
        }

        if (walls.wally) {
            GameObject newWallY = Instantiate (grid.firstWallY);
            newWallY.transform.SetParent (cellTransform);
            newWallY.GetComponent<Transform> ().localPosition = new Vector3(-0.5f, -0.5f, -20f);
        }

        if (walls.wallxy) {
            GameObject newWallXY = Instantiate (grid.firstWallXY);
            newWallXY.transform.SetParent (cellTransform);
            newWallXY.GetComponent<Transform> ().localPosition = new Vector3(0.5f, -0.5f, -22f);
        }
        yield return null;
    }

    /// <summary>
    /// spawn des gameobjects triggers de la cell
    /// </summary>
    /// <param name="cellTransform"> Cell to modify </param>
    /// <param name="trigger"> trigger to add and display </param>
    /// <param name="grid"> grid object </param>
    IEnumerator SpawnTriggers (Transform cellTransform, Cell.Trigger trigger, Grid grid) {
        //is there a trigger ?
        if (trigger.isTrigger) {
            //what trigger ?
            switch (trigger.triggerType) {
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
            default:
                break;
            }
        }
        yield return null;
    }

    /// <summary>
    /// Delete existing wall or trigger
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="holderName"></param>
    void DeleteExistingCellChild (Transform cell, string holderName) {
        if (cell.Find (holderName)) {
            DestroyImmediate (cell.Find (holderName).gameObject);
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

    /// <summary>
    /// grille virtuelle contenant les cellules virtuelles. Sert de sauvegarde des différentes grilles puisque serializable
    /// </summary>
    [System.Serializable]
    public class GridHolder {
        public Coord gridSize;
        public List<CellHolder> cells;
    }

    /// <summary>
    /// cell virtuelle contenant les infos d'une cellule. Sert de sauvegarde du contenu d'une grille
    /// </summary>
    [System.Serializable]
    public class CellHolder {
        public Cell.Walls walls;
        public Cell.Trigger triggers;
        public bool hidden;
        public bool full;
        public bool available;
    }
}