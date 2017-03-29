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
		//gridScript.dictionaryCoordCells = new UniTools.UniDictionary <Coord,Cell> ();
		//gridScript.dictionaryCoordCells = new MyDictionary <Coord,Cell> ();
		//gridScript.dictionaryCoordCells = new Dictionary <Coord,Cell> ();
//		List<Coord> coords = new List<Coord> ();
//		List<Cell> cells = new List<Cell> ();
	
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
				//gridScript.dictionaryCoordCells.Add (cellScript.coordinates, cellScript); 
//				cells.Add (cellScript);
//				coords.Add (cellScript.coordinates);
			}
		}
//		Cell testCell;
//		//Debug.Log(gridScript.dictionaryCoordCells.TryGetValue(new Coord(1,1), out testCell).ToString());
//		Coord testCoord = new Coord(1,1);
//		int indexTestCell = gridScript.coords.IndexOf(testCoord);
//		Debug.Log(gridScript.cells[indexTestCell]);
	}

	// Transforme les coordonnées en position
	Vector3 CoordToPosition (int x, int y) {
		return new Vector3 (-currentGrid.gridSize.x / 2 + 0.5f + x, -currentGrid.gridSize.y / 2 + 0.5f + y, 0);
	}

	//gridHolder:  permet d'avoir une fausse grille pour host les coordonnées, tout en ensuite transmis au Grid
	[System.Serializable]
	public class GridHolder {
		public Coord gridSize;
	}
}