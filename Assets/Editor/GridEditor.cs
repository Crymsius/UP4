using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (GridGenerator))]
public class GridEditor : Editor {

	public override void OnInspectorGUI ()
	{
		GridGenerator grid = target as GridGenerator;

		if (DrawDefaultInspector ()) {
			grid.GenerateGrid ();
		}

		if (GUILayout.Button ("Generate Grid")) {
			grid.GenerateGrid ();
		}
	}

}