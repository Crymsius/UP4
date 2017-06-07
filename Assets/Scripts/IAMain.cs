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
        {
            if (cell.trigger.isTrigger)
                trigPositions.values[cell.coordinates] = cell.trigger.triggerType;
            if (cell.full)
                board.values[cell.coordinates] = -2;
        }
        
        mainNode = new DecisionTreeNode(1, 0, 4, board, trigPositions, myAtlas, new Coord(0,-1), 0, new Dictionary<Coord, DecisionTreeNode>(), typePlayers);
        mainNode.DeploymentTree();

        //PrintAllPlays();
    }

    public void GetCurrentPlay(Coord play) { // Récupère le play qui vient d'être joué pour actualiser l'arbre de décision.
        //print(play.Stringify());
        if (mainNode.children.ContainsKey(play))
            mainNode = mainNode.children[play]; // Le reste est envoyé au GarbageCollector, normalement...
        else
            print("JE CRASHE !"); // Pour une raison obscure, un coup joué n'a pas été prévu par la grille... Enquêter sur le pourquoi...
        mainNode.Maj_Depth(0);
        mainNode.DeploymentTree();

        //PrintAllPlays();
    }

    public void IA_Play() { // compare les scores de chaque coup et joue le meilleur
        mainNode.MAJ_Scores();

        Coord bestplay = new Coord(-1, -1); float bestScore = -10000;
        foreach (Coord play in mainNode.children.Keys)
            if (mainNode.children[play].score >= bestScore)
            {
                bestplay = play;
                bestScore = mainNode.children[play].score;
            }
        
        StartCoroutine(GameObject.Find("GeneralHandler").GetComponent<GameHandler>().PutAPawn(myAtlas.gridDictionary[bestplay]));
    }

    public void PrintAllPlays() { // fonction d'observation
        string s = "";
        foreach (Coord key in mainNode.children.Keys)
            s += key.Stringify() + " : " + mainNode.children[key].score + " // ";
        print(s);

        //mainNode.position.Stringify();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
