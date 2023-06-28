using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private InspectionTaskManager inspectionTaskManager;
    public bool isGoal = false;
    private BoxCollider collider = new BoxCollider();
    private AudioSource waterSound;
    public float sedativeConsentration = 0f;

public TutorialEntry tutorialEntry;
    // Start is called before the first frame update
    void Start()
    {
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
        collider = GetComponent<BoxCollider>();
        waterSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

//The two following functions should be refactored and moved into Bone
    private void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Bone"){
            //waterSound.Play(0);
            other.gameObject.GetComponent<Bone>().UpdateWaterBody(transform.position.y + transform.localScale.y, transform.position, transform.localScale.x, transform.localScale.z, true);
            if(isGoal){
                inspectionTaskManager.ProgressInspection(other.transform.root.gameObject.GetComponent<Fish>());
                tutorialEntry.SetCompleted();
            }
        } else if (other.tag == "Hand" && !isGoal) {
            tutorialEntry.SetCompleted();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.tag == "Bone"){
            other.gameObject.GetComponent<Bone>().UpdateWaterBody(0f, new Vector3(0f,0f,0f), 0f, 0f, false);
            if(isGoal){
                if(other.gameObject.GetComponent<Bone>().GetParent().isInWaterCount == 0){
                    inspectionTaskManager.RegressInspection(other.transform.root.gameObject.GetComponent<Fish>());
                }
            }   
        }
    }
}
