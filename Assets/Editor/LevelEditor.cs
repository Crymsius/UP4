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
    public bool[,] gridLayoutVertical = new bool[0,0];
    public bool[,] gridLayoutHorizontal = new bool[0,0];
    public bool[,] gridLayoutIntersection = new bool[0,0];

    public string gridString;


    [MenuItem ("My Tools/ Level Editor %g")]
    private static void showEditor ()
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
            gridContent = new string[gridHeight,gridWidth];
            gridLayoutVertical = new bool[gridHeight,gridWidth-1];
            gridLayoutHorizontal = new bool[gridHeight-1,gridWidth];
            gridLayoutIntersection = new bool[gridHeight-1,gridWidth-1];
            gridHeightPrivate = gridHeight;
            gridWidthPrivate = gridWidth;
        }

        GUILayout.Label ("Grid customization", EditorStyles.boldLabel);
        for (int i = 0; i < gridHeightPrivate; i++) 
		{
			/* DEBUT D'UNE LIGNE DE CASES */ 

			EditorGUILayout.BeginHorizontal (GUILayout.MaxWidth(gridWidthPrivate));

			for (int j = 0; j < gridWidthPrivate; j++) 
			{
                gridContent [i, j] = EditorGUILayout.TextField (gridContent[i, j], GUILayout.Width(40),GUILayout.Height(40));
                
				if (j != gridWidthPrivate - 1) 
				{
					EditorGUILayout.BeginHorizontal (GUILayout.Width(25));
					EditorGUILayout.BeginVertical (GUILayout.MaxHeight(40));
					GUILayout.FlexibleSpace ();

                    gridLayoutVertical [i, j] = EditorGUILayout.Toggle (gridLayoutVertical [i, j]);

					GUILayout.FlexibleSpace ();
					EditorGUILayout.EndVertical ();
					EditorGUILayout.EndHorizontal ();
                }
            }
            EditorGUILayout.EndHorizontal ();
			/* FIN D'UNE LIGNE DE CASES */ 

			/* DEBUT D'UNE LIGNE DE BARRIERES */
			EditorGUILayout.BeginHorizontal (GUILayout.MaxWidth(gridWidthPrivate ));
            for (int j = 0; j < gridWidthPrivate; j++)
			{
                if (i != gridHeightPrivate -1)
				{
					GUILayout.Space (17);
					gridLayoutHorizontal [i, j] = EditorGUILayout.Toggle (gridLayoutHorizontal [i, j]);
					GUILayout.Space (17);

					if (j != gridWidthPrivate - 1)
					{
						GUILayout.Space (5);
						gridLayoutIntersection [i, j] = EditorGUILayout.Toggle (gridLayoutIntersection [i, j]);
						GUILayout.Space (6);
					}
                }
            }
            EditorGUILayout.EndHorizontal ();
        }

        if (GUILayout.Button("Generate Grid")) 
		{
            gridString = this.generateGrid ();
        }
    }


    public string generateGrid () {
		//l : horizontal
		//k : vertical
        //String vide au départ, va être populé par le reste afin de créer le résultat à rendre
        string resultString = "";

		// en-tête
		resultString = string.Concat(resultString, "N");
		for (int l = 0; l < gridWidth; l++) {
			resultString = string.Concat(resultString, "-N");
		}
		resultString = string.Concat(resultString, "-N+");

		//pour chaque ligne
        for (int k = 0; k < gridHeight; k++) {
			resultString = string.Concat(resultString, "N");
          
			//chaque case (= colone)
            for (int l = 0; l < gridWidth; l++) {
                //ajoute le contenu de chaque case, dans l'ordre
                resultString = string.Concat (resultString, "-");

                resultString = string.Concat (resultString, stringTranslator (gridContent[k,l]));
				if (l != gridWidth -1 && gridLayoutVertical[k,l]) {
					resultString = string.Concat (resultString, "D");
                }
				if (k != gridHeight -1 && gridLayoutHorizontal[k,l]) {
                    resultString = string.Concat (resultString, "B");
				}
				if (l != gridWidth -1 && k != gridHeight -1 && gridLayoutIntersection[k,l]) {
					resultString = string.Concat (resultString, "I");
				}
            }
			resultString = string.Concat(resultString, "-N+");

        }
		// fermeture
		resultString = string.Concat(resultString, "N");
		for (int l = 0; l < gridWidth; l++) {
			resultString = string.Concat(resultString, "-N");
		}
		resultString = string.Concat(resultString, "-N");

        return resultString;
    }
		
	/**
	 * stringTranslator(stringcontent)
	 * Renvoi une lettre en fonction du contenu
	**/
    public string stringTranslator(string content) {
        switch (content) {
        case "N":
            return "N";
        case "switch":
            return "S";
        default:
            return "V";
            break;
        }
    }
}