using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCover : MonoBehaviour {
    public void HideCell () {
        this.GetComponent<MeshRenderer> ().enabled = true;
    }
}
