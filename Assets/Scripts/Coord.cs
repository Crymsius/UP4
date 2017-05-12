using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Coord {
    public int x;
    public int y;

    public Coord(int _x, int _y) {
        x = _x;
        y = _y;
    }

    /// <summary>
    /// Operator + for coordinates
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns> (xa + xb, ya + yb) </returns>
    public static Coord operator +(Coord a, Coord b){
        return new Coord (a.x + b.x, a.y + b.y);
    }

    /// <summary>
    /// Operator - for coordinates
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns> (xa - xb, ya - yb) </returns>
    public static Coord operator -(Coord a, Coord b){
        return new Coord (a.x - b.x, a.y - b.y);
    }

    /// <summary>
    /// Operator * for an int with coordinates
    /// </summary>
    /// <param name="a"> int </param>
    /// <param name="b"> Coord </param>
    /// <returns> a * (xb, yb) </returns>
    public static Coord operator *(int a, Coord b){
        return new Coord (a* b.x, a * b.y);
    }

    /// <summary>
    /// Operator == for an int with coordinates
    /// </summary>
    /// <param name="a"> Coord </param>
    /// <param name="b"> Coord </param>
    /// <returns> true if a==b, else false </returns>
    public static bool operator ==(Coord a, Coord b){
        return a.x == b.x && a.y == b.y ? true: false;
    }

    /// <summary>
    /// Operator != for an int with coordinates
    /// </summary>
    /// <param name="a"> Coord </param>
    /// <param name="b"> Coord </param>
    /// <returns> true if a!=b, else false </returns>
    public static bool operator !=(Coord a, Coord b){
        return a.x != b.x || a.y != b.y ? true: false;
    }

    /// <summary>
    /// Operator Object.Equals ()
    /// </summary>
    /// <param name="o">Coord to compare</param>
    /// <returns>true or false</returns>
    public override bool Equals (object o) {
        try {
            return (bool) (this == (Coord) o);
        } catch {
            return false;
        }
    }

    /// <summary>
    /// GetHashCode method
    /// </summary>
    /// <returns>a unique key for a 2D vector</returns>
    public override int GetHashCode () {
        int tmp = ( y +  ((x+1)/2));
        return x +  ( tmp * tmp);;
    }

    /// <summary>
    /// Method Stringify returns the string (x;y)
    /// </summary>
    /// <returns> "(x;y)" </returns>
    public string Stringify () {
        return "(" + x.ToString () + ";" + y.ToString () + ")";
    }
}