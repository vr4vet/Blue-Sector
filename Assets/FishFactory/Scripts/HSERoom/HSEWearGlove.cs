using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HSEWearGlove : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Player hand trigger with " + gameObject.name);

        if (collider.gameObject.name == "Grabber")
        {
            Debug.Log("Player hand trigger with " + gameObject.name);

            if (collider.transform.parent.name == "LeftController")
            {
                if (name == "SteelGlove")
                {
                    GameManager.instance.LeftGlove = GameManager.LeftGloveEquipped.Steel;
                }
                else
                {
                    GameManager.instance.LeftGlove = GameManager.LeftGloveEquipped.True;
                }
            }
            else
            {
                if (name == "SteelGlove")
                {
                    GameManager.instance.RightGlove = GameManager.RightGloveEquipped.Steel;
                }
                else
                {
                    GameManager.instance.RightGlove = GameManager.RightGloveEquipped.True;
                }
            }
        }
    }
}
