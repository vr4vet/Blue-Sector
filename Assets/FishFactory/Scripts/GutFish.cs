using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GutFish : MonoBehaviour
{
    [SerializeField]
    private Light gutLight;

    [SerializeField]
    private Material guttedFish;

    [SerializeField]
    private Material badFish;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Destroyable")
        {
            Renderer fishRenderer = collider
                .transform.parent.transform.parent.transform.Find("Stereo textured mesh")
                .GetComponent<Renderer>();

            Material[] fishMaterials = fishRenderer.materials;

            // Get the main fish object
            GameObject fish = collider.transform.parent.gameObject.transform.parent.gameObject;

            // Get fish state and check if fish is alive, if fish is alive it's state is set to stunned
            FactoryFishState fishState = fish.GetComponent<FactoryFishState>();

            if (fishState.currentState == FactoryFishState.State.GuttingSuccess)
            {
                // Set first material to gutted fish
                fishMaterials[0] = guttedFish;
                // Set the updated materials
                fishRenderer.materials = fishMaterials;
                GetComponent<AudioSource>().Play();
            }
            else if (fishState.currentState == FactoryFishState.State.GuttingIncomplete)
            {
                //TODO: play different sound
            }
            else
            {
                // Set first material to gutted fish
                fishMaterials[0] = guttedFish;
                // Set second material to badfish
                fishMaterials[1] = badFish;
                // Set the updated materials
                fishRenderer.materials = fishMaterials;
                GetComponent<AudioSource>().Play();
            }

            // Turn on light
            gutLight.enabled = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Destroyable")
        {
            // Turn off light
            gutLight.enabled = false;
        }
    }
}
