using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {
    int variant;
    int language;
    GameObject optionPanel;
    GameObject variant0Button;
    GameObject variant1Button;
    GameObject frenchButton;
    GameObject englishButton;
    void Start () {
        optionPanel = GameObject.Find ("OptionsPanel");

        variant0Button = GameObject.Find ("Variant0Button");
        variant1Button = GameObject.Find ("Variant1Button");

        frenchButton = GameObject.Find ("FrenchButton");
        englishButton = GameObject.Find ("EnglishButton");

        variant = VariantSelector.variant;
        language = int.Parse (GameObject.Find("LocalizationManager").GetComponent<LocalizationManager> ().GetLocalizedValue ("language_id"));

        optionPanel.SetActive(false);

        if (variant == 0) {
            variant0Button.GetComponent<Button> ().interactable = false;
            variant1Button.GetComponent<Button> ().interactable = true;
        } else {
            variant0Button.GetComponent<Button> ().interactable = true;
            variant1Button.GetComponent<Button> ().interactable = false;
        }

        if (language == 0) {
            englishButton.GetComponent<Button> ().interactable = false;
            frenchButton.GetComponent<Button> ().interactable = true;
        } else if (language ==1) {
            englishButton.GetComponent<Button> ().interactable = true;
            frenchButton.GetComponent<Button> ().interactable = false;
        }
    }

    public void SetVariant (int variant) {
        VariantSelector.variant = variant;
    }
}
