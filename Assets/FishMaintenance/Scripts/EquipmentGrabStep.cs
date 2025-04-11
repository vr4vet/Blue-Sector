using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGrabStep : MonoBehaviour
{
    //[SerializeField] private MaintenanceManager manager;

    //[SerializeField] private string step;
    [SerializeField] private GameObject guidingHand;

    [SerializeField] private Task.Step step;

    private WatchManager _watchManager;
    


    private void Start()
    {
        _watchManager = WatchManager.Instance;
        //guidingHand.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.name == "Grabber")
        {
            //Debug.Log(manager.GetStep("Get Equipment", step) == null);
            //Task.Step completedStep = manager.GetStep("Get Equipment", step);
            
            //Debug.Log(completedStep == null);
            _watchManager.CompleteStep(step);
            gameObject.SetActive(false);
            guidingHand.SetActive(false);
        }
    }

}
