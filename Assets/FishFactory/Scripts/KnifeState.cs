using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeState : MonoBehaviour
{
    [SerializeField]
    private GameObject regularKnife; // Reference to the regular knife GameObject

    [SerializeField]
    private GameObject chippedKnife; // Reference to the chipped knife GameObject

    [SerializeField]
    [Tooltip("Enable random count to chip knife instead of a set count")]
    private bool isChippingRandom;

    [SerializeField]
    [Tooltip("Number of cuts before a knife gets chipped")]
    private int durabilityCount = 5;

    void Start()
    {
        if (isChippingRandom)
        {
            durabilityCount = RandomizeDurability();
            UpdateKnifeVisibility();
        }
    }

    private int RandomizeDurability()
    {
        // range for the amounts cuts to chip the knife
        return Random.Range(6, 15);
    }

    // Method to decrement durabilityCount
    public void DecrementDurabilityCount()
    {
        if (durabilityCount > 0)
        {
            durabilityCount--;
        }
        if (durabilityCount == 0)
        {
            UpdateKnifeVisibility();
        }
    }

    private void UpdateKnifeVisibility()
    {
        regularKnife.SetActive(durabilityCount > 0);
        chippedKnife.SetActive(durabilityCount == 0);
    }
}
