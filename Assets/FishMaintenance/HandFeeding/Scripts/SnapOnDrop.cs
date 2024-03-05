using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapOnDrop : MonoBehaviour
{
    private BNG.Grabbable grabbable;
    public BNG.SnapZone snapZone;

    public Transform targetPosition;
    private BNG.Grabber grabber;
    // private BNG.ReturnToSnapZone snapReturn;

    // Start is called before the first frame update
    void Start()
    {
        grabbable= GetComponent<BNG.Grabbable>();
        grabber=grabbable.GetPrimaryGrabber();
    }
    // Update is called once per frame
    void Update(){
    // Debug.Log("dist"+ Vector3.Distance(transform.position, targetPosition.position));
    // Debug.Log("player: " + grabbable.GetPrimaryGrabber());
    //   if (Vector3.Distance(transform.position, targetPosition.position)<10f ){
    //         grabber.RemoveValidGrabbable(grabbable);
    //         grabbable.DropItem(true,true);
    //         grabbable.ResetGrabbing();
    //         grabber.AddValidGrabbable(grabbable);
    //         // snapZone.GrabGrabbable(grabbable);
    // }
    }
}
