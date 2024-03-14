using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportHighlight : MonoBehaviour
{
    [SerializeField] private Renderer glowRenderer;
    [SerializeField] private Renderer cylinderRenderer;
    [SerializeField] private Material hitGlow;
    [SerializeField] private Material hitCylinder;
    [SerializeField] private BNG.PlayerTeleport playerTeleport;

    private Material originalGlow;
    private Material originalCylinder;

    private void Start()
    {

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

}


