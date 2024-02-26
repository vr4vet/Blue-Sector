using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killFish : MonoBehaviour
{
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.tag != "Fish") { return; }
        // Gets the parent's parent
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;

        fish.gameObject.GetComponent<Animator>().enabled = false;
    }
}
