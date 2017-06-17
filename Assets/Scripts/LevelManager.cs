using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LevelManager : MonoBehaviour {

    [System.Serializable]
    public class LevelButtonInfo {
        public string levelText;
        public int levelId;
    }

    public GameObject levelButton;
    public Transform Spacer;
    public List<LevelButtonInfo> listLevel;
    string json;
    private LevelHolder levels;
    private string levelDataFileName = "levelData.json";

    void Start () {
        FillList ();
    }

    /// <summary>
    /// Fill the spacer of buttons for all the levels in the list
    /// </summary>
    public void FillList () {
        foreach (var level in listLevel) {
            int i = 0;
            GameObject newButton = Instantiate (levelButton) as GameObject;
            newButton.transform.SetParent (Spacer,false);
            //set the clickevent for the correct level.
            newButton.GetComponent<Button> ().onClick.AddListener (() => GameObject.Find ("LevelSelectionScreenController").GetComponent<LevelSelectionScreenController> ().StartQuickGame (level.levelId));
            LevelButton button = newButton.GetComponent<LevelButton> ();
            //set the text for the correct number.
            button.levelText.text = level.levelText;
            button.levelIndex = i;
            i++;
        }
    }

    /// <summary>
    /// Load data from json file
    /// </summary>
    public void LoadLevelData () {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine (Application.streamingAssetsPath, levelDataFileName);

        if (File.Exists (filePath)) {
            // Read the json from the file into a string
            json = File.ReadAllText (filePath);
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            levels = JsonUtility.FromJson<LevelHolder> (json);
            GenerateListLevel ();
        } else {
            Debug.LogError ("Cannot load game data!");
        }
    }

    /// <summary>
    /// Generate the list of level from the grid list
    /// </summary>
    public void GenerateListLevel () {
        listLevel.Clear ();
        for (int j = 0 ; j < levels.grids.Count ; j++) {
            listLevel.Add (new LevelButtonInfo { levelText = (j+1).ToString (), levelId = j});
        }
    }
}