using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelectionScreenController : MonoBehaviour {

    public void StartQuickGame(int levelIndex) {
        LevelLoader.level = levelIndex;
        SceneManager.LoadScene("GridScreen");
    }
}