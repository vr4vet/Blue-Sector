using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public InspectionTaskManager inspectionTaskManager;
    public bool isGoal = false;
    private BoxCollider collider = new BoxCollider();
    private AudioSource waterSound;
    public float sedativeConsentration = 0f;

public TutorialEntry tutorialEntry;
    // Start is called before the first frame update
    void Start()
    {
        inspectionTaskManager = FindObjectOfType<InspectionTaskManager>();
        collider = GetComponent<BoxCollider>();
        waterSound = GetComponent<AudioSource>();
    }

/*//The two following functions should be refactored and moved into Bone
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Bone"){
            //waterSound.Play(0);
            if(isGoal){
                Debug.Log("Something bony entered!");
                inspectionTaskManager.ProgressInspection(other.GetComponentInParent<Fish>());
                tutorialEntry.SetCompleted();
            }
        } else if (other.tag == "Hand" && !isGoal) {
            tutorialEntry.SetCompleted();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Bone"){
            if (isGoal){
                //if(other.gameObject.GetComponent<Bone>().GetParent().isInWaterCount == 0){
                if (!other.gameObject.GetComponent<Bone>().GetParent().IsInWater()){
                    inspectionTaskManager.RegressInspection(other.GetComponentInParent<Fish>());
                }
            }   
        }
    }*/
}
