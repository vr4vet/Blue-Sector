using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonText : MonoBehaviour
{

    private TMP_Text buttonText;
    private bool selected = false;
    private string text;
    private RankingStationController rankingStationController;
    // Start is called before the first frame update
    void Start()
    {
        buttonText = GetComponentsInChildren<TMP_Text>()[0];
        text = buttonText.text;
        rankingStationController = GameObject.FindObjectOfType<RankingStationController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleColor() {
        rankingStationController.SyncButtons(this);
        if(selected) {
            buttonText.text = "<color=white>" + text + "</color>";
        }
        else{
            buttonText.text = "<color=green>" + text + "</color>";
        }
        selected = !selected;
    }

    public void SetColor(bool selected) {
        if(selected) {
            buttonText.text = "<color=green>" + text + "</color>";
        }
        else{
            buttonText.text = "<color=white>" + text + "</color>";
        }
        this.selected = selected;
    }
}
