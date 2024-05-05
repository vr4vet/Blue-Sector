using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardBadFish : MonoBehaviour
{
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

    private List<GameObject> discardedFish = new List<GameObject>();

    private void OnTriggerEnter(Collider collisionObject)
    {
        GameObject fish = collisionObject.transform.parent?.parent?.gameObject;

        if (fish == null || fish.tag != "Fish")
        {
            return;
        }

        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        if (!discardedFish.Contains(fish))
        {
            discardedFish.Add(fish);

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