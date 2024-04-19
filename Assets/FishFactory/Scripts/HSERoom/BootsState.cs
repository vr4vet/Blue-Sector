using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsState : MonoBehaviour
{
    // ----------------- Public Variables -----------------

    [Tooltip("The possible states of the boots.")]
    public enum BootsStatus
    {
        Dirty,
        SemiClean,
        Clean
    }

    [Tooltip("The current state of the boots.")]
    private BootsStatus boots = BootsStatus.Dirty;
    public BootsStatus Boots
    {
        get { return boots; }
        set { boots = value; }
    }

    // ----------------- Editor Variables -----------------

    [Tooltip("The time it takes for the boots to soak in water, before they can be scrubbed.")]
    [SerializeField]
    private float soakingTime = 3f;

    [Tooltip("The number of scrubs needed to clean the boots.")]
    [SerializeField]
    private int scrubbingLeft = 5;

    [Tooltip(
        "The materials that will be used to change the boots' appearance. Should be 'dirty', 'soaped', 'semisoaped', 'clean', in that order."
    )]
    [SerializeField]
    private List<Material> materials = new List<Material>();

    // ----------------- Functions -----------------

    /// <summary>
    /// Check if the boots are scrubbed with the scrubber. Only checks for "collisions" with the scrubber object.
    /// </summary>
    /// <param name="collision">The scrubber object</param>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Scrubber" && boots == BootsStatus.SemiClean)
        {
            // Could add sound effect here
            scrubbingLeft--;
            if (scrubbingLeft < 0)
            {
                boots = BootsStatus.Clean;
                GameManager.Instance.PlaySound("correct");
                gameObject.GetComponent<MeshRenderer>().material = materials[3]; // Clean
            }
        }
    }

    /// <summary>
    /// Soak the boots in water to prepare them for scrubbing
    /// </summary>
    public void BootWashing()
    {
        if (boots == BootsStatus.Dirty)
        {
            gameObject.GetComponent<MeshRenderer>().material = materials[1]; // Semi-soaped
            StartCoroutine(WashBoots());
        }
    }

    /// <summary>
    /// Coroutine to soak the boots in water before they can be scrubbed
    /// </summary>
    /// <returns>Wait for the boots to soak</returns>
    private IEnumerator WashBoots()
    {
        yield return new WaitForSeconds(soakingTime);
        GameManager.Instance.PlaySound("correct");
        boots = BootsStatus.SemiClean;
        gameObject.GetComponent<MeshRenderer>().material = materials[2]; // Soaped
        // The boots are now ready to be scrubbed under water
    }
}
