using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddItemToHand : MonoBehaviour
{

    [SerializeField] private GameObject item;
    [SerializeField] private BNG.Grabber grabberRight;

    void OnEnable()
    {
        BNG.Grabbable itemGrabbable = item.GetComponent<BNG.Grabbable>();
        item.SetActive(true);
        grabberRight.GrabGrabbable(itemGrabbable);
    }
}