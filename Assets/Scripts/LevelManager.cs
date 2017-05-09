using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    [System.Serializable]
    public class Level {
        public string levelText;
        public Button.ButtonClickedEvent OnClickEvent;
    }

    public GameObject levelButton;
    public Transform Spacer;
    public List<Level> ListLevel;

    // Use this for initialization
    void Start () {
        FillList ();
    }

    public void FillList () {
        foreach (var level in ListLevel) {
            int i = 0;
            GameObject newButton = Instantiate (levelButton) as GameObject;
            newButton.transform.SetParent (Spacer,false);
            newButton.GetComponent<Button> ().onClick = level.OnClickEvent;
            LevelButton button = newButton.GetComponent<LevelButton> ();
            button.levelText.text = level.levelText;
            button.levelIndex = i;
            i++;
        }
    }
}