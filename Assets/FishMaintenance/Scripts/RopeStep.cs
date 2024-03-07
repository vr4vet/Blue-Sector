using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeStep : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private MaintenanceManager manager;
    [SerializeField] private bool missing;


    void Awake()
    {
    }

    // Update is called once per frame
    void OnEnable()
    {
        if (missing)
        {
            manager.CompleteStep("Vedlikehold", "Runde På Ring", "Legg til tau på merd");
        }
        else
        {
            manager.CompleteStep("Vedlikehold", "Runde På Ring", "Reparer tau på merd");
        }
        manager.PlaySuccess();
    }


}


