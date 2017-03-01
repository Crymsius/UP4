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

    public string gridString;


    [MenuItem ("My Tools/ Level Editor %g")]
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
        gridString = EditorGUILayout.TextField ("Grid String", gridString);
        if (GUILayout.Button("Reload Grid")) {
//            Debug.Log ("reload grid");
            gridContent = new string[gridHeight,gridWidth];
            gridLayoutVertical = new string[gridHeight,gridWidth-1];
            gridLayoutHorizontal = new string[gridHeight-1,gridWidth];
            gridHeightPrivate = gridHeight;
            gridWidthPrivate = gridWidth;
        }

        GUILayout.Label ("Grid customization", EditorStyles.boldLabel);
        for (int i = 0; i < gridHeightPrivate; i++) {
            EditorGUILayout.BeginHorizontal ();
            for (int j = 0; j < gridWidthPrivate; j++) {
                gridContent [i, j] = EditorGUILayout.TextField (gridContent[i, j], GUILayout.Width(40),GUILayout.Height(40));
                GUILayout.Space (5);
                if (j != gridWidthPrivate - 1) {
                    gridLayoutVertical [i, j] = EditorGUILayout.TextField (gridLayoutVertical [i, j], GUILayout.Width (10), GUILayout.Height (40));
                }
                GUILayout.Space (5);
            }
            EditorGUILayout.EndHorizontal ();
            EditorGUILayout.BeginHorizontal ();
            for (int j = 0; j < gridWidthPrivate; j++) {
                if (i != gridHeightPrivate -1) {
                    gridLayoutHorizontal [i, j] = EditorGUILayout.TextField (gridLayoutHorizontal [i, j], GUILayout.Width (40), GUILayout.Height (10));
                }
                GUILayout.Space (24);
            }
            EditorGUILayout.EndHorizontal ();
        }
        if (GUILayout.Button("Generate Grid")) {
            gridString = this.generateGrid ();
            //            Debug.Log (this.generateGrid());
        }

//        groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
//        myBool = EditorGUILayout.Toggle ("Toggle", myBool);
//        myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
//        EditorGUILayout.EndToggleGroup ();
    }

    public string generateGrid() {
        //String vide au départ, va être populé par le reste afin de créer le résultat à rendre
        string resultString = "";
        for (int k = 0; k < gridHeight; k++) {
            if (k != 0 && k != gridHeight) {
                resultString = string.Concat(resultString, "+");
            }
            for (int l = 0; l < gridWidth; l++) {
                //ajoute le contenu de chaque case, dans l'ordre
                if (l != 0 && l != gridWidth) {
                    resultString = string.Concat (resultString, "-");
                }
                resultString = string.Concat(resultString, stringTranslator("gridContent",gridContent[k,l]));
                if (l != gridWidth -1 ) {
                    resultString = string.Concat(resultString, stringTranslator("gridLayoutVertical",gridLayoutVertical[k,l]));
                }
                if (k != gridHeight -1) {
                    resultString = string.Concat(resultString, stringTranslator("gridLayoutHorizontal",gridLayoutHorizontal[k,l]));
                }
//                Debug.Log (gridContent[k,l]);
//                Debug.Log (resultString);
            }
        }
//        Debug.Log (string.Concat(gridContent));
        return resultString;
    }

    public string stringTranslator(string caseType, string content) {
        switch (caseType) {
        case "gridContent":
            switch (content) {
            case "N":
                return "N";
            case "switch":
                return "S";
            default:
                return "V";
                break;
            }
            break;
        case "gridLayoutHorizontal":
            if (content!= null) {
                return "B";
            } else {
                return "";
            }
            break;
        case "gridLayoutVertical":
            if (content!= null) {
                return "D";
            } else {
                return "";
            }
        default:
            Debug.Log ("error stringTranslator: caseType non valide");
            return "error";
            break;
        }
    }
}