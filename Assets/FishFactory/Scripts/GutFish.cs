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

    void Awake()
    {
        guttedFish = Resources.Load<Material>("Materials/Fish/salmonGutted");
        badFish = Resources.Load<Material>("Materials/Fish/salmonWronglyGutted");
    }

    void OnTriggerEnter(Collider collider)
    {
        // Get the main fish object
        GameObject fish = collider.transform.parent.gameObject.transform.parent.gameObject;
        Debug.Log(fish.tag);
        Debug.Log(fish.name);
        if (fish.tag == "Fish")
        {
            Renderer fishMaterial = fish.transform.GetChild(0).GetComponent<Renderer>();

            // Get fish state and check if fish is alive, if fish is alive it's state is set to stunned
            FactoryFishState fishState = fish.GetComponent<FactoryFishState>();

            if (fishState.currentState == FactoryFishState.State.GuttingSuccess)
            {
                // Set first material to gutted fish
                fishMaterial.material = guttedFish;
                GetComponent<AudioSource>().Play();
            }
            else if (fishState.currentState == FactoryFishState.State.GuttingIncomplete)
            {
                //TODO: play different sound
            }
            else
            {
                // Set material to gutted fish
                fishMaterial.material = badFish;
                GetComponent<AudioSource>().Play();
            }

            // Turn on light
            gutLight.enabled = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        // Get the main fish object
        GameObject fish = collider.transform.parent.gameObject.transform.parent.gameObject;

        if (fish.tag == "Fish")
        {
            // Turn off light
            gutLight.enabled = false;
        }
    }
}
