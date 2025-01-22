using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Bone : MonoBehaviour, IPointerClickHandler
{
    private Fish parent;
    [HideInInspector]
    public bool isInWater;
    private bool isGrabbed;
    private Rigidbody rigidBody;

    private GameObject lastMarkedLouse = null;
    private GameObject pointerFinger = null;
    public GameObject marker;
    private List<GameObject> liceList = new List<GameObject>();
    public LayerMask layer;
    public Vector3 storedPosition;
    public Quaternion storedRotation;
    private InspectionNPCBehavior inspectionNPCBehavior;

    // Start is called before the first frame update
    void Start()
    {
        //parent = transform.root.gameObject.GetComponent<Fish>();
        parent = GetComponentInParent<Fish>();
        rigidBody = GetComponent<Rigidbody>();
        //The point from which the raycast targeting lice on fishbodie will have its origin. In this case it is RightHandPointer in XR Rig Advanced
        pointerFinger = GameObject.FindGameObjectWithTag("Pointer");
        liceList = parent.FindObjectwithTag("Louse");
        layer = parent.layer;
        marker = parent.marker;
        // Prevent fish from colliding with player to prevent fish from taking damage when teleporting and other issues
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Bone"));
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bone"), LayerMask.NameToLayer("Bone"));
        // Get the behavior script for the inspection NPC
        inspectionNPCBehavior = GameObject.Find("NPCSpawner").GetComponent<InspectionNPCBehavior>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        MarkLouse(eventData);
    }

    public void MarkLouse(PointerEventData eventData){
        //Debug.Log("Marking Louse");
        lastMarkedLouse = checkForLouse(eventData.pointerCurrentRaycast.worldPosition);
        if(lastMarkedLouse != null){
            parent.markSound.Play(0);
        }
    }

    public GameObject checkForLouse(Vector3 origin) {
        if(Physics.SphereCast(origin - (pointerFinger.transform.forward*.5f), 0.02f, pointerFinger.transform.forward, out RaycastHit hitInfo, 10f, layer)) {
            GameObject hit = hitInfo.collider.gameObject;
            foreach(GameObject louse in liceList) {
                if (hit == louse && !louse.GetComponent<Louse>().marked) {
                    Debug.Log("Same louse");
                    louse.GetComponent<Louse>().marked = true;
                    parent.markedLice++;
                    return louse;
                }
            }
        }
        Debug.Log("No louse");
        return null;
    }

    private void OnCollisionEnter(Collision other)
    {
        //Debug.Log(other.gameObject.layer);
        if (other.transform.gameObject.CompareTag("Water"))
        {
            //Debug.Log("Collided with water");
            parent.checkForDamage(true, other.relativeVelocity.magnitude);
        }
        else if (!other.transform.gameObject.CompareTag("Bone") && !other.transform.gameObject.CompareTag("Fish"))
        {
            //Debug.Log("Collided with something else");
            //Debug.Log(other.gameObject.name);
            parent.checkForDamage(false, other.relativeVelocity.magnitude);
        }
        else if (other.transform.gameObject.CompareTag("Desk")) {
            // Check if a fish has been placed on the table
            Debug.Log("Start dialogue");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer);
        if (other.CompareTag("Water"))
        {
            SetIsInWater(true);

            // set respawn position to the current tank
            parent.findClosestTank();
            parent.GetComponent<Respawner>().SetRespawnPosition(parent.waterBodyCenter);

            // determine which tutorial entry to progress, and whether to add fish to the score wall
            if (parent.tank.isGoal)
            {
                if (parent.IsInWater())
                {
                    parent.tank.inspectionTaskManager.ProgressInspection(parent);
                    parent.tank.tutorialEntry.SetCompleted();

                    // complete step for placing fish in recovery tank
                    if (!parent.PlacedInRecoveryTank)
                    {
                        parent.PlacedInRecoveryTank = true;
                        parent.m_OnPlacedInRecoveryTank.Invoke();
                    }
                }

            }
        }
        else if (other.CompareTag("Desk"))
        {
            // set respawn position to table top
            parent.GetComponent<Respawner>().SetRespawnPosition(Vector3.Scale(other.bounds.center, new Vector3(1, 1.3f, 1)));

            // Active the rating dialogue when a fish is placed on the table
            if (inspectionNPCBehavior.dialogueStarted == false) {
                // Can only be called once
                inspectionNPCBehavior.dialogueStarted = true;

                inspectionNPCBehavior.StartInspectionDialogue();
                Debug.Log("Start dialogue");
            }
            
            // Complete step for bringing salmon to the table
            if (!parent.BringFishStepCompleted)
            {
                parent.BringFishStepCompleted = true;
                parent.m_OnBroughtToTable.Invoke();
            }
        }
        else if (!parent.tank.isGoal && other.CompareTag("Hand"))
        {
            parent.tank.tutorialEntry.SetCompleted();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            //Debug.Log("Water exited");
            SetIsInWater(false);

            // remove fish from score wall it just left the "wake-up tank"
            if (parent.tank.isGoal)
            {
                if (!parent.IsInWater())
                {
                    parent.tank.inspectionTaskManager.RegressInspection(parent);
                }
            }
        }
            
    }

    public void SetIsInWater(bool isInWater) 
    {
        this.isInWater = isInWater;
    }
    

    public void SetIsGrabbed(bool isGrabbed) {
        if (isGrabbed) parent.isGrabbedCount++;
        else parent.isGrabbedCount--;
    }

    public Fish GetParent() {
        return parent;
    }
}
