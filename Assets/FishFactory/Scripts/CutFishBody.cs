using UnityEngine;

public class CutFishBody : MonoBehaviour
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
    /// When the knife collides with the fish body, the player makes a wrong cut
    /// </summary>
    /// <param name="collision"> The knife object </param>
    private void OnCollisionEnter(Collision collisionObject)
    {
        if (collisionObject.gameObject.tag != "Knife")
        {
            return;
        }

        fishState.CutFishBody();
        knifeState.DecrementDurabilityCount();
    }
}
