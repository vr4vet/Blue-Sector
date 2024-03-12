using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTeleportMarker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Renderer glowRenderer;
    [SerializeField] private Material glowMaterial;
    [SerializeField] private Renderer cylinderRenderer;
    [SerializeField] private Material cylinderMaterial;
    private Material originalMaterial;
    private Material originalGlow;

    void Start()
    {
        originalMaterial = glowRenderer.material;
        originalGlow = cylinderRenderer.material;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("TeleportMarker"))
        {
            glowRenderer.material = glowMaterial;
            cylinderRenderer.material = cylinderMaterial;
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("TeleportMarker"))
        {
            glowRenderer.material = originalGlow;
            cylinderRenderer.material = originalMaterial;
        }
    }
}
