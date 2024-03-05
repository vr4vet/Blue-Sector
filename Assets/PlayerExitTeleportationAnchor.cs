using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExitTeleportationAnchor : MonoBehaviour
{
    public GameObject cylinder;
    public GameObject cylinderGlow;
    // public Collider player;

    public void OnTriggerExit(Collider player)
    {
        cylinder.SetActive(true);
        cylinderGlow.SetActive(true);
    }
}
