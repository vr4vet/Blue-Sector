using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Destroyable") { return; }

        SpawnedObject obj = other.GetComponent<SpawnedObject>();
        if (obj)
        {
            obj.UpdateListeners();
        }

        Destroy(other.gameObject);
    }
}
