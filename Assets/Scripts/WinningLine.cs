using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningLine : MonoBehaviour {

    private LineRenderer lineRenderer;
    public Vector3 origin = Vector3.zero;
    public Vector3 destination = Vector3.zero;
    // Use this for initialization
    void Start () {
        lineRenderer = GetComponent<LineRenderer> ();
        lineRenderer.SetPosition (0, origin);
        lineRenderer.SetPosition (1, destination);
    }

    public void DisplayLine (Vector3 originPosition, Vector3 destinationPosition) {
        origin = new Vector3 (0, 0, -20f) + originPosition;
        destination = new Vector3 (0, 0, -20f) + destinationPosition;
    }
}
