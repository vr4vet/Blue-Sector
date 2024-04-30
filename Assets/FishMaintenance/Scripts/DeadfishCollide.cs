using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadfishCollide : MonoBehaviour
{
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private Task.Subtask subtask;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Deadfish"))
        {
            mm.CompleteStep(subtask.GetStep("Skyv dødfisken i karet"));
            other.GetComponent<Outline>().enabled = false;
        }
    }
}
