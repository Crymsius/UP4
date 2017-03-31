using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Grid : MonoBehaviour {
	public Coord gridSize;

//	[Range(0,1)]
//	public float outlinePercent;

	public GameObject firstTile; // Linkée depuis le dossier Prefabs

	public GameObject firstWallX;
	public GameObject firstWallY;
	public GameObject firstWallXY;

	public GameObject rotateR;
	public GameObject rotateL;
	public GameObject rotateUD;

}
