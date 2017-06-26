using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix
{
    public int hDim { get; set; }
    public int vDim { get; set; }

    public Dictionary<Coord, int> values;
    public Atlas myAtlas { get; set; }

    public bool isVictory { get; set; }
    public Dictionary<int,List<Coord>> inVictory = new Dictionary<int, List<Coord>>();
    public List<Vector3> coordWinningLines = new List<Vector3>();

    public Matrix(Coord _c, Atlas _myatlas) // Création d'une grille vierge
    {
        hDim = _c.x;
        vDim = _c.y;
        values = new Dictionary<Coord, int>();
        for (int i = 0; i < hDim; i++)
            for (int j = 0; j < vDim; j++)
                values.Add(new Coord(i, j), -1);
        myAtlas = _myatlas;

        isVictory = false;
        inVictory.Add(0, new List<Coord>());
        inVictory.Add(1, new List<Coord>());
    }

    public Matrix(Matrix copy, Atlas _myatlas) { // Copie d'une autre matrice
        hDim = copy.hDim;
        vDim = copy.vDim;
        values = new Dictionary<Coord, int>();

        foreach (Coord c in copy.values.Keys)
            values.Add(c, copy.values[c]);
        myAtlas = _myatlas;

        isVictory = false;
        inVictory.Add(0, new List<Coord>());
        inVictory.Add(1, new List<Coord>());
    }

    public int MeasureScore(int player) { // Calcule le score instantané d'un joueur sur la grille
        int hscore =0, vscore=0, d1score=0, d2score=0;

        for (int i = 0; i < hDim; i++)
            hscore += intermediateCalculus(new Coord(i, 0), new Coord(0, 1), player);

        for (int i = 0; i < vDim; i++)
            vscore += intermediateCalculus(new Coord(0, i), new Coord(1, 0), player);

        for (int i = 3; i <= hDim + vDim - 5; i++)
            d1score += intermediateCalculus(new Coord(Mathf.Min(i, hDim - 1), Mathf.Max(0, i + 1 - hDim)), new Coord(-1, 1), player);

        for (int i = 3; i <= hDim + vDim - 5; i++)
            d2score += intermediateCalculus(new Coord(Mathf.Max(0, i + 1 - vDim), Mathf.Max(0, vDim - i - 1)), new Coord(1, 1), player);

        return hscore + vscore + d1score + d2score - 2500 * inVictory[1 - player].Count; // Le dernier terme prend en co^mpte le fait que l'adversaire peut obtenir une P4 en même temps
    }
    public int intermediateCalculus(Coord init, Coord direction, int player) { // Calcule le score d'un joueur sur une ligne directionnelle
        int result=0, pinter = 0, vinter = 0;
        Coord actuel = init;

        while (values.ContainsKey(actuel))
        {
            if (values[actuel] == pinter)
            {
                vinter++;
                if (vinter == 4) // On se rend compte qu'il existe une P4
                {
                    isVictory = true;
                    result += (player == pinter) ? 10000 : 0; // Marche bien avec 100 000
                    //vinter = 0;
                    coordWinningLines.Add(myAtlas.gridDictionary[actuel - 3 * direction].GetComponent<Transform>().position);
                    coordWinningLines.Add(myAtlas.gridDictionary[actuel].GetComponent<Transform>().position);
                    for (int i = 0; i <= 3; i++) if (!inVictory[pinter].Contains(actuel - i * direction)) // On enregistre les cellules concernées pour le score au points
                            inVictory[pinter].Add(actuel - i * direction);
                }
                else if (vinter > 4) if (!inVictory[pinter].Contains(actuel)) // On a plus qu'une P4
                    {
                        inVictory[pinter].Add(actuel);
                        result += (player == pinter) ? 2500 : 0;

                        coordWinningLines.RemoveAt(coordWinningLines.Count - 1);
                        coordWinningLines.Add(myAtlas.gridDictionary[actuel].GetComponent<Transform>().position);
                    }
            }
            else if (values[actuel] == 1 - pinter)
            {
                result += (player == pinter) ? (int)Mathf.Pow(vinter, 3) : 0;
                pinter = 1 - pinter;
                vinter = 1;
            }
            else {
                result += (player == pinter) ? (int)Mathf.Pow(vinter, 3) : 0;
                vinter = 0;
            }
            if (isBlocked(actuel, direction))
                vinter = 0;
            actuel = actuel + direction;
        }
        result += (player == pinter) ? (int)Mathf.Pow(vinter, 3) : 0;
       
        return result;
    }
    public bool isBlocked(Coord actuel, Coord direction) {
        bool result = false;
        if (direction == new Coord(0, 1))
            result = result || (myAtlas.gridDictionary.ContainsKey(actuel + direction) && myAtlas.gridDictionary[actuel + direction].walls.wally);
        else if (direction == new Coord(1, 0))
            result = result || myAtlas.gridDictionary[actuel].walls.wallx;
        else if (direction == new Coord(1, 1))
            result = result || (myAtlas.gridDictionary.ContainsKey(actuel + new Coord(0,1)) && myAtlas.gridDictionary[actuel + new Coord(0, 1)].walls.wallxy);
        else if (direction == new Coord(-1, 1))
            result = result || (myAtlas.gridDictionary.ContainsKey(actuel + direction) && myAtlas.gridDictionary[actuel + direction].walls.wallxy);

        return result;
    }
    public List<int> CheckTriggers(Coord play, Coord gravity)
    {
        List<int> results = new List<int>();

        Coord startCell = play;
        while (myAtlas.gridDictionary.ContainsKey(startCell - gravity))
            startCell = startCell - gravity;

        while (startCell != play + gravity)
        {
            if (myAtlas.gridDictionary[startCell].trigger.isTrigger)
                results.Add(myAtlas.gridDictionary[startCell].trigger.triggerType);
            startCell = startCell + gravity;
        }
        return results;
    }
	public Coord CheckNextFloorOrTrigger(Coord play, Coord gravity){
		Coord startCell = play;
		bool next = true;
		while (!isBlocked(startCell,gravity) && values.ContainsKey (startCell + gravity) && values[startCell + gravity]==-1 && next) {
			startCell = startCell + gravity;
			next = next && !myAtlas.gridDictionary [startCell].trigger.isTrigger;
		}
		return startCell;
	}

    public void ResetGravity(Coord gravity)
    {
        Coord startcell, goalCell, actualCell;
        if (gravity == new Coord(0, -1))
            startcell = new Coord(0, 0);
        else if (gravity == new Coord(1, 0))
            startcell = new Coord(hDim - 1, 0);
        else if (gravity == new Coord(0, 1))
            startcell = new Coord(hDim - 1, vDim - 1);
        else
            startcell = new Coord(0, vDim - 1);

        while (values.ContainsKey(startcell)) {
            goalCell = new Coord(startcell.x, startcell.y); actualCell = new Coord(startcell.x - gravity.x, startcell.y - gravity.y);
            while (values.ContainsKey(actualCell))
            {
                if (values[goalCell] != -1)
                    goalCell = goalCell - gravity;
                else if (values[actualCell] != -1) {
                    values[goalCell] = values[actualCell];
                    values[actualCell] = -1;
                    goalCell = goalCell - gravity;
                }

                actualCell = actualCell - gravity;
            }
            startcell = startcell + new Coord(-gravity.y, gravity.x);
        }
    }
        
    public void Stringify()
    {
        string s = "";
        for (int j = vDim - 1; j >= 0; j--)
        {
            for (int i = 0; i <= hDim - 1; i++)
                s += values[new Coord(i, j)] + " ";
            Debug.Log(s); s = "";
        }
    }
}