using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TeleportHighlight : MonoBehaviour
{
    [SerializeField] private GameObject cylinder;
    [SerializeField] private GameObject cylinderGlow;
    [SerializeField] private Material hitGlow;
    [SerializeField] private Material hitCylinder;
    [SerializeField] private BNG.PlayerTeleport playerTeleport;
    [SerializeField] private Transform destination;
    [SerializeField] private bool teleportOnHand = true;

    private Renderer glowRenderer;
    private Renderer cylinderRenderer;


    private Material originalGlow;
    private Material originalCylinder;
    private BoxCollider boxCollider;
    private Vector3 originalSize;
    private bool teleportedInside = false;
    public UnityEvent PlayerTeleported;
    private void Start()
    {
        glowRenderer = cylinderGlow.GetComponent<Renderer>();
        cylinderRenderer = cylinder.GetComponent<Renderer>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        originalSize = boxCollider.size;
        originalGlow = glowRenderer.sharedMaterial;
        originalCylinder = cylinderRenderer.sharedMaterial;
        //playerTeleport.AnchorHitEvent.AddListener(SetTeleportHighlight);
    }

    // The method below requires modifications to BNG 

    // public void SetTeleportHighlight(GameObject hitAnchor)
    // {
    //     if (hitAnchor == this.gameObject)
    //     {
    //         glowRenderer.material = hitGlow;
    //         cylinderRenderer.material = hitCylinder;

    //     }
    //     else
    //     {
    //         glowRenderer.material = originalGlow;
    //         cylinderRenderer.material = originalCylinder;

    //     }
    // }

    public void PlayerInside()
    {
        teleportedInside = true;
    }


    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || (teleportOnHand && other.CompareTag("Hand")))
        {
            // cylinder.SetActive(false);
            // cylinderGlow.SetActive(false);
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
        {
            // cylinder.SetActive(true);
            // cylinderGlow.SetActive(true);
            teleportedInside = false;
            // boxCollider.size = originalSize;
        }
    }



}


