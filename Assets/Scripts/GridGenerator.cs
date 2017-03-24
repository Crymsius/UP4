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

	void Start() {
		GenerateGrid ();
	}

	public void GenerateGrid() {

		currentGrid = grids[gridIndex]; //on setup l'index de la grid qu'on modifie.

		allCellCoords = new List<Coord> ();

		// Create grid holder object
		string holderName = "Generated Grid(Clone)";
		if (transform.FindChild (holderName)) {
			DestroyImmediate(transform.FindChild(holderName).gameObject);
		}

//		Transform gridHolder = new GameObject (holderName).transform;
//		gridHolder.parent = transform;

		Transform newGrid = Instantiate (gridPrefab, Vector3.zero, Quaternion.identity) as Transform; 
		newGrid.parent = transform;
		Grid gridScript = newGrid.GetComponent<Grid> ();
		gridScript.gridSize = currentGrid.gridSize;	

		//currentGrid.dictionaryCoordCells = new Dictionary <Coord,Cell> ();
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

				// ajoute le couple coordonnée - cell dans le dictionaire de la grille
				//currentGrid.dictionaryCoordCells.Add (cellScript.coordinates, cellScript); 
			}
		}
	}

	Vector3 CoordToPosition(int x, int y) {
		
		return new Vector3 (-currentGrid.gridSize.x / 2 + 0.5f + x, -currentGrid.gridSize.y / 2 + 0.5f + y, 0);
	}

	[System.Serializable]
	public class GridHolder {
		public Coord gridSize;
	}
}