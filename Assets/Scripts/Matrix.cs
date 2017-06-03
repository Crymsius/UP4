﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Matrix
{
    public int hDim;
    public int vDim;
    public Dictionary<Coord, int> values;

    public Matrix(Coord _c)
    {
        hDim = _c.x;
        vDim = _c.y;
        values = new Dictionary<Coord, int>();
        for (int i = 0; i < hDim; i++)
            for (int j = 0; j < vDim; j++)
                values.Add(new Coord(i, j), -1);
    }

    public Matrix(Matrix copy) {
        hDim = copy.hDim;
        vDim = copy.vDim;
        values = new Dictionary<Coord, int>();

        foreach (Coord c in copy.values.Keys)
            values.Add(c, copy.values[c]);
    }

    public int MeasureScore(int player) {
        int hscore =0, vscore=0, d1score=0, d2score=0;

        for (int i = 0; i < hDim; i++)
            hscore += intermediateCalculus(new Coord(i, 0), new Coord(0, 1), player);

        for (int i = 0; i < vDim; i++)
            vscore += intermediateCalculus(new Coord(0, i), new Coord(1, 0), player);

        for (int i = 3; i <= hDim + vDim - 5; i++)
            d1score += intermediateCalculus(new Coord(Mathf.Min(i, hDim - 1), Mathf.Max(0, i + 1 - hDim)), new Coord(-1, 1), player);

        for (int i = 3; i <= hDim + vDim - 5; i++)
            d2score += intermediateCalculus(new Coord(Mathf.Max(0, i + 1 - vDim), Mathf.Max(0, vDim - i - 1)), new Coord(1, 1), player);

        return hscore + vscore + d1score + d2score;
    }

    public int intermediateCalculus(Coord init, Coord direction, int player) {
        int result =0,vinter = 0;
        Coord actuel = init;

        while (values.ContainsKey(actuel))
        {
            if (values[actuel] == player)
            {
                vinter++;
                if (vinter >= 4)
                {
                    result += 100000;
                    vinter = 0;
                }
            }
            else
            {
                result += (int)Mathf.Pow(vinter, 3);
                vinter = 0;
            }
            actuel = actuel + direction;
        }
        result += (int)Mathf.Pow(vinter, 2);

        return result;
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