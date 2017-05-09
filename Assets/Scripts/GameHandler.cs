using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// INPUT : les cases sélectionnées par le joueur actif
/// TRAITEMENTS : Gestion d'un Puissance 4
/// OUTPUT : envoie à la grille le combo Joueur actif + cellule sélectionnée
/// </summary>
public class GameHandler : MonoBehaviour {
    public int activePlayer { get; set; }
    public GameObject player1;
    public GameObject player2;
    public bool running = false;
    public bool isOver; 
    public GameObject EndPanel;
    public MechanismHandler myMechanisms { get; set; }

    // Use this for initialization
    void Start () {
        EndPanel.SetActive (false);
        activePlayer = 0;
        myMechanisms = gameObject.GetComponent<MechanismHandler> ();
        NextTurn();
    }

    /// <summary>
    /// appellée lors du clic sur une case
    /// 1) Empêche un autre pion d'être joué pendant l'animation de chute
    /// 2) Appel du traitement du pion
    /// 3) Appel du nouveau tour
    /// </summary>
    /// <param name="callingCell"> cell que le joueur vient de cliquer </param>
    /// <returns></returns>
    public IEnumerator PutAPawn (Cell callingCell) {
        running = true;
        yield return StartCoroutine (myMechanisms.PawnFallCalculation (callingCell, activePlayer, false));
        NextTurn ();
    }

    /// <summary>
    /// Rétabli la possibilité de jouer et change le joueur actif ainsi que le pion correspondant
    /// </summary>
    public void NextTurn () {
        running = false;
        activePlayer = 1 - activePlayer; // si jamais partie à plus de 2, ne marche plus
        myMechanisms.currentPawn = (activePlayer == 1) ? player1 : player2;
    }

    /// <summary>
    /// Gère la fin de partie
    /// </summary>
    /// <param name="winner"></param>
    public void GameOver (int winner) {
        EndPanel.SetActive (true);
        print ("Player " + winner + " is the winner");
    }
}