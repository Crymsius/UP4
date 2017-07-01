#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;


    [ExecuteInEditMode]
public class GridCreatorManager : MonoBehaviour {
    GameObject gridGenerator;
    GameObject generatedGrid;

    void OnEnable () {
        gridGenerator.GetComponent<GridGenerator> ().LoadLevelData ();
        EditorSceneManager.sceneClosed += OnSceneClosed;
    }

    void OnDisable () {
        EditorSceneManager.sceneClosed -= OnSceneClosed;
    }

    void OnSceneClosed (Scene scene) {
        DestroyImmediate (generatedGrid);
    }

    void Awake () {
        gridGenerator = GameObject.Find ("GridHolder");
        generatedGrid = GameObject.Find ("Generated Grid(Clone)");
        // gridGenerator.GetComponent<GridGenerator> ().LoadLevelData ();
    }
}
    #endif
