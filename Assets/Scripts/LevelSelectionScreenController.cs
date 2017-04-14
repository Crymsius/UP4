using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelSelectionScreenController : MonoBehaviour {

    public void StartQuickGame()
    {
        SceneManager.LoadScene("GridScreen");
    }
}