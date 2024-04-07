using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BootsState : MonoBehaviour
{
    public enum BootsStatus
    {
        Dirty,
        SemiClean,
        Clean
    }

    private BootsStatus boots = BootsStatus.Dirty;
    public BootsStatus Boots
    {
        get { return boots; }
        set { boots = value; }
    }

    [Tooltip("The time it takes for the boots to soak in water, before they can be scrubbed.")]
    [SerializeField]
    private float soakingTime = 3f;

    [SerializeField]
    private int scrubbingLeft = 5;

    [Tooltip(
        "The materials that will be used to change the boots' appearance. Should be 'dirty', 'soaped', 'semisoaped', 'clean', in that order."
    )]
    [SerializeField]
    private List<Material> materials = new List<Material>();

    public void BootWashing()
    {
        if (boots == BootsStatus.Clean || boots == BootsStatus.SemiClean)
        {
            return;
        }
        gameObject.GetComponent<MeshRenderer>().material = materials[1];
        StartCoroutine(WashBoots());
    }

    private IEnumerator WashBoots()
    {
        yield return new WaitForSeconds(soakingTime);
        GameManager.instance.PlaySound("correct");
        boots = BootsStatus.SemiClean;
        gameObject.GetComponent<MeshRenderer>().material = materials[2];
        //the boots are now ready to be scrubbed under water
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Scrubber" && boots == BootsStatus.SemiClean)
        {
            Debug.Log("Player scrubbed boots");
            scrubbingLeft--;
            if (scrubbingLeft < 0)
            {
                boots = BootsStatus.Clean;
                GameManager.instance.PlaySound("correct");
                gameObject.GetComponent<MeshRenderer>().material = materials[3];
            }
        }
    }
}
