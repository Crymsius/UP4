using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellClick : MonoBehaviour, IPointerClickHandler {
    public GameHandler myHandler;
    public Cell myCell;
    void Start () {
        myHandler = GameObject.Find ("GeneralHandler").GetComponent<GameHandler> ();
        myCell = transform.GetComponent<Cell> ();
    }
    #region IPointerClickHandler implementation
    public void OnPointerClick (PointerEventData eventData) {
        if (myCell.available && !myHandler.running) {
            StartCoroutine (myHandler.PutAPawn (myCell));
        }
    }
    #endregion
}