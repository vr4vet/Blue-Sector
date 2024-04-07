using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WashBoots : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Boots")
        {
            Debug.Log("Player boots washed with " + gameObject.name);

            collider.GetComponent<BootsState>().BootWashing();
        }
    }
}
