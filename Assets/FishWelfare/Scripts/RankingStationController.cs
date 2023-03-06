using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using TMPro;

public class RankingStationController : MonoBehaviour
{

    private List<ButtonText> buttons = new List<ButtonText>();
    private InspectionTaskManager inspectionTaskManager;
    private TMP_Text screen;

    // Start is called before the first frame update
    void Start()
    {
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
        screen = GameObject.FindGameObjectWithTag("Display").GetComponent("TMP_Text") as TMP_Text;
        foreach(ButtonText button in GetComponentsInChildren<ButtonText>()){
            buttons.Add(button);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SyncButtons(ButtonText clicked) {
        foreach(ButtonText button in buttons) {
            if(button != clicked) {
                button.SetColor(false);
            }
        }
    }

    public void DrawScreen(bool complete) {
        string output = "";
        foreach(Fish fish in inspectionTaskManager.GetInspectedFish()) {
            output += "Fish " + fish.GetId() + ": " + "Guess: " + fish.GetGillDamageGuessed();
            if(complete) {
                output += "    Answer: " + fish.GetGillDamage();
            }
            output += "\n";
        }
        screen.text = output;
    }
}
