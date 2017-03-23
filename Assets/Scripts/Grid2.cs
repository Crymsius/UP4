using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Grid2 : MonoBehaviour {
	public Coord gridSize;

	[Range(0,1)]
	public float outlinePercent;
		

	public List<Coord> allCellCoords;

	public GameObject firstTile; // Linkée depuis le dossier Prefabs
	public GameObject firstWallD;
	public GameObject firstWallB;
	public GameObject firstWallC;
	public GameObject rotateR;
	public GameObject rotateL;

	//public Dictionary<Coord, Cell> dictionaryCoordCells;
}
