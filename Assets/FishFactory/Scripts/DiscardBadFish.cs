using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardBadFish : MonoBehaviour
{
    // counter for the number of fish that are stunned
    [SerializeField]
    [Range(0, 15)]
    private int destroyFishTreshold = 5;
    public int DestroyFishTreshold
    {
        get { return destroyFishTreshold; }
    }
    private int nrFishDiscarded = 0;
    public int NrFishDiscarded
    {
        get { return nrFishDiscarded; }
    }
    private int incorrectlyDiscardedFish = 0;
    public int IncorrectlyDiscardedFish
    {
        get { return incorrectlyDiscardedFish; }
    }
    private int correctlyDiscardedFish = 0;
    public int CorrectlyDiscardedFish
    {
        get { return correctlyDiscardedFish; }
    }

    // list of fish that are currently stunned
    private List<GameObject> discardedFish = new List<GameObject>();

    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.tag != "Destroyable")
            return;
        // get the parent's parent
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;
        // get fish state and check if fish is dead, if fish is dead it's deleted
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        if (!discardedFish.Contains(fish.gameObject))
        {
            discardedFish.Add(fish.gameObject);

            // Check if the fish should have been discarded or not, and give the user feedback
            if (
                fishState.currentState == FactoryFishState.State.BadQuality
                || fishState.currentState == FactoryFishState.State.GuttingFailure
            )
            {
                GameManager.Instance.PlaySound("correct");
                correctlyDiscardedFish++;
            }
            else
            {
                GameManager.Instance.PlaySound("incorrect");
                incorrectlyDiscardedFish--;
            }

            // Delete fish if number of fish in bin is larger than destroyFishTreshold
            if (discardedFish.Count >= destroyFishTreshold)
            {
                GameObject fishDelete = discardedFish[0];
                discardedFish.RemoveAt(0);
                Destroy(fishDelete);
            }
            nrFishDiscarded++;
        }
    }
}
