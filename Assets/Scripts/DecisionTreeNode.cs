using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTreeNode {

    public int player { get; set; }
    public int depth { get; set; }
    public int depthMax { get; set; }
    public Matrix position { get; set; }

    public float score { get; set; }
    public bool considered { get; set; }

    public Dictionary<Coord, DecisionTreeNode> children;

    public DecisionTreeNode(int _player, int _depth, int _depthMax, Matrix _position, float _score, Dictionary<Coord, DecisionTreeNode> _children)
    {
        player = _player;
        depth = _depth;
        depthMax = _depthMax;
        position = _position;
        score = _score;
        children = _children;

        considered = true;
    }

    public void DeploymentTree() { // Fonction récursive mettant à jour l'arbre de décision.
        if (depth == depthMax)
            /*do nothing*/
            ;

        else if (children.Count == 0)
        {
            List<Coord> playables = new List<Coord>();
            for (int i = 0; i < position.hDim; i++)
                for (int j = 0; j < position.vDim; j++)
                    if (position.values[new Coord(i, j)] == -1 && (!position.values.ContainsKey(new Coord(i, j - 1)) || position.values[new Coord(i, j - 1)] != -1))
                        playables.Add(new Coord(i, j));

            bool existsVictory = false;

            foreach (Coord play in playables)
            { // Création des nouveaux noeuds fils
                children.Add(play, new DecisionTreeNode(1 - player, depth + 1, depthMax, new Matrix(position), 0, new Dictionary<Coord, DecisionTreeNode>()));
                children[play].position.values[play] = 1 - player;

                children[play].position.MeasureScore(children[play].player);
                if (children[play].position.isVictory)
                    existsVictory = true;

                //children[play].DeploymentTree();
            }

            foreach (Coord play in playables) {
                if ((existsVictory && children[play].position.isVictory) || (!existsVictory))
                    children[play].DeploymentTree();
                else
                     children[play].considered = false;
            }
        }

        else foreach (Coord key in children.Keys) if (children[key].considered)
                    children[key].DeploymentTree();
}

    public void MAJ_Scores() // Fonction récursive mettant à jour les scores de chaque coup
    {
        if (depth == depthMax)
            score = (float)position.MeasureScore(player);

        else
        {
            int numberConsidered = 0;
            float result = 0;
            foreach (Coord key in children.Keys)
                if (children[key].considered)
                {
                    children[key].MAJ_Scores();
                    result += children[key].score;
                    numberConsidered++;
                }
            score = (float)position.MeasureScore(player) - 1f / 3f * result / Mathf.Max(1, numberConsidered);
        }
    }

    public void Maj_Depth(int d) // Fonction récursive mettant à jour la profondeur de chaque noeud
    {
        depth = d;

        foreach (Coord c in children.Keys)
            children[c].Maj_Depth(d + 1);
    }

    public int HowManyNodes() { // Fonction récursive de comptage permettant de savoir combien de noeuds sont actuellement considérés
        if (children.Count == 0)
            return 1;

        else
        {
            int result = 1;
            foreach (Coord key in children.Keys)
                result += children[key].HowManyNodes();
            return result;
        }
    }
}
