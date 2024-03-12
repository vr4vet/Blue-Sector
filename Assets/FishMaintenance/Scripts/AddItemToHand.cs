using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemToHand : MonoBehaviour
{

    [SerializeField] private BNG.Grabber grabber;

    void OnEnable()
    {
        BNG.Grabbable itemGrabbable = gameObject.GetComponent<BNG.Grabbable>();
        gameObject.SetActive(true);
        grabber.GrabGrabbable(itemGrabbable);
    }
}