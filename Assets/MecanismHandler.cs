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
    public string gravity { get; set; } // direction de chute des pions
    public Dictionary<string, int> fallIntegers = new Dictionary<string, int>() { { "up", 100 }, { "down", -100 }, { "left", -1 }, { "right", 1 } };

    // Use this for initialization
    void Start () {
        // following instructions are a test
        myGrid = GameObject.Find("GridRecipient").GetComponent<Grid>();
        myGrid.createGrid("N-N-N-N-N-N-N+N-V-V-V-V-V-N+N-V-V-V-V-V-N+N-V-V-V-V-V-N+N-V-V-V-V-V-N+N-V-V-V-V-V-N+N-N-N-N-N-N-N"); // grille 5*5
	}

    public void PawnFall(int coords, string player) // Player peut etre une structure qui contient les visuels des pions, les noms, les taunts, etc...
    {
        /* Calcule où le pion va s'arrêter de chuter depuis les coordonnées ou il a été lâché.
         * Retient tous les triggers qui ont été déclenchés au passage du pion.
         */
        int interCoords = coords;
        bool falling = true;

        //TODO : retenir les triggers rencontrés;
        while (falling)
        {
            Cell currentCell = myGrid.coordinates[interCoords];
            Cell nextCell = myGrid.coordinates[interCoords + fallIntegers[gravity]];
            if (nextCell.available &&
                (gravity == "down" && currentCell.walls.Contains("B")
                || gravity == "up" && nextCell.walls.Contains("B")
                || gravity == "right" && currentCell.walls.Contains("D")
                || gravity == "left" && nextCell.walls.Contains("D")
                ))
            {
                interCoords += fallIntegers[gravity];
            }
            else
                falling = false;
        }
        //TODO : méthode plaçant le pion et lançant le visuel
        //TODO : méthode activant tous les triggers et lançant le visuel
        //TODO : méthode de vérification de la puissance 4 et lançant le visuel
    }

    // Update is called once per frame
    void Update () {
		
	}
}
