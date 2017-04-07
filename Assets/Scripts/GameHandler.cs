using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

	/* INPUT : les cases sélectionnées par le joueur actif
     * TRAITEMENTS : 
     * OUTPUT : envoie à la grille le combo Joueur actif + cellule sélectionnée
     */ 

	public int activePlayer { get; set; }
	public GameObject player1;
	public GameObject player2;

	public bool isOver; 

	public MechanismHandler myMechanisms { get; set; }

	// Use this for initialization
	void Start () {
		activePlayer = 0; 
		myMechanisms = gameObject.GetComponent<MechanismHandler> ();
		NextTurn();
	}

	public void PutAPawn (Cell callingCell) { // fonction déclenchée par un clic sur cellule de la grille
		if (myMechanisms.PawnFallCalculation (callingCell, activePlayer, false)) {
			NextTurn ();
		}
	}

	public void NextTurn ()
	{
		activePlayer = 1 - activePlayer; // si jamais partie à plus de 2, ne marche plus
		myMechanisms.currentPawn = (activePlayer == 1) ? player1 : player2;
	}

	public void GameOver (int winner) {
		print ("Player " + winner + " is the winner"); 
	}

	// Update is called once per frame
	void Update () {

	}
}