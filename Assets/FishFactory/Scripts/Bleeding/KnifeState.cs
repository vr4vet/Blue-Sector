using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeState : MonoBehaviour
{
    // ---------------- Editor Variables ----------------

    // Reference to the regular knife GameObject
    [Header("Knife Object settings")]
    [Tooltip("The gameobject matching the regular fish knife blade")]
    [SerializeField]
    private GameObject _regularKnife;

    // Reference to the chipped knife GameObject
    [Tooltip("The gameobject matching the chipped fish knife blade")]
    [SerializeField]
    private GameObject _chippedKnife;

    [Header("Knife Durability Settings")]
    [Tooltip("Enable random count to chip knife instead of a set count")]
    [SerializeField]
    private bool _isChippingRandom;

    [Tooltip("Number of cuts before a knife gets chipped")]
    [SerializeField]
    private int _durabilityCount = 5;

    // ---------------- Unity Functions ----------------

    void Start()
    {
        if (_isChippingRandom)
        {
            _durabilityCount = RandomizeDurability();
        }
        UpdateKnifeVisibility();
    }

    // ---------------- Public Functions ----------------

    // Method to decrement durabilityCount
    public void DecrementDurabilityCount()
    {
        if (_durabilityCount > 0)
        {
            _durabilityCount--;
        }
        if (_durabilityCount == 0)
        {
            UpdateKnifeVisibility();
        }
    }

    // ---------------- Private Functions ----------------

    private int RandomizeDurability()
    {
        // Range for the amount of cuts to chip the knife
        return Random.Range(6, 15);
    }

    private void UpdateKnifeVisibility()
    {
        _regularKnife.SetActive(_durabilityCount > 0);
        _chippedKnife.SetActive(_durabilityCount == 0);
    }
}
