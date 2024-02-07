using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Fish"));
    }

    // Update is called once per frame
    void Update()
    {
        if(isInWater && !isGrabbed){
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        } else {
            rigidBody.freezeRotation = false;
        }
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
        if (other.transform.gameObject.tag == "Water")
        {
            Debug.Log("Collided with water");
            parent.checkForDamage(true, other.relativeVelocity.magnitude);
        }
        else if (other.transform.gameObject.gameObject.tag != "Bone" && other.transform.gameObject.tag != "Fish")
        {
            Debug.Log("Collided with something else");
            parent.checkForDamage(false, other.relativeVelocity.magnitude);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer);
        if (other.tag == "Water")
        {
            Debug.Log("Water entered");
            Debug.Log(parent.tank.name);
            SetIsInWater(true);
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
        {
            Debug.Log("Water exited");
            SetIsInWater(false);
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
