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
    [SerializeField] private AddInstructionsToWatch watch;
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
        if (manager.GetStep("Hent Utstyr", "Hent bøtte og spade").IsCompeleted())
        {
            SetEquipmentActive();
        }
        else
        {
            watch.text = "Hent BØTTE OG SPADE på båten for å starte denne oppgaven";
            watch.addInstructions();
        }

    }

    void SetEquipmentActive()
    {
        watch.text = "Kast mat til fisken 5 ganger. Bruk spaden til å hente mat fra bøtten.";
        watch.addInstructions();
        bucket.SetActive(true);
        shovel.SetActive(true);
        BNG.Grabbable bucketGrabbable = bucket.GetComponent<BNG.Grabbable>();
        BNG.Grabbable shovelGrabbable = shovel.GetComponent<BNG.Grabbable>();
        grabberLeft.GrabGrabbable(bucketGrabbable);
        grabberRight.GrabGrabbable(shovelGrabbable);
    }

}