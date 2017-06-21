using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {
    int variant;
    int language;
    int opponent;
    GameObject optionPanel;
    GameObject variant0Button;
    GameObject variant1Button;
    GameObject frenchButton;
    GameObject englishButton;
    GameObject humanButton;
    GameObject iaButton;
    void Start () {
        optionPanel = GameObject.Find ("OptionsPanel");

        variant0Button = GameObject.Find ("Variant0Button");
        variant1Button = GameObject.Find ("Variant1Button");

        frenchButton = GameObject.Find ("FrenchButton");
        englishButton = GameObject.Find ("EnglishButton");
        
        humanButton = GameObject.Find ("HumanButton");
        iaButton = GameObject.Find ("IAButton");

        variant = VariantSelector.variant;
        language = int.Parse (GameObject.Find("LocalizationManager").GetComponent<LocalizationManager> ().GetLocalizedValue ("language_id"));
        opponent = OpponentSelector.opponent;

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
        } else if (language == 1) {
            englishButton.GetComponent<Button> ().interactable = true;
            frenchButton.GetComponent<Button> ().interactable = false;
        }
        
        if (opponent == 0) {
            humanButton.GetComponent<Button> ().interactable = false;
            iaButton.GetComponent<Button> ().interactable = true;
        } else if (opponent == 1) {
            humanButton.GetComponent<Button> ().interactable = true;
            iaButton.GetComponent<Button> ().interactable = false;
        }
    }

    public void SetVariant (int variant) {
        VariantSelector.variant = variant;
    }

    public void SetOpponent (int opponent) {
        if (opponent == 0) {
            OpponentSelector.opponents = new List<string> () {"human", "human" };
            OpponentSelector.opponent = 0;
        } else if (opponent == 1) {
            OpponentSelector.opponents = new List<string> () {"human", "IA" };
            OpponentSelector.opponent = 1;
        }
    }
}
