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
    public Nets nets;
    public Trigger trigger;
    public Pawn pawn;
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
    public struct Nets {
        public bool netx;
        public bool nety;
    }

    [System.Serializable]
    public struct Trigger {
        public bool isTrigger;
        [Range(0,8)]
         //0 : 90r | 1 : 90l | 2 : 180 | 3 : gravity | 4 : rotationChoice | 5 : translationR | 6 : translationL | 7 : translationU | 8 : translationD
        public int triggerType;
    }

    [System.Serializable]
    public struct Pawn {
        public bool isPawn;
        [Range(-1,2)]
        public int pawnType; //-1 : neutral | 0 : player1 | 1 : player 2 | 2 : common
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