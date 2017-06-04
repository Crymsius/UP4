using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMain : MonoBehaviour {

    public Atlas myAtlas;

    public DecisionTreeNode mainNode;
    //public Matrix currentPosition;
    public int myNumber;

    // Use this for initialization
    void Start () {

	}

    public void settingGrid(Coord c)
    {
        mainNode = new DecisionTreeNode(myNumber, 0, 4, new Matrix(c), 0, new Dictionary<Coord, DecisionTreeNode>());
        mainNode.DeploymentTree();
    }

    public void GetCurrentPlay(Coord play) { // Récupère le play qui vient d'être joué pour actualiser l'arbre de décision.
        if (mainNode.children.ContainsKey(play))
            mainNode = mainNode.children[play]; // Le reste est envoyé au GarbageCollector, normalement...
        else
            print("JE CONCEDE !"); // Pour une raison obscure, un coup joué n'a pas été prévu par la grille... Enquêter sur le pourquoi...
        mainNode.Maj_Depth(0);
        mainNode.DeploymentTree();
    }

    public void IA_Play() { // compare les scores de chaque coup et joue le meilleur
        mainNode.MAJ_Scores();

        Coord bestplay = new Coord(); float bestScore = -1000;
        foreach (Coord play in mainNode.children.Keys)
            if (mainNode.children[play].score >= bestScore)
            {
                bestplay = play;
                bestScore = mainNode.children[play].score;
            }

        // Juste quelques infos sur les scores. Uncomment pour observer.
        /*string s = "";
        foreach (Coord key in mainNode.children.Keys)
            s += key.Stringify() + " : " + mainNode.children[key].score + " // ";
        print(s);

        print(mainNode.HowManyNodes() + "");*/

        StartCoroutine(GameObject.Find("GeneralHandler").GetComponent<GameHandler>().PutAPawn(myAtlas.gridDictionary[bestplay]));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
