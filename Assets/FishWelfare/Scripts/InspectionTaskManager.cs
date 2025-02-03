using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tablet;
using BNG;
using Task;
using UnityEngine.Events;

public class InspectionTaskManager : MonoBehaviour
{
    private int inspectedFishCount = 0;
    private int inspectionTarget;
    private Fish selectedFish;
    private List<Fish> inspectedFish = new();
    private ScreenController screenController;
    [SerializeField]
    private List<Fish> fishList = new();
    public RatingInterfaceController ratingInterfaceController;

    public LiceInterfaceController liceInterfaceController;

    // event to trigger "Wakey wakey" skill when all fish are placed in the wake up tank
    public UnityEvent m_OnAllFishWakeupTank;
    private bool _allFishInWakeupTank = false;

    // Start is called before the first frame update
    void Start()
    {
        if (m_OnAllFishWakeupTank == null)
            m_OnAllFishWakeupTank = new();

        screenController = GameObject.FindObjectOfType<ScreenController>();
        inspectionTarget = fishList.Count;
        selectedFish = fishList[0];
    }

    public void ProgressInspection(Fish fish) 
    {
        if(AddFish(fish)){
            inspectedFishCount++;
            if(inspectedFishCount == inspectionTarget)
                screenController.DrawScreen(fish);
            else
                screenController.DrawScreen(fish);
        }
        
        // invoke event to unlock "Wakey wakey" skill badge
        if (!_allFishInWakeupTank && inspectedFishCount >= inspectionTarget)
        {
            _allFishInWakeupTank = true;
            m_OnAllFishWakeupTank.Invoke();
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
            ratingInterfaceController.SyncButtons(null, fish.GetGillDamageGuessed());
            liceInterfaceController.SetLice(selectedFish.markedLice);
        }
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

        // invoke event to complete inspection step
        if (!selectedFish.HasBeenInspected)
        {
            selectedFish.HasBeenInspected = true;
            selectedFish.m_OnInspected.Invoke();
        }
    }

    public void SetLiceCount() {
        selectedFish.markedLice = (int)liceInterfaceController.liceSlider.value;
        liceInterfaceController.CountLice();

        // invoke event to complete inspection step
        if (!selectedFish.HasBeenInspected)
        {
            selectedFish.HasBeenInspected = true;
            selectedFish.m_OnInspected.Invoke();
        }
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

    public void ClearSelectedFish() {
        // For setting "selectedFish" to null if none is out of any tanks
        selectedFish = null;
    }

    public Fish GetSelectedFish() {
        return selectedFish;
    }
}
