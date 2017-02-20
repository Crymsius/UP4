using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelEditor : EditorWindow {

    //TODO link avec des gameobjects, link les assets et tout ce qui va bien

    public int gridWidth = 2;
    public int gridHeight = 2;
    private int gridWidthPrivate;
    private int gridHeightPrivate;
    public string[,] gridContent = new string[0,0];
    public string[,] gridLayoutVertical = new string[0,0];
    public string[,] gridLayoutHorizontal = new string[0,0];


    [MenuItem ("My Tools/ Level Editor")]
    private static void showEditor()
    {
        EditorWindow.GetWindow<LevelEditor> (false, "Grid Editor");
    }

    void OnGUI () {

        // The actual window code goes here
        GUILayout.Label ("Grid Size", EditorStyles.boldLabel);
        //TODO mettre des checks sur les valeurs possibles de width/height (ex: pas de negatifs)
        gridWidth = EditorGUILayout.IntField ("Grid Width", gridWidth);
        gridHeight = EditorGUILayout.IntField ("Grid Height", gridHeight);
        if (GUILayout.Button("Reload Grid")) {
            Debug.Log ("reload grid");
            gridContent = new string[gridHeight,gridWidth];
            gridLayoutVertical = new string[gridHeight,gridWidth];
            gridLayoutHorizontal = new string[gridHeight,gridWidth];
            gridHeightPrivate = gridHeight;
            gridWidthPrivate = gridWidth;
        }

        GUILayout.Label ("Grid customization", EditorStyles.boldLabel);
        for (int i = 0; i < gridHeightPrivate; i++) {
            EditorGUILayout.BeginHorizontal ();
            for (int j = 0; j < gridWidthPrivate; j++) {
                gridContent [i, j] = EditorGUILayout.TextField (gridContent[i, j], GUILayout.Width(40),GUILayout.Height(40));
                GUILayout.Space (5);
                gridLayoutVertical [i, j] = EditorGUILayout.TextField (gridLayoutVertical [i, j], GUILayout.Width (10), GUILayout.Height (40));
                GUILayout.Space (5);
            }
            EditorGUILayout.EndHorizontal ();
            EditorGUILayout.BeginHorizontal ();
            for (int j = 0; j < gridWidthPrivate; j++) {
                gridLayoutHorizontal [i, j] = EditorGUILayout.TextField (gridLayoutHorizontal [i, j], GUILayout.Width (40), GUILayout.Height (10));
                GUILayout.Space (24);
            }
            EditorGUILayout.EndHorizontal ();
        }
        if (GUILayout.Button("Generate Grid")) {
            this.generateGrid ();
            //            Debug.Log (this.generateGrid());

        }

       

//        groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
//        myBool = EditorGUILayout.Toggle ("Toggle", myBool);
//        myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
//        EditorGUILayout.EndToggleGroup ();
    }

    public string generateGrid() {
        string gridString = "truc";
        for (int k = 0; k < gridHeight; k++) {
            for (int l = 0; l < gridWidth; l++) {
                string.Concat(gridString, gridContent[k,l]);
            }
        }
        Debug.Log (string.Concat(gridContent));
        return gridString;
    }

}