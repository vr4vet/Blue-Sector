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

            if (
                collider.transform.parent.name == "LeftController"
                && (GameManager.instance.RightHand != GameManager.PlayerRightHand.Unsanitized)
            )
            {
                if (name == "SteelGlove")
                {
                    GameManager.instance.LeftHand = GameManager.PlayerLeftHand.SteelGlove;
                }
                else
                {
                    GameManager.instance.LeftHand = GameManager.PlayerLeftHand.BlueGlove;
                }
                GameManager.instance.PlaySound("correct");
            }
            else if (GameManager.instance.RightHand != GameManager.PlayerRightHand.Unsanitized)
            {
                if (name == "SteelGlove")
                {
                    GameManager.instance.RightHand = GameManager.PlayerRightHand.SteelGlove;
                }
                else
                {
                    GameManager.instance.RightHand = GameManager.PlayerRightHand.BlueGlove;
                }

                GameManager.instance.PlaySound("correct");
            }
            else
            {
                GameManager.instance.PlaySound("incorrect");
            }
        }
    }
}
