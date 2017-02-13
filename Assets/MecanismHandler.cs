using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MecanismHandler : MonoBehaviour {

    /*
     * INPUTS : GameObject grille, les coordonnées de la case sur lequel le joueur a décidé de jouer
     * TRAITEMENTS : Analyse la chute du pion, déclenche tous les triggers, détecte les puissance 4
     * OUTPUTS : fin de tour auprès du GestionnairePartie, avec info puissance 4
    */

        public Grid myGrid { get; set; }

	// Use this for initialization
	void Start () {
        // following instructions are a test
        myGrid = GameObject.Find("GridRecipient").GetComponent<Grid>();
        myGrid.createGrid("N-N-N-N-N-N-N+N-V-V-V-V-V-N+N-V-V-V-V-V-N+N-V-V-V-V-V-N+N-V-V-V-V-V-N+N-V-V-V-V-V-N+N-N-N-N-N-N-N"); // grille 5*5
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
