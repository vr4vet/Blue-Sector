using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportHighlight : MonoBehaviour
{
    [SerializeField] private BNG.PlayerTeleport playerTeleport;
    [SerializeField] private Transform destination;
    [SerializeField] private bool teleportOnHand = true;

    private bool teleportedInside = false;
    public UnityEvent PlayerTeleported;

    public void PlayerInside()
    {
        teleportedInside = true;
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || (teleportOnHand && other.CompareTag("Hand")))
        {
            if (!teleportedInside)
            {
                playerTeleport.TeleportPlayer(destination.position, destination.rotation);
                PlayerTeleported.Invoke();
                teleportedInside = true;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            teleportedInside = false;
    }
}


