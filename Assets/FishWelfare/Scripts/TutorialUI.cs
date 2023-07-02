using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
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

    public void SeteActive(bool active) {
        this.active = active;
        if(active) {
            gameObject.SetActive(active);
        } else {
            StartCoroutine(DelayedClose());
        }

    }

    private IEnumerator DelayedClose() {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(active);
    }
}
