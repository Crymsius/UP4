using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    /* La cellule constitue une case de la grille
     * Elle retient les informations à propos de la case qu'elle décrit
     * De base, ce script est automatiquement attaché à chaque nouvelle case créée. Il suffit d'écrire "gameobject" pour obtenir l'instance du GameObject parent
     * OUTPUT : Envoie à la grille ses coordonnées si un joueur l'a sélectionnée
     */
    public GameHandler myHandler { get; set; }

    public List<string> wallsAndTriggers = new List<string>(); // comprend les murs à droite (D), en bas (B), et coin bas-gauche (C) + triggers droit (R) et gauche (L)
    public bool available { get; set; } // Peut-on placer un pion dessus ?
    public string content { get; set; } // A quel joueur appartient le pion ? 

    public int coordinates { get; set; }

    // Use this for initialization
    void Start () {
        myHandler = GameObject.Find("GeneralHandler").GetComponent<GameHandler>();
	}

    void OnMouseDown() // déclenché avec clic sur la grille
    {
        if (available)
            myHandler.PutAPawn(this);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
