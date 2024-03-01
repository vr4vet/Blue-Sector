using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunFish : MonoBehaviour
{
    private int nrFishStun = 0;
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.tag != "Destroyable") { return; }
        // Gets the parent's parent
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;
        if (fish.GetComponent<Animator>().enabled == true) 
        {
            fish.GetComponent<Animator>().enabled = false;
            nrFishStun++;
        }
    }
}
