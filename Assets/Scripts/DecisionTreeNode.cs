using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTreeNode {

    public int player { get; set; }
    public int depth { get; set; }
    public int depthMax { get; set; }
    public Matrix position { get; set; }

    public float score { get; set; }

    public Dictionary<Coord, DecisionTreeNode> children;

    public DecisionTreeNode(int _player, int _depth, int _depthMax, Matrix _position, float _score, Dictionary<Coord, DecisionTreeNode> _children)
    {
        player = _player;
        depth = _depth;
        depthMax = _depthMax;
        position = _position;
        score = _score;
        children = _children;
    }

    public void Maj_DeploymentTree()
    { // détecte tous les coups possibles jusqu'à une certaine profondeur
        if (depth < depthMax && children.Count == 0)
            GrowTree();
        else if (depth < depthMax) foreach (Coord key in children.Keys)
                children[key].Maj_DeploymentTree();
    }
    public void GrowTree()
    { 
        List<Coord> playables = new List<Coord>();
        for (int i = 0; i < position.hDim; i++)
            for (int j = 0; j < position.vDim; j++)
                if (position.values[new Coord(i,j)]==-1 && (!position.values.ContainsKey(new Coord(i, j - 1)) || position.values[new Coord(i, j - 1)] != -1))
                    playables.Add(new Coord(i, j));

        foreach (Coord play in playables) {
            children.Add(play, new DecisionTreeNode(1-player,depth+1,depthMax,new Matrix(position),0,new Dictionary<Coord, DecisionTreeNode>()));
            children[play].position.values[play] = 1 - player;

            if (children[play].depth < depthMax)
                children[play].GrowTree();
        }
    }

    public void MAJ_Scores()
    {
        if (depth == depthMax)
            score = (float)position.MeasureScore(player);

        else
        {
            float result = 0;
            foreach (Coord key in children.Keys)
            {
                children[key].MAJ_Scores();
                result += children[key].score;
            }
            score = (float)position.MeasureScore(player) - 1f / 3f * result / children.Count;
        }
    }

    public void Maj_Depth(int d)
    {
        depth = d;

        foreach (Coord c in children.Keys)
            children[c].Maj_Depth(d + 1);
    }
}
