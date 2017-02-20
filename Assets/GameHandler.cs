using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour {

    /* INPUT : les cases sélectionnées par le joueur actif
     * TRAITEMENTS : 
     * OUTPUT : envoie à la grille le combo Joueur actif + cellule sélectionnée
     */ 

    public string activePlayer { get; set; }
    public MecanismHandler myMecanisms { get; set; }

	// Use this for initialization
	void Start () {
        myMecanisms = gameObject.GetComponent<MecanismHandler>();
	}

    public void PutAPawn(int coords) {
        myMecanisms.PawnFall(coords, activePlayer);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
