using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using TMPro;

public class ScreenController : MonoBehaviour
{

    private InspectionTaskManager inspectionTaskManager;
    private TMP_Text screen;

    // Start is called before the first frame update
    void Start()
    {
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
        screen = GameObject.FindGameObjectWithTag("Display").GetComponent("TMP_Text") as TMP_Text;
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
