using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

[CustomEditor (typeof (LevelManager))]
public class ListLevelEditor : Editor {
    //GameObject[] goArray = SceneManager.GetSceneByName ("GridScene").GetRootGameObjects ();
    public override void OnInspectorGUI () {
        if (GUILayout.Button ("UpdateList")) {
            GameObject[] goArray = SceneManager.GetSceneByName ("GridScreen").GetRootGameObjects ();
            GameObject rootGo = goArray[7];
          //  int grids = rootGo.GetComponent<GridGenerator> ().grids.Length;
        }
    }
}
