using UnityEngine;

public class CutFishGill : MonoBehaviour
{
    [Tooltip("The fish state script attached to the main fish object")]
    [SerializeField]
    private FactoryFishState fishState;

    [SerializeField]
    private KnifeState knifeState;

    void Start()
    {
        // Check if the knife GameObject was found
        if (knifeState != null)
        {
            // Get the KnifeState component if the knife GameObject was found
            knifeState = GetComponent<KnifeState>();
        }
        else
        {
            // Log a warning if the knife GameObject was not found
            Debug.LogWarning("Knife GameObject not found in the scene.");
        }
    }



    /// <summary>
    /// When the knife collides with the fish gill, cut the gill
    /// </summary>
    /// <param name="collider"> The knife collider </param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Knife")
        {
            return;
        }

        fishState.CutFishGills();
        knifeState.DecrementDurabilityCount();
    }
}
