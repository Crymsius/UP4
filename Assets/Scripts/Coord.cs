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

}