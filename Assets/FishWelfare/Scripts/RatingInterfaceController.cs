using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RatingInterfaceController : MonoBehaviour
{

    private bool active = false;
    private List<ButtonController> buttons = new List<ButtonController>();

    void Start() {
        foreach(ButtonController button in GetComponentsInChildren<ButtonController>()){
            if(button.GetComponent<Image>() != null) {
                buttons.Add(button);
            }
        }
        gameObject.SetActive(active);
    }

    public void ToggleActive() {
        active = !active;
        gameObject.SetActive(active);
    }

    public void SeteActive(bool active) {
        this.active = active;
        gameObject.SetActive(active);
    }

    public void SyncButtons(ButtonController clicked) {
        foreach(ButtonController button in buttons) {
            if(button != clicked) {
                button.SetColor(false);
            }
        }
    }
}
