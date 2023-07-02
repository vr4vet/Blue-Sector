using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using TMPro;

public class ScreenController : MonoBehaviour
{

    private InspectionTaskManager inspectionTaskManager;
    private TMP_Text screen;
    private List<Fish> inspectedFish;
    public RowUi rowUi; 

    // Start is called before the first frame update
    void Start()
    {
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
    }

    public void DrawScreen(Fish fish) {
        RowUi row = Instantiate(rowUi, transform).GetComponent<RowUi>();
        row.fish.text = fish.GetId().ToString();
        row.gillDamage.text = fish.GetGillDamageGuessed().ToString() + "/" + fish.GetGillDamage().ToString();
        //row.handling.text = fish.health.ToString();
        row.lice.text =fish.markedLice.ToString() + "/" + fish.numberOfLice;
        row.score.text = CalculateScore(fish.GetGillDamageGuessed(), fish.GetGillDamage(), fish.markedLice, fish.numberOfLice, fish.health).ToString() + "/30";
        fish.scoreBoardEntry = row;
    }

    public void RemoveItem(int id) {
        inspectedFish = GameObject.FindObjectOfType<InspectionTaskManager>().GetInspectedFish();
        foreach(RowUi item in GetComponentsInChildren<RowUi>()){
            if(id.ToString() == item.fish.text){
                Destroy(item.gameObject);
            }
        }
    }

    private int CalculateScore(int guessedGillDamage, int gillDamage, int markedLice, int lice, int handling) {
        int gillScore = 0;
        int diff = Mathf.Abs(gillDamage - guessedGillDamage);
        switch (diff)
        {
            case 0:
                gillScore = 10;
                break;
            case 1:
                gillScore = 8;
                break;
            case 2:
                gillScore = 6;
                break;
            case 3:
                gillScore = 4;
                break;
            case 4:
                gillScore = 2;
                break;
            case 5:
                gillScore = 0;
                break;
        }
        int liceScore = 10 - Mathf.Abs(10 - (markedLice/lice)*10);
        //Debug.Log("Gill score: " + gillScore + " Lice score: " + liceScore + " handling: " + handling);
        return gillScore + liceScore + handling;
    }
}
