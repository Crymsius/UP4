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
        mainNode.Maj_DeploymentTree();
    }

    public void GetCurrentPlay(Coord play) {
        mainNode = mainNode.children[play]; // vérifier si le reste est bien envoyé au GarbageCollector, sinon on va bouffer toute la mémoire.
        mainNode.Maj_Depth(0);
        mainNode.Maj_DeploymentTree();
    }

    public void IA_Play() {
        mainNode.MAJ_Scores();

        Coord bestplay = new Coord(); float bestScore = -1000;
        foreach (Coord play in mainNode.children.Keys)
            if (mainNode.children[play].score >= bestScore)
            {
                bestplay = play;
                bestScore = mainNode.children[play].score;
            }

        StartCoroutine(GameObject.Find("GeneralHandler").GetComponent<GameHandler>().PutAPawn(myAtlas.gridDictionary[bestplay]));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
