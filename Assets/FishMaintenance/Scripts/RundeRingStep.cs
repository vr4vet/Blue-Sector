using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RundeRingStep : MonoBehaviour
{
    public GameObject fixedItem;
    public GameObject handheldItem;
    [SerializeField] private BNG.Grabber grabberRight;
    [SerializeField] private Task.Step step;
    private WatchManager _watchManager;

    private void Start()
    {
        _watchManager = WatchManager.Instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("rundeRing"))
        {
            fixedItem.SetActive(true);
            gameObject.SetActive(false);
            handheldItem.GetComponent<BNG.Grabbable>().DropItem(grabberRight, true, true);
            handheldItem.SetActive(false);
            _watchManager.CompleteStep(step);
        }
    }
}
