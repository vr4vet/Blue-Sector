using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Destroyable")
        {
            return;
        }

        Destroy(collider.transform.parent.transform.parent.gameObject); // Destroy the main fish object
    }
}
