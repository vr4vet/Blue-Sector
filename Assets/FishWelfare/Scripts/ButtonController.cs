using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private bool selected = false;
    private string text;
    private RatingInterfaceController ratingInterfaceController;
    Color unselectedColor = new Color(0,35,154);

    void Start()
    {
        ratingInterfaceController = GameObject.FindObjectOfType<RatingInterfaceController>();
    }

    public void ToggleColor() {
        ratingInterfaceController.SyncButtons(this);
        if(selected) {
            gameObject.GetComponent<Image>().color = unselectedColor;
        }
        else{
            gameObject.GetComponent<Image>().color = Color.green;
        }
        selected = !selected;
    }

    public void SetColor(bool selected) {
        if(selected) {
            gameObject.GetComponent<Image>().color = Color.green;
        }
        else{
            gameObject.GetComponent<Image>().color = unselectedColor;
        }
        this.selected = selected;
    }
}
