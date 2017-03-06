using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    /* INPUT : les cases sélectionnées par le joueur actif
     * TRAITEMENTS : 
     * OUTPUT : envoie à la grille le combo Joueur actif + cellule sélectionnée
     */ 

    public int activePlayer { get; set; }
    public Sprite player1;
    public Sprite player2;

    public MecanismHandler myMecanisms { get; set; }

	// Use this for initialization
	void Start () {
        activePlayer = 0; 
        myMecanisms = gameObject.GetComponent<MecanismHandler>();
        NextTurn();
    }

    public void PutAPawn(Cell callingCell) { // fonction déclenchée par un clic sur cellule de la grille
        if (!myMecanisms.PawnFallCalculation(callingCell, activePlayer)) //renvoie TRUE si le joueur remporte la partie
            NextTurn();
        else
            print("It's over"); // Déclencher un script de fin de partie quand on pourra gérer correctement l'event.
    }
    public void NextTurn()
    {
        activePlayer = 1 - activePlayer; // si jamais partie à plus de 2, ne marche plus
        myMecanisms.currentSprite = (activePlayer == 1) ? player1 : player2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
