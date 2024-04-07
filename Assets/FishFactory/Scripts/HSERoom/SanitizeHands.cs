using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanitizeHands : MonoBehaviour
{
    // private GameManager GameManager.instance = GameManager.instance;

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Player hand sanitized with " + gameObject.name);

        if (collider.gameObject.name == "Grabber")
        {
            Debug.Log("Player sanitized inside trigger with " + gameObject.name);

            // Player should not be able to put on gloves without sanitizing hands first

            if (
                collider.transform.parent.name == "LeftController"
                && GameManager.instance.LeftHand == GameManager.PlayerLeftHand.Unsanitized
            )
            {
                GameManager.instance.LeftHand = GameManager.PlayerLeftHand.Sanitized;
                GameManager.instance.PlaySound("correct");
            }
            else if (GameManager.instance.RightHand == GameManager.PlayerRightHand.Unsanitized)
            {
                GameManager.instance.RightHand = GameManager.PlayerRightHand.Sanitized;
                GameManager.instance.PlaySound("correct");
            }
        }
    }
}
