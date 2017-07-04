using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateCell : MonoBehaviour {
    [Range(0,3)]
    public int direction = 0;
    
    // Update is called once per frame
    void Update () {
        // transform.Rotate(0,0, direction);
    }
}