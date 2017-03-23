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
}