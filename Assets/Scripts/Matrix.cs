using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix
{
    public int hDim { get; set; }
    public int vDim { get; set; }

    public Dictionary<Coord, int> values;
    public Atlas myAtlas { get; set; }
	public Coord forecastedTranslate { get; set;}

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

		forecastedTranslate = new Coord (0, 0);
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

		forecastedTranslate = copy.forecastedTranslate;
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

        return hscore + vscore + d1score + d2score - 2500 * inVictory[1 - player].Count; // Le dernier terme prend en compte le fait que l'adversaire peut obtenir une P4 en même temps
    }
    public int intermediateCalculus(Coord init, Coord direction, int player) { // Calcule le score d'un joueur sur une ligne directionnelle
        int result=0, pinter = 0, vinter = 0;
        Coord actuel = init;

		while (values.ContainsKey(actuel))
        {
			if (new List<int> (){ pinter, 3 }.Contains (values [actuel])) { // Si l'on tombe sur un pion du joueur OU un pion commun
				vinter++;
				if (vinter == 4) { // On se rend compte qu'il existe une P4
					isVictory = true;
					result += (player == pinter) ? 10000 : 0;
					coordWinningLines.Add (GetAtlasCell (actuel - 3 * direction).GetComponent<Transform> ().position);
					coordWinningLines.Add (GetAtlasCell (actuel).GetComponent<Transform> ().position);
					// On enregistre les cellules concernées pour le score aux points, on ne prend pas en compte les cellules communes
					for (int i = 0; i <= 3; i++)
						if (!inVictory [pinter].Contains (actuel - i * direction) && values [actuel - i * direction] != 3)
							inVictory [pinter].Add (actuel - i * direction);
				} 
				else if (vinter > 4)
					if (!inVictory [pinter].Contains (actuel) && values [actuel] != 3) { // On a une P>=4
						inVictory [pinter].Add (actuel);
						result += (player == pinter) ? 2500 : 0;

						coordWinningLines.RemoveAt (coordWinningLines.Count - 1);
						coordWinningLines.Add (GetAtlasCell (actuel).GetComponent<Transform> ().position);
					}
			} 
			else if (values [actuel] == 1 - pinter || willChange (actuel, direction, pinter)) // La 2nde condition gère les cas où le pion commun peut compter dans le jeu adverse
			{
				result += (player == pinter) ? (int)Mathf.Pow (vinter, 3) : 0;
				pinter = 1 - pinter;
				vinter = 1;
			}
            else 
			{
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
	public bool willChange(Coord init, Coord direction, int currentPlayer){ // Dans le cas précis, sur un pion commun, de voir si on a besoin de commencer à compter pour le joueur adverse
		bool result = false;
		Coord actuel = init;

		while (values[actuel]==3 && values.ContainsKey(actuel+direction)) {
			result = result || values [actuel + direction] == 1 - currentPlayer;
			actuel = actuel + direction;
		}

		return result;
	}
    public bool isBlocked(Coord actuel, Coord direction) {
        bool result = false;

		Coord adjustedCoord = adjustCoord (actuel);

        if (direction == new Coord(0, 1)) // DEBUT DU DAAAAAAANGER
			result = result || (myAtlas.gridDictionary.ContainsKey(adjustedCoord + direction) && myAtlas.gridDictionary[adjustedCoord + direction].walls.wally);
        else if (direction == new Coord(1, 0))
			result = result || GetAtlasCell(actuel).walls.wallx;
        else if (direction == new Coord(1, 1))
			result = result || (myAtlas.gridDictionary.ContainsKey(adjustedCoord + new Coord(0,1)) && myAtlas.gridDictionary[adjustedCoord + new Coord(0, 1)].walls.wallxy);
        else if (direction == new Coord(-1, 1))
			result = result || (myAtlas.gridDictionary.ContainsKey(adjustedCoord + direction) && myAtlas.gridDictionary[adjustedCoord + direction].walls.wallxy);
		
        return result;
    }

    public List<int> CheckTriggers(Coord play, Coord gravity)
    {
        List<int> results = new List<int>();

        Coord startCell = play;
		while (myAtlas.gridDictionary.ContainsKey(adjustCoord(startCell) - gravity))
            startCell = startCell - gravity;

        while (startCell != play + gravity)
        {
			if (GetAtlasCell(startCell).trigger.isTrigger)
				results.Add(GetAtlasCell(startCell).trigger.triggerType);
            startCell = startCell + gravity;
        }
        return results;
    }
	public Coord CheckNextFloorOrTrigger(Coord play, Coord gravity){
		Coord startCell = play;
		bool next = true;
		while (!isBlocked(startCell,gravity) && values.ContainsKey (startCell + gravity) && values[startCell + gravity]==-1 && !myAtlas.gridDictionary[adjustCoord(startCell) + gravity].full && next) {
			startCell = startCell + gravity;
			next = next && !GetAtlasCell(startCell).trigger.isTrigger;
		}
		return startCell;
	}

	public Coord GetHighestPlayPosition(Coord play, Coord gravity){
		Coord startCell = play;

		while (!isBlocked(startCell,new Coord(-gravity.x, -gravity.y)) && values.ContainsKey (startCell - gravity) && values[startCell - gravity]==-1 && 
			!myAtlas.gridDictionary[adjustCoord(startCell) - gravity].full && !myAtlas.gridDictionary [adjustCoord(startCell) - gravity].trigger.isTrigger)
			startCell = startCell - gravity;
		
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
            goalCell = new Coord(startcell.x, startcell.y); actualCell = startcell - gravity;
            while (values.ContainsKey(actualCell))
            {
				if (values [goalCell] != -1)
					goalCell = goalCell - gravity;
				else if (isBlocked (actualCell + gravity, new Coord (-gravity.x, -gravity.y)))
					goalCell = new Coord (actualCell.x, actualCell.y);
				else if (values [actualCell] == -2) {
					goalCell = actualCell - gravity;
					actualCell = actualCell - gravity;
				}
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

	public void Translate(Coord direction){
		Coord myGridSize = GameObject.Find("Generated Grid(Clone)").GetComponent<Grid>().gridSize;
		//Translation de la grille virtuelle, l'Atlas est déjà modifié
		Coord iterTranslate, iterNormal, startCoord, startNewCoord;

		iterTranslate = direction;
		iterNormal = new Coord (direction.y, direction.x);

		bool isNegative = direction.x < 0 || direction.y < 0;
		bool isX = direction.x != 0;

		startCoord = isNegative ? new Coord (0, 0) : new Coord (myGridSize.x - 1, myGridSize.y - 1);
		startNewCoord = isNegative ? new Coord (myGridSize.x - 1, myGridSize.y - 1) : new Coord (0, 0);

		Dictionary<Coord, int> temporary = new Dictionary<Coord, int> ();
		for (int i = 0; i < (isX ? myGridSize.y : myGridSize.x); i++) 
			temporary.Add (startCoord - i * iterNormal, values [startCoord - i * iterNormal]);

		for (int n = 0; n < (isX ? myGridSize.x - 1 : myGridSize.y - 1); n++)
			for (int m = 0; m < (isX ? myGridSize.y : myGridSize.x); m++) 
				values [startCoord - n * iterTranslate - m * iterNormal] = values [startCoord - (n + 1) * iterTranslate - m * iterNormal];

		for (int i = 0; i < (isX ? myGridSize.y : myGridSize.x); i++) 
			values [startNewCoord + i * iterNormal] = temporary [startCoord - ((isX ? myGridSize.y - 1 : myGridSize.x - 1) - i) * iterNormal];

		forecastedTranslate = forecastedTranslate + direction;
	}

	public Cell GetAtlasCell(Coord position){
		return myAtlas.gridDictionary [adjustCoord (position)];
	}
	public Coord adjustCoord(Coord aCoord){ // Provoque boucle infinie OU explosion du temps de calcul
		if (forecastedTranslate != new Coord (0, 0)) {
			Coord truePosition = aCoord - forecastedTranslate;

			int adjustedX = truePosition.x % hDim;
			int adjustedY = truePosition.y % vDim;

			adjustedX += (adjustedX < 0 ? hDim : 0);
			adjustedY += (adjustedY < 0 ? vDim : 0);

			return new Coord (adjustedX, adjustedY);
		} else
			return aCoord;
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