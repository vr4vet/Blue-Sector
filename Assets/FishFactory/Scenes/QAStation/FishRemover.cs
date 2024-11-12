using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishRemover : MonoBehaviour
{

    void OnTriggerEnter(Collider trigger)
    {
        Debug.Log(trigger.gameObject.transform.parent.parent.parent.parent.name);
        if (trigger.gameObject.transform.parent.parent.parent.parent.name == "FactoryFishSpawner")
        {
            Destroy(trigger.gameObject.transform.parent.parent.parent.gameObject);
        }
    }
}
