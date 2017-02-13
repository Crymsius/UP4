using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    /* La cellule constitue une case de la grille
     * Elle retient les informations à propos de la case qu'elle décrit
     * De base, ce script est automatiquement attaché à chaque nouvelle case créée. Il suffit d'écrire "gameobject" pour obtenir l'instance du GameObject parent
     */

    public List<string> walls = new List<string>(); // comprend les murs à droite (D), en bas (B), et coin bas-gauche (C)
    public bool available { get; set; } // Peut-on placer un pion dessus ?
    public string content { get; set; } // Y a-t-il un trigger, pion ou autre élément spécial ? 

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
