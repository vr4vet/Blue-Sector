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
            output += "Fish " + fish.GetId() + ":\nGuess:\nGill Damage: " + fish.GetGillDamageGuessed() + "   Lice: " + fish.GetMarkedLice() + "\nAnswer:\nGill Damage: " + fish.GetGillDamage() + "  Lice: " + fish.GetLiceList().Count + "\nHealth: " + fish.health + "/10";
        }
        screen.text = output;
    }
}
