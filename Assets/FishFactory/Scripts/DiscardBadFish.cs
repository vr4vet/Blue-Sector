using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardBadFish : MonoBehaviour
{
    // counter for the number of fish discarded
    [SerializeField]
    [Range(0, 15)]
    private int destroyFishTreshold = 5;
    public int DestroyFishTreshold => destroyFishTreshold;

    private int nrFishDiscarded = 0;
    public int NrFishDiscarded => nrFishDiscarded;

    private int incorrectlyDiscardedFish = 0;
    public int IncorrectlyDiscardedFish => incorrectlyDiscardedFish;

    private int correctlyDiscardedFish = 0;
    public int CorrectlyDiscardedFish => correctlyDiscardedFish;

    // list of fish that have been discarded
    private List<GameObject> discardedFish = new List<GameObject>();

    private void OnTriggerEnter(Collider collisionObject)
    {
        // get the parent's parent
        GameObject fish = collisionObject.transform.parent?.parent?.gameObject;

        if (fish == null || !fish.CompareTag("Fish"))
        {
            return;
        }

        // get the fish state script attached to the fish object to check if the fish is of bad quality
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        if (!discardedFish.Contains(fish))
        {
            discardedFish.Add(fish);

            // check if the fish is of bad quality or if the gutting failed and give user feedback
            if (fishState.CurrentState == FactoryFishState.State.BadQuality ||
                fishState.CurrentState == FactoryFishState.State.GuttingFailure)
            {
                GameManager.Instance?.PlaySound("correct");
                correctlyDiscardedFish++;
            }
            else
            {
                GameManager.Instance?.PlaySound("incorrect");
                incorrectlyDiscardedFish--;
            }

            // destroy the fish if the number of discarded fish exceeds the threshold
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