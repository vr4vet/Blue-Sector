using System.Collections;
using System.Collections.Generic;
using BNG;
using JetBrains.Annotations;
using UnityEngine;

public class FishSortingTrigger : MonoBehaviour
{
    private GameObject slidingDoor;

    private void OnTriggerEnter(Collider collisionObject)
    {    
        // (Not functional)get the tag of the trigger and open the corresponding door to let the fish fall onto the belt
        if (collisionObject.CompareTag("Destroyable")) 
        {
            string triggerTag = collisionObject.gameObject.tag;
            switch (triggerTag)
            {
                case "Trigger1":
                    slidingDoor = GameObject.FindWithTag("SlidingDoor1");
                    break;
                case "Trigger2":
                    slidingDoor = GameObject.FindWithTag("SlidingDoor2");
                    break;
                case "Trigger3":
                    slidingDoor = GameObject.FindWithTag("SlidingDoor3");
                    break;
                case "Trigger4":
                    slidingDoor = GameObject.FindWithTag("SlidingDoor4");
                    break;
            }
            slidingDoor.transform.position += new Vector3(0, 100, 0);
            new WaitForSeconds(2);
            slidingDoor.transform.position += new Vector3(0, -100, 0);
        }
    }
}
