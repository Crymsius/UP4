using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor (typeof (GridGenerator))]
public class GridEditor : Editor {

	public override void OnInspectorGUI ()
	{
		GridGenerator grid = target as GridGenerator;

        if (DrawDefaultInspector () && SceneManager.GetActiveScene().name == "GridCreator") {
            grid.GenerateEditor ();
		}

		if (GUILayout.Button ("Generate Grid")) {
			grid.GenerateGrid ();
            grid.DisplayFromSave ();
		}

		if (GUILayout.Button ("Display Properties from Cells")) {
			grid.DisplayFromCells ();
		}

        if (GUILayout.Button ("Save Cells Modifications")) {
            grid.SaveCells ();
        }
	}

}