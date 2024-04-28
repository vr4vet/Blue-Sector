using System.Collections;
using UnityEngine;

public class FishSortingTrigger : MonoBehaviour
{
    // Variables to allow for tracking of performance
    private int correctSortedFish;
    private int incorrectSortedFish;

    /// <summary>
    /// When the fish enters the trigger, check if the fish has been sorted and if it has the correct tier
    /// </summary>
    /// <param name="collisionObject">The fish collider</param>
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.tag != "Bone"){
            return;
        }

        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;

        // Check if fish has already been sorted
        if (FishSortingButton.fishSortingButton.sortedFish.Contains(fish.gameObject))
        {
            return;
        }

        // Get fish state as a string
        string fishState = fish.GetComponent<FactoryFishState>().CurrentState.ToString();

        // add fish to list of sorted fish
        FishSortingButton.fishSortingButton.sortedFish.Add(fish.gameObject);
    
        // check if fish has been given correct tier
        if (fishState == FishSortingButton.fishSortingButton.currentFishTier.ToString())
        {
            GameManager.Instance.PlaySound("correct");
            correctSortedFish++;
        }
        else
        {
            GameManager.Instance.PlaySound("incorrect");
            incorrectSortedFish--;
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

    /// <summary>
    /// Open the door with the given name and close it after a delay
    /// </summary>
    /// <param name="doorName">The name of the door to open</param>
    private void OpenDoor(string doorName)
    {
        GameObject door = GameObject.Find(doorName);
        StartCoroutine(CloseDoorAfterDelay(door));
    }

    /// <summary>
    /// Close the door after a delay
    /// </summary>
    /// <param name="door">The door to close</param>
    private IEnumerator CloseDoorAfterDelay(GameObject door)
    {
        // Delay opening the door 1 second
        yield return new WaitForSeconds(1);
        // Move the door up
        door.transform.Translate(Vector3.back * 0.17f);
        // Keep the door open for 2 seconds
        yield return new WaitForSeconds(2);
        // Move the door down
        door.transform.Translate(Vector3.forward * 0.17f);
    }
}
