using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnShape : MonoBehaviour {
     MechanismHandler myMechanismHandler;
    Transform pawnShape;
    int gravity;
    void Start () {
        myMechanismHandler = GameObject.Find("GeneralHandler").GetComponent<MechanismHandler> ();
        gravity = 0;
    }
    public void TurnPawnShape (int gravity) {
        print("coucou" + gravity.ToString ());
        switch (gravity) {
        case 0 :
            transform.eulerAngles = new Vector3(0,0,0);
            break;
        case 1 :
            transform.eulerAngles = new Vector3 (0,0,-90);
            break;
        case 2 :
            transform.eulerAngles = new Vector3 (0,0,180);
            break;
        case 3 :
            transform.eulerAngles = new Vector3 (0,0,90);
            break;
        default :
            transform.eulerAngles = new Vector3 (0,0,0);
            break;
        }
    }
}
