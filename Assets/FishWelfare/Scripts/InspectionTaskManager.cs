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
    public RatingInterfaceController ratingInterfaceController;

    public LiceInterfaceController liceInterfaceController;

    // Start is called before the first frame update
    void Start()
    {
        taskHolder = GameObject.FindObjectOfType<TaskHolder>();
        screenController = GameObject.FindObjectOfType<ScreenController>();
        inspectionTarget = fishList.Count;
        selectedFish = fishList[0];
    }

    public void ProgressInspection(Fish fish) 
    {
        if(AddFish(fish)){
            inspectedFishCount++;
            if(inspectedFishCount == inspectionTarget){
                taskHolder.AddPoints(activityInspect, skill1, 10);
                screenController.DrawScreen(fish);
            } else {
                screenController.DrawScreen(fish);
            }
        }   
    }

    public void RegressInspection(Fish fish) 
    {
        RemoveFish(fish);
        inspectedFishCount--;
        screenController.RemoveItem(fish.GetId());
    }

    public void SetSelectedFish(Fish fish){
        if(selectedFish != fish) {
            selectedFish = fish;
            ratingInterfaceController.SyncButtons(null);
            liceInterfaceController.SetLice(selectedFish.markedLice);
        }
        //Debug.Log("Gilldamage: " + selectedFish.GetGillDamage());
    }

    public void SetGuess (int guess) {
        if(guess == selectedFish.GetGillDamageGuessed()){
            selectedFish.SetgillDamageGuessed(0);
        }
        else {
            selectedFish.SetgillDamageGuessed(guess);
        }
        //Debug.Log("Guess: " + selectedFish.GetGillDamageGuessed());
    }

    public void SetLiceCount() {
        selectedFish.markedLice = (int)liceInterfaceController.liceSlider.value;
        liceInterfaceController.CountLice();
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
