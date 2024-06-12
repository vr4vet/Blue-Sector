using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardKnife : MonoBehaviour
{
    protected KnifeState _knifeState;

    private void OnTriggerEnter(Collider collisionObject)
    {
        GameObject Knife = collisionObject.gameObject;

        if (Knife.tag != "Knife")
        {
            return;
        }

        // for testing purposes where there is no GameManager Instance
        if (GameManager.Instance != null)
        {
            HandleAudioFeedback(Knife.GetComponent<KnifeState>());
            Destroy(Knife);
        }
    }

    /// <summary>
    /// Play audio feedback based on the fish state, and if the fish was correctly discarded
    /// </summary>
    /// <param name="fishState"> The fish state script attached to the fish object </param>
    private void HandleAudioFeedback(KnifeState knifeState)
    {
        if (knifeState._durabilityCount <= 0)
        {
            GameManager.Instance.PlaySound("correct");
        }
        else 
        {
            GameManager.Instance.PlaySound("incorrect");
        }
    }
}
