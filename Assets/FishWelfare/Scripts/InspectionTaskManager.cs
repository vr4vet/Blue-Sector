using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tablet;
using BNG;

public class InspectionTaskManager : MonoBehaviour
{
    private int inspectedFishCount = 0;
    private int inspectionTarget;
    private string activityInspect = "Inspect the fish";
    private string skill1 = "Observant";
    private TaskHolder taskHolder;
    private Fish selectedFish;
    private List<Fish> inspectedFish = new List<Fish>();
    private ScreenController screenController;
    [SerializeField]
    private List<Fish> fishList = new List<Fish>();

    // Start is called before the first frame update
    void Start()
    {
        taskHolder = GameObject.FindObjectOfType<TaskHolder>();
        screenController = GameObject.FindObjectOfType<ScreenController>();
        inspectionTarget = fishList.Count;
        selectedFish = fishList[0];
    }

    public void ProgressInspection(GameObject obj) 
    {
        if((obj.GetComponent("Fish") as Fish) != null) {
            if(AddFish(obj.GetComponent("Fish") as Fish)){
                inspectedFishCount++;
            if(inspectedFishCount == inspectionTarget){
                taskHolder.AddPoints(activityInspect, skill1, 10);
                screenController.DrawScreen(true);
            } else {
                screenController.DrawScreen(false);
            }
            }
        }
    }

    public void RegressInspection(GameObject obj) 
    {
        if((obj.GetComponent("Fish") as Fish) != null) {
            RemoveFish(obj.GetComponent("Fish") as Fish);
            inspectedFishCount--;
            screenController.DrawScreen(false);
        }
    }

    public void SetSelectedFish(Fish fish){
        selectedFish = fish;
        Debug.Log("Gilldamage: " + selectedFish.GetGillDamage());
    }

    public void SetGuess (int guess) {
        if(guess == selectedFish.GetGillDamageGuessed()){
            selectedFish.SetgillDamageGuessed(0);
        }
        else {
            selectedFish.SetgillDamageGuessed(guess);
        }
        Debug.Log("Guess: " + selectedFish.GetGillDamageGuessed());
    }

    public List<Fish> GetInspectedFish() {
        return inspectedFish;
    }

    private bool AddFish(Fish fish) {
        foreach(Fish registered in inspectedFish) {
            if(registered == fish) {
                return false;
            }
        }
        inspectedFish.Add(fish);
        return true;
    }

    private void RemoveFish(Fish fish) {
        foreach(Fish registered in inspectedFish) {
            if(registered == fish) {
                inspectedFish.Remove(registered);
                break;
            }
        }
    }  
}
