using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

[System.Serializable]
public class Cell : MonoBehaviour {
    
    public Coord coordinates;
    public Walls walls;
    public Trigger trigger;
    //cell cachée mais jouable
    public bool hidden = false;
    //cell cachée et non jouable -> mur
    public bool full = false;
    // Peut-on placer un pion dessus ?
    [HideInInspector]
    public bool available = true;

    [System.Serializable]
    public struct Walls {
        public bool wallx;
        public bool wally;
        public bool wallxy;
    }

    [System.Serializable]
    public struct Trigger {
        public bool isTrigger;
        [Range(0,3)]
        public int triggerType; //0 : 90r | 1 : 90l | 2 : 180 | 3 : gravity
    }

    #if UNITY_EDITOR
    /// <summary>
    /// Utilisé uniquement lors de la création de grille dans GridCreator.
    /// Sert à actualiser la vue de la grille et à enregistrer les modifs
    /// </summary>
    void OnValidate () {
        if (SceneManager.GetActiveScene().name == "GridCreator") {
            GameObject.Find ("GridHolder").GetComponent<GridGenerator> ().GenerateFromValidate ();
        }
    }
    #endif
}