using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor (typeof (GridGenerator))]
public class GridEditor : Editor {

    public override void OnInspectorGUI () {
        GridGenerator grid = target as GridGenerator;

        if (DrawDefaultInspector () && SceneManager.GetActiveScene().name == "GridCreator") {
            grid.GenerateEditor ();
        }

        if (GUILayout.Button ("Json -> Grid")) {
            grid.LoadLevelData ();
        }

        if (GUILayout.Button ("Grid -> Json")) {
            grid.GenerateJson ();
        }
        
        // if (GUILayout.Button ("Save Cells Modifications (old, updates should be automatic)")) {
        //     grid.SaveCells ();
        // }

        // if (GUILayout.Button ("Generate Grid (old, updates should be automatic)")) {
        //     grid.GenerateGridButton ();
        // }
    }
}