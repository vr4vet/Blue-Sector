using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandFeeding : MonoBehaviour
{

    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private GameObject bucket;
    [SerializeField] private GameObject shovel;
    [SerializeField] private BNG.Grabber grabberLeft;
    [SerializeField] private BNG.Grabber grabberRight;
    private DropItem dropItem;
    private Task.Step step;
    // Start is called before the first frame update
    void Start()
    {
        dropItem = gameObject.GetComponent<DropItem>();
        step = manager.GetStep("Håndforing", "Kast mat til fisken");
    }

    // Update is called once per frame
    void Update()
    {

        if (step.IsCompeleted()) dropItem.DropAll();

    }
    void OnEnable()
    {
        BNG.Grabbable bucketGrabbable = bucket.GetComponent<BNG.Grabbable>();
        BNG.Grabbable shovelGrabbable = shovel.GetComponent<BNG.Grabbable>();
        // bucket.SetActive(true);
        grabberLeft.GrabGrabbable(bucketGrabbable);
        // shovel.SetActive(true);
        grabberRight.GrabGrabbable(shovelGrabbable);


    }
}
