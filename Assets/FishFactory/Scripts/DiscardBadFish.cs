using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DiscardBadFish : MonoBehaviour
{
    [Tooltip("The number of fish that need to be in the bin before they are destroyed")]
    [SerializeField]
    [Range(0, 15)]
    private int _destroyFishTreshold = 5;
    public int DestroyFishTreshold
    {
        get { return _destroyFishTreshold; }
    }

    [Tooltip("List of fish that are in the bin")]
    private List<GameObject> _discardedFish = new List<GameObject>();
    public int DiscardedFishCount
    {
        get { return _discardedFish.Count; }
    }

    public UnityEvent OnDiscard;
    public UnityEvent OnDiscardAfterCount;

    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.transform.parent == null || collisionObject.transform.parent.parent == null)
            return;
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;

        if (fish.tag != "Fish")
        {
            return;
        }

        if (!_discardedFish.Contains(fish.gameObject))
        {
            _discardedFish.Add(fish.gameObject);
            OnDiscard.Invoke();
            if (_discardedFish.Count == 10)
            {
                OnDiscardAfterCount.Invoke();
            }
            // for testing purposes where there is no GameManager Instance
            if (GameManager.Instance != null)
                HandleAudioFeedback(fish.GetComponent<FactoryFishState>());
            if (_discardedFish.Count >= _destroyFishTreshold)
            {
                DestroyOldestFish();
            }
        }
    }

    /// <summary>
    /// Play audio feedback based on the fish state, and if the fish was correctly discarded
    /// </summary>
    /// <param name="fishState"> The fish state script attached to the fish object </param>
    private void HandleAudioFeedback(FactoryFishState fishState)
    {
        if (
            fishState.fishTier == FactoryFishState.Tier.BadQuality
            || fishState.guttingState == FactoryFishState.GuttingState.GuttingFailure
            || fishState.ContainsMetal == true
        )
        {
            GameManager.Instance.PlaySound("correct");
        }
        else
        {
            GameManager.Instance.PlaySound("incorrect");
        }
    }

    /// <summary>
    /// Destroy the oldest fish in the bin
    /// </summary>
    private void DestroyOldestFish()
    {
        GameObject fishDelete = _discardedFish[0];
        _discardedFish.RemoveAt(0);
        Destroy(fishDelete);
    }
}
