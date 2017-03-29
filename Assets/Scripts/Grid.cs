using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Grid : MonoBehaviour {
	public Coord gridSize;

//	[Range(0,1)]
//	public float outlinePercent;

	public GameObject firstTile; // Linkée depuis le dossier Prefabs
	public GameObject firstWallD;
	public GameObject firstWallB;
	public GameObject firstWallC;
	public GameObject rotateR;
	public GameObject rotateL;

}
