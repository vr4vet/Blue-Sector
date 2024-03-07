using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeStep : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private MaintenanceManager manager;


    void Awake()
    {
    }

    // Update is called once per frame
    void OnEnable()
    {
        manager.CompleteStep("Runde På Ring", "Reparer tau på merd");
        manager.PlaySuccess();
    }


}


