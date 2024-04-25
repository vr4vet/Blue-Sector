using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuttingFishSorting : MonoBehaviour
{
    [SerializeField]
    [Tooltip(
        "If toggled, the trigger will give correct if fish has state GuttingSuccess. If not it will give correct for GuttingIncomplete state"
    )]
    private bool successfullGuttingCheck;

    // ------------------ Private Variables ------------------

    // Counts the amount correctly and incorrectly sorted fish
    private int successfullySortedFish;
    private int incorrectlySortedFish;

    // ------------------ Unity Functions ------------------

    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.tag != "Destroyable")
        {
            return;
        }
        // Get the main fish object
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;
        // Get fish state and check if fish is alive, if fish is alive it's state is set to stunned
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();

        if (successfullGuttingCheck)
        {
            if (fishState.CurrentState == FactoryFishState.State.GuttingSuccess)
            {
                GameManager.Instance.PlaySound("correct");
                successfullySortedFish++;
                return;
            }
        }
        else
        {
            if (fishState.CurrentState == FactoryFishState.State.GuttingIncomplete)
            {
                GameManager.Instance.PlaySound("correct");
                successfullySortedFish++;
                return;
            }
        }
        GameManager.Instance.PlaySound("incorrect");
        incorrectlySortedFish++;
    }
}
