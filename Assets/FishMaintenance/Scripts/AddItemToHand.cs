using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemToHand : MonoBehaviour
{

    public BNG.Grabber grabber;
    [SerializeField] private GameObject bucket;


    void OnEnable()
    {
        BNG.Grabbable itemGrabbable = gameObject.GetComponent<BNG.Grabbable>();
        grabber.GrabGrabbable(itemGrabbable);
    }

}