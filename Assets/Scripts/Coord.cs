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

	// Operator + for coordinates
	public static Coord operator+(Coord a, Coord b){
		return new Coord (a.x + b.x, a.y + b.y);
	}

	// Operator - for coordinates
	public static Coord operator-(Coord a, Coord b){
		return new Coord (a.x - b.x, a.y - b.y);
	}

    // Operator * for an int with coordinates
    public static Coord operator*(int a, Coord b){
        return new Coord (a* b.x, a * b.y);
    }

	// Method Stringify returns the string (x;y) 
	public string Stringify () {
		return "(" + x.ToString () + ";" + y.ToString () + ")";
	}
}