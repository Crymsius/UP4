using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor (typeof (GridLoader))]
public class GridLoaderEditor : Editor {

    public override void OnInspectorGUI () {
        GridLoader grid = target as GridLoader;

        //la fonction qui suit est uniquement là pour avoir la surcharge de l'editor
        if (DrawDefaultInspector ()) {
        }

        if (GUILayout.Button ("Load Grid Data from the json")) {
            grid.LoadLevelData ();
        }
    }
}