using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    private InspectionTaskManager inspectionTaskManager;
    public bool isGoal = false;
    // Start is called before the first frame update
    void Start()
    {
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        Debug.Log("*TRIGGERED*");
        if(isGoal){
            inspectionTaskManager.ProgressInspection(other.gameObject);
        }
        if(other.tag == "Floatable"){
            other.gameObject.GetComponent<Floating>().waterHeight = other.transform.localScale.y + other.transform.position.y;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(isGoal){
            inspectionTaskManager.RegressInspection(other.gameObject);
        }
        if(other.tag == "Floatable"){
            other.GetComponent<Floating>().waterHeight = 0f;
        }
    }
}
