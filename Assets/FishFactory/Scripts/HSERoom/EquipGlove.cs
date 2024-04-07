using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipGlove : MonoBehaviour
{
    // Player should be able to change the glove they are wearing on either hand
    // The player needs to wear a steel glove and a blue glove on either hand to progress to the factory
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Grabber")
        {
            GameManager.PlayerHandState gloveType =
                name == "SteelGlove"
                    ? GameManager.PlayerHandState.SteelGlove
                    : GameManager.PlayerHandState.BlueGlove;

            if (
                collider.transform.parent.name == "LeftController"
                && GameManager.instance.RightHand != GameManager.PlayerHandState.Unsanitized
            )
            {
                GameManager.instance.LeftHand = gloveType;
                GameManager.instance.PlaySound("correct");
            }
            else if (GameManager.instance.RightHand != GameManager.PlayerHandState.Unsanitized)
            {
                GameManager.instance.RightHand = gloveType;
                GameManager.instance.PlaySound("correct");
            }
            else
            {
                GameManager.instance.PlaySound("incorrect");
            }
        }
    }
}
