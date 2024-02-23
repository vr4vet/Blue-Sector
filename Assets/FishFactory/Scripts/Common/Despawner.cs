using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.tag != "Destroyable") { return; }
        // Gets the parent's parent
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;

        SpawnedObject obj = fish.GetComponent<SpawnedObject>();
        if (obj)
        {
            obj.UpdateListeners();
        }

        Destroy(fish.gameObject);
    }
}
