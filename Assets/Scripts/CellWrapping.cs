using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellWrapping : MonoBehaviour {
    public MechanismHandlerBoth mechanism;
    private Coord gridSize;

    void Start () {
        mechanism = GameObject.Find ("GeneralHandler").GetComponent<MechanismHandlerBoth> ();
    }
    void OnTriggerEnter2D (Collider2D col) {
        if (col.name == "EdgeCopyRight" && mechanism.isTranslatingRight && gameObject.name == "Cell(Clone)") {
            gridSize = mechanism.myGrid.gridSize;
            GameObject cellClone = GameObject.Instantiate (gameObject, gameObject.GetComponent<Transform> ().position, Quaternion.identity, GameObject.Find ("Generated Grid(Clone)").GetComponent<Transform> ());
            cellClone.name = "Cell(Ghost)";
            gameObject.transform.Translate (new Vector3 (- gridSize.x, 0, 0));
            
        }
        if (col.name == "EdgeCopyLeft" && mechanism.isTranslatingLeft && gameObject.name == "Cell(Clone)") {
            gridSize = mechanism.myGrid.gridSize;
            GameObject cellClone = GameObject.Instantiate (gameObject, gameObject.GetComponent<Transform> ().position, Quaternion.identity, GameObject.Find ("Generated Grid(Clone)").GetComponent<Transform> ());
            cellClone.name = "Cell(Ghost)";
            gameObject.transform.Translate (new Vector3 (gridSize.x, 0, 0));
        }
        if (col.name == "EdgeCopyTop" && mechanism.isTranslatingUp && gameObject.name == "Cell(Clone)") {
            gridSize = mechanism.myGrid.gridSize;
            GameObject cellClone = GameObject.Instantiate (gameObject, gameObject.GetComponent<Transform> ().position, Quaternion.identity, GameObject.Find ("Generated Grid(Clone)").GetComponent<Transform> ());
            cellClone.name = "Cell(Ghost)";
            gameObject.transform.Translate (new Vector3 (0, - gridSize.y, 0));
        }
        if (col.name == "EdgeCopyBottom" && mechanism.isTranslatingDown && gameObject.name == "Cell(Clone)") {
            gridSize = mechanism.myGrid.gridSize;
            GameObject cellClone = GameObject.Instantiate (gameObject, gameObject.GetComponent<Transform> ().position, Quaternion.identity, GameObject.Find ("Generated Grid(Clone)").GetComponent<Transform> ());
            cellClone.name = "Cell(Ghost)";
            gameObject.transform.Translate (new Vector3 (0, gridSize.y, 0));
        }

        if (col.name == "EdgeDestroyRight" && mechanism.isTranslatingRight && gameObject.name == "Cell(Ghost)") {
            Destroy (gameObject);
        }
        if (col.name == "EdgeDestroyLeft" && mechanism.isTranslatingLeft && gameObject.name == "Cell(Ghost)") {
            Destroy (gameObject);
        }
        if (col.name == "EdgeDestroyTop" && mechanism.isTranslatingUp && gameObject.name == "Cell(Ghost)") {
            Destroy (gameObject);
        }
        if (col.name == "EdgeDestroyBottom" && mechanism.isTranslatingDown && gameObject.name == "Cell(Ghost)") {
            Destroy (gameObject);
        }
    }
}
