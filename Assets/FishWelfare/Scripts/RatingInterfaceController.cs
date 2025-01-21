using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RatingInterfaceController : MonoBehaviour
{

    private bool active = true;
    private List<ButtonController> buttons = new List<ButtonController>();
    private Fish fish = null;

    void Start() {
        foreach(ButtonController button in GetComponentsInChildren<ButtonController>()){
            if(button.GetComponent<Image>() != null) {
                buttons.Add(button);
            }
        }
        fish = GetComponent<RectTransform>().root.gameObject.GetComponent<Fish>();
        gameObject.SetActive(active);

    }

    public void ToggleActive() {
        active = !active;
        gameObject.SetActive(active);
    }

    public void SeteActive() {
        if (fish.isGrabbedCount > 0) {
            active = true;
        }
        else {
            active = false;
        }
        gameObject.SetActive(active);
    }

    public bool GetActive() {
        return active;
    }

    public void SyncButtons(ButtonController clicked, int? gillGuess) {
        foreach(ButtonController button in buttons) {
            if(button != clicked) {
                button.SetColor(false);
            } 
            if (gillGuess == button.buttonNmbr) {
                // This is called if a previously guessed fish is returned to tank
                // Then the same fish is grabbed again, light up the correct guess button
                button.SetColor(true);
            }
        }
    }
}
