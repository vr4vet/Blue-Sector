using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SanitizeHands : MonoBehaviour
{
    // Player can put one or both hands into the sanitizer to clean their hands
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Grabber")
        {
            // Player should not be able to put on gloves without sanitizing hands first
            if (
                collider.transform.parent.name == "LeftController"
                && GameManager.instance.LeftHand == GameManager.PlayerHandState.Unsanitized
            )
            {
                GameManager.instance.LeftHand = GameManager.PlayerHandState.Sanitized;
                GameManager.instance.PlaySound("correct");
            }
            else if (GameManager.instance.RightHand == GameManager.PlayerHandState.Unsanitized)
            {
                GameManager.instance.RightHand = GameManager.PlayerHandState.Sanitized;
                GameManager.instance.PlaySound("correct");
            }
        }
    }
}
