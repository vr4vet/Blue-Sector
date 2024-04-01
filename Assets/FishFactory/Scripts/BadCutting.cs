using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadCutting : MonoBehaviour
{
    [SerializeField]
    private FactoryFishState fishState;

    private void OnCollisionEnter(Collision collisionObject)
    {
        if (collisionObject.gameObject.tag != "Knife")
        {
            return;
        }

        Debug.Log("bad with " + collisionObject.gameObject.name);

        fishState.BadCut();
    }
}
