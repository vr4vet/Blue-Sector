using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WashBoots : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Boots")
        {
            collider.GetComponent<BootsState>().BootWashing();
        }
    }
}
