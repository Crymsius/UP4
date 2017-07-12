using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnChoiceController : MonoBehaviour {

    // Use this for initialization

    public Canvas canvasChoice;
    public Camera mainCamera;
    public int idRotation;
    public bool clicked;
    void Start () {
        clicked = false;
        //Tout à passer dans le call du trigger, pas dans le start
        canvasChoice = GetComponentInChildren<Canvas> ();
        mainCamera = GameObject.Find ("Main Camera").GetComponent<Camera>();
        canvasChoice.worldCamera = mainCamera;
        canvasChoice.gameObject.SetActive (false);
    }

    public IEnumerator ChooseRotation () {
        canvasChoice.gameObject.SetActive (true);
        while (!clicked) {
            yield return null;
        }
        clicked = false;
    }

    public void ClickRotation (int idRotationClicked) {
        clicked = true;
        idRotation = idRotationClicked;
    }
}
