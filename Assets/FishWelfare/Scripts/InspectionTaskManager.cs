using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tablet;
using BNG;

public class InspectionTaskManager : MonoBehaviour
{
    private int inspectedFish = 0;
    private int inspectionTarget;
    private string activityInspect = "Inspect the fish";
    private string skill1 = "Observant";
    private TaskHolder taskHolder;
    private Fish selectedFish;
    [SerializeField]
    private List<Fish> fishList = new List<Fish>();
    // Start is called before the first frame update
    void Start()
    {
        taskHolder = GameObject.FindObjectOfType<TaskHolder>();
        inspectionTarget = fishList.Count;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(taskHolder.GetTaskList()[0].activities[0].aktivitetName);
    }

    public void ProgressInspection() 
    {
        inspectedFish++;
        if(inspectedFish == inspectionTarget){
            taskHolder.AddPoints(activityInspect, skill1, 10);
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
}
