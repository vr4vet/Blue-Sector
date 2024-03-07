using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixSplint : MonoBehaviour
{
    public GameObject splint;
    public GameObject splintGuide;
    public GameObject handSplint;
    [SerializeField] private BNG.Grabber grabberRight;
    [SerializeField] private MaintenanceManager manager;

    void OnTriggerEnter(Collider other)
    {
        splint.SetActive(true);
        splintGuide.SetActive(false);
        handSplint.GetComponent<BNG.Grabbable>().DropItem(grabberRight, true, true);
        handSplint.SetActive(false);
        manager.CompleteStep("Vedlikehold", "Runde På Ring", "Legg til splinter på kjetting");
    }
}
