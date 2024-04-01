using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GillCutting : MonoBehaviour
{
    [SerializeField]
    private FactoryFishState fishState;

    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.tag != "Knife")
        {
            return;
        }

        Debug.Log("gill with " + collisionObject.name);

        fishState.CutGills();
    }
}
