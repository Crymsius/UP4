﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell : MonoBehaviour {
	
	public Coord coordinates;
	public GameHandler myHandler { get; set; }

	public bool available;	// Peut-on placer un pion dessus ?
	public string content { get; set; }		// A quel joueur appartient le pion ? 

	void Start () {
		myHandler = GameObject.Find ("GeneralHandler").GetComponent<GameHandler> ();
	}

	void OnMouseDown () // déclenché avec clic sur la grille
	{
		print (coordinates.x + " ; " + coordinates.y);
		if (available)
			myHandler.PutAPawn (this);
	}

	// Update is called once per frame
	void Update () {

	}
}
