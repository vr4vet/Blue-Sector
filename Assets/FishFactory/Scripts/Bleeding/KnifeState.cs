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
    public int _durabilityCount = 5;

    // ---------------- Unity Functions ----------------

    void Start()
    {
        if (_isChippingRandom)
        {
            _durabilityCount = RandomizeDurability();
        }
        UpdateKnifeVisibility();

        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("FishKnife"));
    }

    // ---------------- Public Functions ----------------

    // Method to decrement durabilityCount
    public bool DecrementDurabilityCount()
    {
        if (_durabilityCount > 0)
        {
            _durabilityCount--;
        }
        if (_durabilityCount == 0)
        {
            UpdateKnifeVisibility();
            _durabilityCount--;
            return true;
        }
        return false;
    }

    // ---------------- Private Functions ----------------

    public int RandomizeDurability()
    {
        // Range for the amount of cuts to chip the knife
        return Random.Range(6, 15);
    }

    private void UpdateKnifeVisibility()
    {
        _regularKnife.SetActive(_durabilityCount > 0);
        _chippedKnife.SetActive(_durabilityCount <= 0);
    }
}
