using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{

    // Destroys an object when it collides with a despawn point

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "DespawnPoint")
        {
            Destroy(transform.root.gameObject);
        }
    }
}
