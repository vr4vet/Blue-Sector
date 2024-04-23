using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportHighlight : MonoBehaviour
{
    [SerializeField] private GameObject cylinder;
    [SerializeField] private GameObject cylinderGlow;
    [SerializeField] private Material hitGlow;
    [SerializeField] private Material hitCylinder;
    [SerializeField] private BNG.PlayerTeleport playerTeleport;


    private Renderer glowRenderer;
    private Renderer cylinderRenderer;


    private Material originalGlow;
    private Material originalCylinder;
    private BoxCollider boxCollider;
    private Vector3 originalSize;

    private void Start()
    {
        glowRenderer = cylinderGlow.GetComponent<Renderer>();
        cylinderRenderer = cylinder.GetComponent<Renderer>();
        boxCollider = gameObject.GetComponent<BoxCollider>();
        originalSize = boxCollider.size;
        originalGlow = glowRenderer.sharedMaterial;
        originalCylinder = cylinderRenderer.sharedMaterial;
        playerTeleport.AnchorHitEvent.AddListener(SetTeleportHighlight);
    }

    public void SetTeleportHighlight(GameObject hitAnchor)
    {
        if (hitAnchor == this.gameObject)
        {
            glowRenderer.material = hitGlow;
            cylinderRenderer.material = hitCylinder;

        }
        else
        {
            glowRenderer.material = originalGlow;
            cylinderRenderer.material = originalCylinder;

        }
    }


    public void PlayerInside()
    {
        //if (currentSubtask && currentSubtask.SubtaskName != subTask)
        // {
        //    manager.UpdateCurrentSubtask(manager.GetSubtask(subTask));
        // }

        cylinder.SetActive(false);
        cylinderGlow.SetActive(false);

        boxCollider.size = new Vector3(1f, 3f, 1f);
        // else
        // {
        //     cylinder.SetActive(true);
        //     cylinderGlow.SetActive(true);

        //     boxCollider.size = originalSize;
        // }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cylinder.SetActive(true);
            cylinderGlow.SetActive(true);
            boxCollider.size = originalSize;
        }
    }



}


