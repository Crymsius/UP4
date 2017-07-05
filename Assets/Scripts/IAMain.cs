using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IAMain : MonoBehaviour {

    public Atlas myAtlas;
    public string recapGame;

    public DecisionTreeNode mainNode;
    public List<string> typePlayers;
    public TMP_Text bugReportText;
    public GameObject bugReportPanel;
    public GameObject overlayPanel;

    // Use this for initialization
    void Start () {
        bugReportText = GameObject.Find ("BugReportText").GetComponent<TMP_Text> ();
        bugReportPanel = GameObject.Find ("BugReportPanel");
		overlayPanel = GameObject.Find ("OverlayPanel");
        bugReportPanel.SetActive (false);
    }

    public void settingGrid(Coord c)
    {
        Matrix board = new Matrix(c, myAtlas);
        Matrix trigPositions = new Matrix(c, myAtlas);

        recapGame = "";

		foreach (Cell cell in myAtlas.gridDictionary.Values) {
			if (cell.full)
				board.values [cell.coordinates] = -2;
			else if (cell.pawn.isPawn)
				board.values [cell.coordinates] = (cell.pawn.pawnType == -1) ? 2 : ((cell.pawn.pawnType == 2) ? 3 : cell.pawn.pawnType);
		}
        
        if(typePlayers.Contains("IA"))
            mainNode = new DecisionTreeNode(1, 0, 4, board, myAtlas, new Coord(0,-1), 0, new Dictionary<Coord, DecisionTreeNode>(), typePlayers);
        else 
            mainNode = new DecisionTreeNode(1, 0, 1, board, myAtlas, new Coord(0,-1), 0, new Dictionary<Coord, DecisionTreeNode>(), typePlayers);
        mainNode.DeploymentTree();

        //PrintAllPlays();
    }

    public void GetCurrentPlay(Coord play) { // Récupère le play qui vient d'être joué pour actualiser l'arbre de décision.
        recapGame += play.Stringify()+"-";

        if (mainNode.children.ContainsKey (play))
            mainNode = mainNode.children [play]; // Le reste est envoyé au GarbageCollector, normalement...
		else{ // détection d'un bug !
            string crashText = "L'IA a crashé" +
                               "\n\nFélicitations, vous avez débusqué un bug !" +
                               "\nPour nous aider à corriger l'application, envoyez-nous un screenshot de cet écran" +
                               "\n\n" + recapGame;
            bugReportText.text = crashText;
            bugReportPanel.SetActive (true);
            overlayPanel.SetActive (true);

            print ("JE CRASHE !");
        }

        //PrintAllPlays();

        mainNode.Maj_Depth(0);
        mainNode.DeploymentTree();
    }

    public void IA_Play() { // compare les scores de chaque coup et joue le meilleur
        mainNode.MAJ_Scores();

        List<Coord> myPlays = new List<Coord> (); 
        List<float> myScores = new List<float> ();
        foreach (Coord play in mainNode.children.Keys) {
            myPlays.Add (play);
            myScores.Add (mainNode.children [play].score);
        }

        if (!mainNode.position.isVictory) {
            Coord decision = VariantSelector.variant==0 ? RandomizeDecision (myPlays, myScores) : mainNode.position.GetHighestPlayPosition(RandomizeDecision (myPlays, myScores), mainNode.gravity) ;
            StartCoroutine (GameObject.Find ("GeneralHandler").GetComponent<GameHandler> ().PutAPawn (myAtlas.gridDictionary [decision]));
        }
    }
    
    public Coord RandomizeDecision(List<Coord> myPlays, List<float> myScores){
        if (myScores.Count != 1) {

            float sMin = myScores[0], sMax = myScores[0];
            int iMin = 0, iMax = 0;
            for (int i=0; i<myScores.Count; i++) { // get score min and max
                iMin = myScores [i] < sMin ? i : iMin;
                sMin = myScores [i] < sMin ? myScores [i] : sMin;
                iMax = myScores [i] > sMax ? i : iMax;
                sMax = myScores [i] > sMax ? myScores [i] : sMax;
            }

            List<int> indConsidered = new List<int>(); //get only interesting scores
            for (int i=0; i<myScores.Count; i++)
                if (myScores [i] > sMin + 0.9 * (sMax - sMin))
                    indConsidered.Add(i);

            int nb = indConsidered.Count;
            List<int> indSorted = new List<int> ();
            while (indSorted.Count<nb) {
                iMax = indConsidered[0]; sMax = myScores [iMax];
                for (int i = 0; i < indConsidered.Count; i++) {
                    iMax = myScores [i] > sMax ? i : iMax;
                    sMax = myScores [i] > sMax ? myScores [i] : sMax;
                }
                indSorted.Add (iMax);
                indConsidered.Remove (iMax);
            }

            int choice = 0; bool select = true;
            while (select && choice < indSorted.Count-1)
            {
                if (Random.Range(0f, 1f) < 0.3f) // influer sur cette proba pour donner à l'IA un choix plus ou moins random
                    choice++;
                else
                    select = false;
            }

            // print("Score choisi : "+ (choice+1));

            return myPlays [indSorted[choice]];

        } else
            return myPlays [0];
    }

    public void PrintAllPlays() { // fonction d'observation
        string s = "";
        foreach (Coord key in mainNode.children.Keys)
            s += key.Stringify() + " : " + mainNode.children[key].score + " // ";
        print(s);

        mainNode.position.Stringify();
    }
    
    // Update is called once per frame
    void Update () {
		
    }
}
