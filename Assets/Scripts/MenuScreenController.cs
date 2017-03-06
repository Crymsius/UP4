using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScreenController : MonoBehaviour {

    public void StartQuickGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartStory()
    {
        SceneManager.LoadScene("WorldView");
    }

    public void StartOnline()
    {
        SceneManager.LoadScene("Online");
    }
}