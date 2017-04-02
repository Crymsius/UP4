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
		GenerateGrid ();
	}

	public void GenerateGrid () {

		currentGrid = grids[gridIndex]; //on setup l'index de la grid qu'on modifie.
		allCellCoords = new List<Coord> ();

		// Vérifier que l'object n'existe pas déjà, en cas, le détruire
		string holderName = "Generated Grid(Clone)";
		if (transform.FindChild (holderName)) {
			DestroyImmediate(transform.FindChild(holderName).gameObject);
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
	}

	public void UpdateCells () {
		Grid grid = transform.FindChild ("Generated Grid(Clone)").gameObject.GetComponent<Grid> ();
		List<string> contentNames = new List<string> () {"WallX(Clone)", "WallY(Clone)", "WallXY(Clone)", "TurnRight(Clone)", "TurnLeft(Clone)", "TurnUpsideDown(Clone)"};
		foreach (Transform cellChild in grid.GetComponent<Transform> ()) {
			foreach (string content in contentNames) {
				DeleteExistingCellChild (cellChild, content);
			}
			SpawnWalls (cellChild, grid);
			SpawnTriggers (cellChild, grid);
		}
	}

	void SpawnWalls (Transform cellTransform, Grid grid) {
		Cell cell = cellTransform.GetComponent<Cell> ();
		Cell.Walls walls = cell.walls;

		if (walls.wallx) {
			GameObject newWallX = Instantiate (grid.firstWallX);
			newWallX.transform.SetParent (cellTransform);
			newWallX.GetComponent<Transform> ().localPosition = new Vector3(0.627f, 0, 0);
		}

		if (walls.wally) {
			GameObject newWallY = Instantiate (grid.firstWallY);
			newWallY.transform.SetParent (cellTransform);
			newWallY.GetComponent<Transform> ().localPosition = new Vector3(0, -0.627f, 0);
		}

		if (walls.wallxy) {
			GameObject newWallXY = Instantiate (grid.firstWallXY);
			newWallXY.transform.SetParent (cellTransform);
			newWallXY.GetComponent<Transform> ().localPosition = new Vector3(0.627f, -0.627f, 0);
		}
	}

	void SpawnTriggers (Transform cellTransform, Grid grid) {
		Cell cell = cellTransform.GetComponent<Cell> ();
		Cell.Trigger trigger = cell.trigger;

		if (trigger.isTrigger) {
			switch (trigger.triggerType) {
			case 0: //trigger right 
				GameObject newTriggerR = Instantiate (grid.rotateR);
				newTriggerR.transform.SetParent (cellTransform);
				newTriggerR.GetComponent<Transform> ().localPosition = Vector3.zero;
				break;
			case 1: //trigger left
				GameObject newTriggerL = Instantiate (grid.rotateL);
				newTriggerL.transform.SetParent (cellTransform);
				newTriggerL.GetComponent<Transform> ().localPosition = Vector3.zero;
				break;
			case 2: //trigger upside down
				GameObject newTriggerUD = Instantiate (grid.rotateUD);
				newTriggerUD.transform.SetParent (cellTransform);
				newTriggerUD.GetComponent<Transform> ().localPosition = Vector3.zero;
				break;
			default:
				break;
			}
		}
	}

	//Delete existing wall or trigger 
	void DeleteExistingCellChild (Transform cell, string holderName) {
		if (cell.FindChild (holderName)) {
			DestroyImmediate (cell.FindChild (holderName).gameObject);
		}
	}

	// Transforme les coordonnées en position
	Vector3 CoordToPosition (int x, int y) {
		return new Vector3 (-currentGrid.gridSize.x / 2 + 0.5f + x, -currentGrid.gridSize.y / 2 + 0.5f + y, 0);
	}

	//gridHolder:  permet d'avoir une fausse grille pour host les coordonnées lors de la génération
	[System.Serializable]
	public class GridHolder {
		public Coord gridSize;
	}
}