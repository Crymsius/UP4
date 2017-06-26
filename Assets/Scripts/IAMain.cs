using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMain : MonoBehaviour {

    public Atlas myAtlas;

    public DecisionTreeNode mainNode;
    public List<string> typePlayers;

    // Use this for initialization
    void Start () {

	}

    public void settingGrid(Coord c)
    {
        Matrix board = new Matrix(c, myAtlas);
        Matrix trigPositions = new Matrix(c, myAtlas);

        foreach (Cell cell in myAtlas.gridDictionary.Values)
            if (cell.full)
                board.values[cell.coordinates] = -2;
        
		if(typePlayers.Contains("IA"))
        	mainNode = new DecisionTreeNode(1, 0, 4, board, myAtlas, new Coord(0,-1), 0, new Dictionary<Coord, DecisionTreeNode>(), typePlayers);
		else 
			mainNode = new DecisionTreeNode(1, 0, 1, board, myAtlas, new Coord(0,-1), 0, new Dictionary<Coord, DecisionTreeNode>(), typePlayers);
        mainNode.DeploymentTree();

        //PrintAllPlays();
    }

    public void GetCurrentPlay(Coord play) { // Récupère le play qui vient d'être joué pour actualiser l'arbre de décision.
        if (mainNode.children.ContainsKey(play))
            mainNode = mainNode.children[play]; // Le reste est envoyé au GarbageCollector, normalement...
		else
            print("JE CRASHE !"); // Pour une raison obscure, un coup joué n'a pas été prévu par la grille... Enquêter sur le pourquoi...

        //PrintAllPlays();

        mainNode.Maj_Depth(0);
        mainNode.DeploymentTree();
    }

    public void IA_Play() { // compare les scores de chaque coup et joue le meilleur
        mainNode.MAJ_Scores();

        Dictionary<float,Coord> scores = new Dictionary<float, Coord>();
        foreach (Coord play in mainNode.children.Keys)
            scores.Add(mainNode.children[play].score + Random.Range(-0.01f,0.01f), play);
        
        if (!mainNode.position.isVictory)
            StartCoroutine(GameObject.Find("GeneralHandler").GetComponent<GameHandler>().PutAPawn(myAtlas.gridDictionary[RandomizeDecision(scores)]));
    }
    public Coord RandomizeDecision(Dictionary<float,Coord> scores) {
        if (scores.Count != 1)
        {
            List<float> scoresInit = new List<float>(scores.Keys);

            scoresInit.Sort();
            float min = scoresInit[0];
            float max = scoresInit[scoresInit.Count - 1];

            List<float> scoresConsidered = new List<float>();
            foreach (float i in scoresInit)
                if (i > min + 0.9 * (max - min))
                    scoresConsidered.Add(i);

            //print(scoresConsidered.Count + " scores considérés.");

            scoresConsidered.Sort(); //scoresConsidered.Reverse();
            int choice = scoresConsidered.Count - 1; bool select = true;
            while (select && choice - 1 >= 0)
            {
                if (Random.Range(0f, 1f) < 0.3f) // influer sur cette proba pour donner à l'IA un choix plus ou moins random
                    choice--;
                else
                    select = false;
            }

            //print("Score choisi : "+ (scoresConsidered.Count-choice));

            return scores[scoresConsidered[choice]];
        }
        else
            return scores[new List<float>(scores.Keys)[0]];
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
