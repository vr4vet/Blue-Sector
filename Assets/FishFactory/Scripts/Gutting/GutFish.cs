using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GutFish : MonoBehaviour
{
    // ---------------- Editor Variables ----------------

    [Tooltip("The light that turns on when the fish is gutted")]
    [SerializeField]
    private Light _gutLight;

    [Header("Fishtexture settings")]
    [Tooltip("The material that is applied to the fish when it is gutted correctly")]
    [SerializeField]
    private Material _correctlyGuttedFish;

    [Tooltip("The material that is applied to the fish when it is gutted incorrectly")]
    [SerializeField]
    private Material _incorrectlyGuttedFish;

    // ---------------- Unity Functions ----------------

    void Awake()
    {
        _correctlyGuttedFish = Resources.Load<Material>("Materials/Fish/salmonGutted");
        _incorrectlyGuttedFish = Resources.Load<Material>("Materials/Fish/salmonWronglyGutted");
    }

    // ---------------- Private Functions ----------------

    /// <summary>
    /// When the fish enters the trigger, gut the fish.
    /// </summary>
    /// <param name="collider">The head bone collider of the fish object</param>
    private void OnTriggerEnter(Collider collider)
    {
        GameObject fish = collider.transform.parent.gameObject.transform.parent.gameObject;
        if (fish.tag != "Fish")
        {
            return;
        }

        Renderer fishMaterial = fish.transform.GetChild(0).GetComponent<Renderer>();
        handleGutting(fish, fishMaterial);
        _gutLight.enabled = true;
    }

    /// <summary>
    /// Handle the gutting of the fish.
    /// If the fish is gutted correctly, apply the gutted fish material.
    /// If the fish is gutted incorrectly, apply the bad fish material.
    /// </summary>
    private void handleGutting(GameObject fish, Renderer fishMaterial)
    {
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        switch (fishState.guttingState)
        {
            case FactoryFishState.GuttingState.GuttingSuccess:
                fishMaterial.material = _correctlyGuttedFish;
                break;
            case FactoryFishState.GuttingState.GuttingFailure:
                fishMaterial.material = _incorrectlyGuttedFish;
                break;
        }
        GetComponent<AudioSource>().Play();
    }

    /// <summary>
    /// When the fish leaves the trigger, turn off the gut light
    /// </summary>
    private void OnTriggerExit(Collider collider)
    {
        GameObject fish = collider.transform.parent.gameObject.transform.parent.gameObject;

        if (fish.tag == "Fish")
        {
            _gutLight.enabled = false;
        }
    }
}
