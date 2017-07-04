using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class Grid : MonoBehaviour {
    public Coord gridSize;

//	[Range(0,1)]
//	public float outlinePercent;

    public GameObject wallX;
    public GameObject wallY;
    public GameObject wallXY;
    public GameObject netX;
    public GameObject netY;

    public GameObject rotateR;
    public GameObject rotateL;
    public GameObject rotateUD;
    public GameObject rotateChoice;

    public GameObject gravityReset;
    public GameObject randomTrigger;

    public GameObject translationR;
    public GameObject translationL;
    public GameObject translationU;
    public GameObject translationD;

    public GameObject pawnNeutral;
    public GameObject pawnPlayer1;
    public GameObject pawnPlayer2;
    public GameObject pawnCommon;

    public GameObject cellHidden;
    public GameObject cellCover;
}