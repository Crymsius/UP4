using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour {

	public Grid[] grids; //liste de tous les lvls
	public int gridIndex; //index de la grid qu'on regarde

	public Transform cellPrefab;

	[Range(0,1)]
	public float outlinePercent;

	List<Coord> allCellCoords;

	Grid currentGrid;

	void Start() {
		GenerateGrid ();
	}

	public void GenerateGrid() {

		currentGrid = grids[gridIndex]; //on setup l'index de la grid qu'on modifie.


		// Generates coords
		allCellCoords = new List<Coord> ();
		for (int x = 0; x < currentGrid.gridSize.x; x ++) {
			for (int y = 0; y < currentGrid.gridSize.y; y ++) {
				
			}
		}

		// Create grid holder object
		string holderName = "Generated Grid";
		if (transform.FindChild (holderName)) {
			DestroyImmediate(transform.FindChild(holderName).gameObject);
		}

		Transform gridHolder = new GameObject (holderName).transform;
		gridHolder.parent = transform;

		// Spawning cells
		for (int x = 0; x < currentGrid.gridSize.x; x ++) {
			for (int y = 0; y < currentGrid.gridSize.y; y ++) {
				Vector3 cellPosition = CoordToPosition (x,y);
				Transform newCell = Instantiate(cellPrefab, cellPosition, Quaternion.identity) as Transform;
				newCell.localScale = Vector3.one * (1 - outlinePercent);
				newCell.parent = gridHolder;

				Cell CellScript = newCell.GetComponent<Cell>();
				CellScript.coordinates = new Coord (x, y); //ajoute les coordonnées de chacune des Cells
				allCellCoords.Add(new Coord(x,y));
			//	currentGrid.dictionaryCoordCells.Add(new Coord(x,y), CellScript);
			}
		}
	}

	Vector3 CoordToPosition(int x, int y) {
		return new Vector3 (-currentGrid.gridSize.x / 2 + x, -currentGrid.gridSize.y / 2 + y, 0);
	}

	[System.Serializable]
	public class Grid {
		
		public Coord gridSize;


//			public GameObject firstCell; // Linkée depuis le dossier Prefabs
//			public GameObject firstWallD;
//			public GameObject firstWallB;
//			public GameObject firstWallC;
//			public GameObject rotateR;
//			public GameObject rotateL;

		//public Dictionary<Coord, Cell> dictionaryCoordCells;
	}

}