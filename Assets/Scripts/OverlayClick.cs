using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OverlayClick : MonoBehaviour, IPointerClickHandler {
    public GameHandler myHandler;
    public GameObject gameOverPanel;
    void Start () {
        myHandler = GameObject.Find ("GeneralHandler").GetComponent<GameHandler> ();
    }
    #region IPointerClickHandler implementation
    public void OnPointerClick (PointerEventData eventData) {
        if (myHandler.isOver) {
            gameObject.GetComponent <CanvasRenderer> ().SetAlpha (1f);
            gameOverPanel.SetActive (true);
        }
    }
    #endregion
}