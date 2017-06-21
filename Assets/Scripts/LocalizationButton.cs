using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LocalizationButton : MonoBehaviour, IPointerClickHandler {
    public string fileName;
    public LocalizationManager localizationManager;
    void Start () {
        localizationManager = LocalizationManager.instance;
    }
    #region IPointerClickHandler implementation
    public void OnPointerClick (PointerEventData eventData) {
        localizationManager.LoadLocalizedText (fileName);
    }
    #endregion
}