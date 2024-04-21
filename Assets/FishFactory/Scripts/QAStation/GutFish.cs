using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GutFish : MonoBehaviour
{
    [SerializeField]
    private Light gutLight;

    [SerializeField]
    private Texture guttedFish;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Destroyable")
        {
            // TODO: Change texture to gutted fish
            collider
                .transform.parent.transform.parent.transform.Find("Stereo textured mesh")
                .GetComponent<Renderer>()
                .material.mainTexture = guttedFish;
            // Play sound
            GetComponent<AudioSource>()
                .Play();

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
