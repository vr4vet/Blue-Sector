using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GutFish : MonoBehaviour
{
    [SerializeField]
    private Light gutLight;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Destroyable")
        {
            // TODO: Change texture to gutted fish
            // collider.gameObject.GetComponent<Renderer>().material.mainTexture =
            //     Resources.Load<Texture>("GuttedFish");
            // Play sound
            GameManager.Instance.PlaySound("door");

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
