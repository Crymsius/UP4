using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// INPUT : les cases sélectionnées par le joueur actif
/// TRAITEMENTS : Gestion d'un Puissance 4
/// OUTPUT : envoie à la grille le combo Joueur actif + cellule sélectionnée
/// </summary>

/// IMPORTANT : POUR SWITCH ENTRE LES VARIANTES, comment - un comment [switchVar]
public class GameHandler : MonoBehaviour {
    public int activePlayer { get; set; }
    public GameObject player1;
    public GameObject player2;
    public List<string> typePlayer { get; set; } // human or IA
    public bool running = false;
    public bool isOver; 
    public GameObject overlayPanel;
    public GameObject gameOverPanel;
    
    /// [switchVar]
     public MechanismHandler myMechanisms { get; set; }
    //public MechanismHandlerVariant myMechanisms;
    /// [switchVar]

    // Use this for initialization
    void Start () {
        isOver = false;
        activePlayer = 0;

        typePlayer = new List<string>() { "human", "IA" }; // Test, à remplacer par un appel au lancement d'une partie
        GameObject.Find("IAHandler").GetComponent<IAMain>().typePlayers = typePlayer;

        /// [switchVar]
        myMechanisms = gameObject.GetComponent<MechanismHandler> ();
        //myMechanisms = gameObject.GetComponent<MechanismHandlerVariant> ();
        /// [switchVar]
        
        //NextTurn ();
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
        yield return StartCoroutine (myMechanisms.PawnFallCalculation (callingCell, activePlayer, false, true));

        bool available = false;
        foreach (Cell cell in myMechanisms.gridAtlas.gridDictionary.Values)
            available = available || cell.available;

        if (available)
            NextTurn();
        else
            GameOver();
    }

    /// <summary>
    /// Rétabli la possibilité de jouer et change le joueur actif ainsi que le pion correspondant.
    /// Si le joueur est une IA, on maintient l'impossibilité de cliquer.
    /// </summary>
    public void NextTurn () {
        //running = false;
        activePlayer = 1 - activePlayer; // si jamais partie à plus de 2, ne marche plus
        running = typePlayer[activePlayer] == "human" ? false : true;
        myMechanisms.currentPawn = (activePlayer == 1) ? player1 : player2;

        if (typePlayer[activePlayer] == "IA")
            GameObject.Find("IAHandler").GetComponent<IAMain>().IA_Play();
    }

    /// <summary>
    /// Gère la fin de partie
    /// </summary>
    /// <param name="winner"></param>
    public void GameOver () {
        isOver = true;
        overlayPanel.SetActive (true);
        overlayPanel.GetComponent<CanvasRenderer> ().SetAlpha(0);

        int p0 = GameObject.Find("IAHandler").GetComponent<IAMain>().mainNode.position.inVictory[0].Count;
        int p1 = GameObject.Find("IAHandler").GetComponent<IAMain>().mainNode.position.inVictory[1].Count;
        int winner = p1 != p0 ? (p1 > p0 ? 1 : 0) : -1;

        // gameOverPanel.SetActive (true);
        if (winner != -1)
            print("Player " + winner + " is the winner " + Mathf.Max(p1, p0) + "-" + Mathf.Min(p1, p0));
        else
            print("Match nul");
    }
}