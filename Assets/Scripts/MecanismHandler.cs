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

    public GameObject neutralPawn; // Linked depuis le dossier Prefabs
    public Sprite currentSprite { get; set; }

    public string gravity { get; set; } // direction de chute des pions
    public Dictionary<string, int> fallIntegers = new Dictionary<string, int>() { { "up", 1 }, { "down", -1 }, { "left", -100 }, { "right", 100 }, { "UL", -99 }, { "UR", 101 }};
    
    // Use this for initialization
    void Start () {
        gravity = "down";
        // following instructions are a test
        myGrid = GameObject.Find("GridRecipient").GetComponent<Grid>();
        myGrid.createGrid("N-N-N-N-N-N-N-N+N-VD-V-V-V-V-V-N+N-V-VC-VB-V-VD-VC-N+N-V-V-V-V-V-V-N+N-V-V-V-V-V-V-N+N-V-V-V-V-V-V-N+N-V-VC-VB-V-V-VC-N+N-N-N-N-N-N-N-N");
	}
    
    public bool PawnFallCalculation(Cell startCell, int player) // Player peut etre une structure qui contient les visuels des pions, les noms, les taunts, etc...
    {
        /* Calcule où le pion va s'arrêter de chuter depuis les coordonnées ou il a été lâché.
         * Retient tous les triggers qui ont été déclenchés au passage du pion.
         */
        int interCoords = startCell.coordinates;
        Cell currentCell = startCell;
        bool falling = true;
        
        //TODO : retenir les triggers rencontrés;
        while (falling)
        {
            currentCell = myGrid.coordinates[interCoords];
            Cell nextCell = myGrid.coordinates[interCoords + fallIntegers[gravity]];
            if (nextCell.available &&
                (gravity == "down" && !currentCell.walls.Contains("B")
                || gravity == "up" && !nextCell.walls.Contains("B")
                || gravity == "right" && !currentCell.walls.Contains("D")
                || gravity == "left" && !nextCell.walls.Contains("D")
                ))
            {
                interCoords += fallIntegers[gravity];
            }
            else
                falling = false;
        }
        //script plaçant le pion et lançant le visuel
        GameObject newPawn = Instantiate(neutralPawn);
        newPawn.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = currentSprite;
        newPawn.transform.SetParent(startCell.transform);
        newPawn.GetComponent<Transform>().localPosition = Vector3.zero;
        PawnFallDo(newPawn, currentCell, player);
        //TODO : méthode activant tous les triggers et lançant le visuel
        //script de vérification de la puissance 4 et lançant le visuel
        return checkAlign4(currentCell, player);
    }
    public void PawnFallDo(GameObject pawn, Cell endCell, int player) // possibilité d'exporter plus tard la partie graphique de la méthode ailleurs
    {
        pawn.transform.SetParent(endCell.transform);
        float coeff = 1f;
        while (coeff > 0) { //not working : pas d'update visuel depuis une while loop
            pawn.GetComponent<Transform>().localPosition = coeff * pawn.GetComponent<Transform>().localPosition;
            coeff -= Time.deltaTime;
        }
        endCell.available = false;
        endCell.content = player.ToString();
    }
    public bool checkAlign4(Cell newFilled, int player) { //ne doit pas prendre les murs en compte correctement si rotation de grille... a tester

        int currentCoords = newFilled.coordinates;
        int startCoords; bool isWinner = false;
        foreach (string i in new List<string>() { "right", "UR", "up", "UL" }) //test selon les 4 directions
        {
            startCoords = currentCoords;
            while (startCoords / 100 > 0 && startCoords % 100 > 0)
                startCoords -= fallIntegers[i]; // On a rejoint le bord du graphique, prêts à balayer en sens inverse.
            int count = 0;
            while (myGrid.coordinates.ContainsKey(startCoords)) // On compte les pions CONSECUTIFS du joueur suivant la direction. Arrivé à 4 c'est la victoire.
            {
                if (myGrid.coordinates[startCoords].content == player.ToString())
                {
                    if (count + 1 >= 4)
                        isWinner = true;
                    else if (!isBlocked(startCoords, i)) count++;
                    else count = 0;
                }
                else count = 0;
                startCoords += fallIntegers[i];
            }
        }
        return isWinner;
    }
    public bool isBlocked(int coords, string direction) {//vérifie si un mur ne bloque pas l'établissement de la puissance 4
        if (direction == "right" && myGrid.coordinates[coords].walls.Contains("D")
            || direction == "up" && myGrid.coordinates[coords + 1].walls.Contains("B")
            || direction == "UR" && myGrid.coordinates[coords + 101].walls.Contains("C")
            || direction == "UL" && myGrid.coordinates[coords + 1].walls.Contains("C")
            )
            return true;
        else return false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
