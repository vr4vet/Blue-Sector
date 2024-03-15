using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyCheckStep : MonoBehaviour
{
    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private BNG.Grabber grabber;
    [SerializeField] private GameObject grabbableObject;
    [SerializeField] private string equipmentStep;

    [SerializeField] private AddInstructionsToWatch watch;

    private string originalText;

    void Start()
    {
        originalText = watch.text;

    }
    void OnEnable()
    {
        if (manager.GetStep("Hent Utstyr", equipmentStep).RepetionsCompleated > 0)
        {
            SetEquipmentActive();
        }
        else
        {
            watch.text = equipmentStep.ToUpper() + " på båten for å starte denne oppgaven";
            watch.addInstructions();
        }

    }

    void SetEquipmentActive()
    {
        watch.text = originalText;
        watch.addInstructions();
        grabbableObject.SetActive(true);

        BNG.Grabbable grabbable = grabbableObject.GetComponent<BNG.Grabbable>();

        grabber.GrabGrabbable(grabbable);
    }
}


