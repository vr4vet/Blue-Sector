using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSplinterToHand : MonoBehaviour
{

    [SerializeField] private GameObject splint;
    [SerializeField] private BNG.Grabber grabberRight;

    void OnEnable()
    {
        BNG.Grabbable splintGrabbable = splint.GetComponent<BNG.Grabbable>();
        splint.SetActive(true);
        grabberRight.GrabGrabbable(splintGrabbable);
    }
}