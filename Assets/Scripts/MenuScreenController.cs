using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScreenController : MonoBehaviour {

    public void StartQuickGame () {
        SceneManager.LoadScene ("GridScreen");
    }

    public void SelectLevelMenu () {
        SceneManager.LoadScene ("LevelSelectionScreen");
    }

    public void StartStory () {
        SceneManager.LoadScene ("WorldView");
    }

    public void StartOnline () {
        SceneManager.LoadScene ("Online");
    }

    public void MainMenu () {
        SceneManager.LoadScene ("MenuScreen");
    }

    public void ExitGame () {
        Application.Quit();
    }
}