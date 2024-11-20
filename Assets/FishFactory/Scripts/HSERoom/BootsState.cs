using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    // ----------------- Editor Variables -----------------

    [Tooltip("The current state of the boots.")]
    private BootsStatus boots = BootsStatus.Dirty;
    public BootsStatus Boots
    {
        get { return boots; }
        set { boots = value; }
    }

    [Tooltip("The time it takes for the boots to soak in water, before they can be scrubbed.")]
    [SerializeField]
    private float soakingTime = 3f;

    [Tooltip("An estimate of scrubbingtime needed to clean the boots.")]
    [SerializeField]
    private int scrubbingLeft = 5;

    [Tooltip(
        "The materials that will be used to change the boots' appearance. Should be 'dirty', 'soaped', 'semisoaped', 'clean', in that order."
    )]
    [SerializeField]
    private List<Material> materials = new List<Material>();

    public UnityEvent OnWash;


    // ----------------- Private Variables -----------------

    private float scrubbingDuration = 0f;

    // ----------------- Functions -----------------

    private void Start()
    {
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Boots"));
    }

    /// <summary>
    /// Check if the boots are scrubbed with the scrubber. Only checks for "collisions" with the scrubber object.
    /// Uses deltatime to manage the time it takes to scrub and clean the boots.
    /// </summary>
    /// <param name="collision">The scrubber object</param>
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Scrubber" && boots == BootsStatus.SemiClean)
        {
            // Track scrubbing duration
            scrubbingDuration += Time.deltaTime;
            // Calculate scrubbing progress based on time
            int scrubbingProgress = Mathf.FloorToInt(
                scrubbingDuration / soakingTime * scrubbingLeft
            );
            scrubbingLeft = Mathf.Max(0, scrubbingLeft - scrubbingProgress);

            if (scrubbingLeft <= 0)
            {
                transform.gameObject.layer = LayerMask.NameToLayer("Grabbable");
                boots = BootsStatus.Clean;
                GameManager.Instance.PlaySound("correct");
                gameObject.GetComponent<MeshRenderer>().material = materials[3]; // Clean
                OnWash.Invoke();
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
            // Semi-soaped
            gameObject.GetComponent<MeshRenderer>().material = materials[1];
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
        // Soaped
        gameObject.GetComponent<MeshRenderer>().material = materials[2];
        // The boots are now ready to be scrubbed under water
    }
}
