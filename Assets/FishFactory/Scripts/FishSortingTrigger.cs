using System;
using System.Collections;
using System.Collections.Generic;
using BNG;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class FishSortingTrigger : MonoBehaviour
{
    public int score;

    private void OnTriggerEnter(Collider collisionObject)
    {    
        // get parent of parent of bone
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;
        // check if fish has already been sorted
        if (FishSortingButton.fishSortingButton.sortedFish.Contains(fish.gameObject)) return;

        // get the tag of the trigger and open the corresponding door to let the fish fall onto the belt
        if (collisionObject.CompareTag("Destroyable")) 
        {
            // add fish to list of sorted fish
            FishSortingButton.fishSortingButton.sortedFish.Add(fish.gameObject);
            
            // check if fish has been given correct tier
            if (fish.tag == FishSortingButton.fishSortingButton.currentFishTier.ToString())
            {
                Debug.Log("Correct");
                score++;
            } 
            else 
            {
                Debug.Log("Wrong");
                score--;
            }
            // swich case to ensure the right trigger opens the right door
            string triggerTag = gameObject.name;
            switch (triggerTag)
            {
                case "Trigger1":
                    OpenDoor("SlidingDoor1");
                    break;
                case "Trigger2":
                    OpenDoor("SlidingDoor2");
                    break;
                case "Trigger3":
                    OpenDoor("SlidingDoor3");
                    break;
                case "Trigger4":
                    OpenDoor("SlidingDoor4");
                    break;
                case "Trigger5":
                    OpenDoor("SlidingDoor5");
                    break;
                case "Trigger6":
                    OpenDoor("SlidingDoor6");
                    break;
                case "Trigger7":
                    OpenDoor("SlidingDoor7");
                    break;
                case "Trigger8":
                    OpenDoor("SlidingDoor8");
                    break;
            }
        }
    }
    
    // get the game object with doorName, open the door and start coroutine to close the door
    private void OpenDoor(string doorName)
    {
        GameObject door = GameObject.Find(doorName);
        door.transform.Translate(Vector3.back * 0.17f);
        StartCoroutine(CloseDoorAfterDelay(door));

    }

    private IEnumerator CloseDoorAfterDelay(GameObject door)
    {
        // Keep the door open for 2 seconds
        yield return new WaitForSeconds(2);
        // Move the door down
        door.transform.Translate(Vector3.forward * 0.17f);
    }
}
