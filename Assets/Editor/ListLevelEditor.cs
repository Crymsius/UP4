using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (LevelManager))]
public class ListLevelEditor : Editor {

    public override void OnInspectorGUI () {
        LevelManager level = target as LevelManager;
        if (DrawDefaultInspector ()) {
        }
        if (GUILayout.Button ("UpdateList")) {
            level.LoadLevelData ();
        }
    }
}