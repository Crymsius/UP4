using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellWrapping : MonoBehaviour {
    public MechanismHandlerBoth mechanism;

    void Start () {
        mechanism = GameObject.Find ("GeneralHandler").GetComponent<MechanismHandlerBoth> ();
    }
    void OnTriggerEnter2D (Collider2D col) {
        if (col.name == "EdgeCopyRight" && mechanism.isTranslatingRight){
            GameObject cellClone = GameObject.Instantiate (gameObject, gameObject.GetComponent<Transform> ().position - new Vector3 (6f,0,0), Quaternion.identity, GameObject.Find("Generated Grid(Clone)").GetComponent<Transform> ());
            cellClone.name = "Cell(Clone)";
            cellClone.GetComponent<Collider2D> ().isTrigger = false;
        }
        if (col.name == "EdgeCopyLeft" && mechanism.isTranslatingLeft){
            GameObject cellClone = GameObject.Instantiate (gameObject, gameObject.GetComponent<Transform> ().position + new Vector3 (6f,0,0), Quaternion.identity, GameObject.Find("Generated Grid(Clone)").GetComponent<Transform> ());
            cellClone.name = "Cell(Clone)";
            cellClone.GetComponent<Collider2D> ().isTrigger = false;
        }
        if (col.name == "EdgeCopyTop" && mechanism.isTranslatingUp){
            GameObject cellClone = GameObject.Instantiate (gameObject, gameObject.GetComponent<Transform> ().position - new Vector3 (0,6f,0), Quaternion.identity, GameObject.Find("Generated Grid(Clone)").GetComponent<Transform> ());
            cellClone.name = "Cell(Clone)";
            cellClone.GetComponent<Collider2D> ().isTrigger = false;
        }
        if (col.name == "EdgeCopyBottom" && mechanism.isTranslatingDown){
            GameObject cellClone = GameObject.Instantiate (gameObject, gameObject.GetComponent<Transform> ().position - new Vector3 (0,-6f,0), Quaternion.identity, GameObject.Find("Generated Grid(Clone)").GetComponent<Transform> ());
            cellClone.name = "Cell(Clone)";
            cellClone.GetComponent<Collider2D> ().isTrigger = false;
        }
        if (col.name == "EdgeDestroyRight" && mechanism.isTranslatingRight){
            Destroy(gameObject);
        }
        if (col.name == "EdgeDestroyLeft" && mechanism.isTranslatingLeft){
            Destroy(gameObject);
        }
        if (col.name == "EdgeDestroyTop" && mechanism.isTranslatingUp){
            Destroy(gameObject);
        }
        if (col.name == "EdgeDestroyBottom" && mechanism.isTranslatingDown){
            Destroy(gameObject);
        }
    }
}
