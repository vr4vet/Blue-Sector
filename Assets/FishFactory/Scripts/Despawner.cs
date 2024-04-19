using UnityEngine;

public class Despawner : MonoBehaviour
{
    /// <summary>
    /// Destroy the fish object when it collides with the despawner
    /// </summary>
    /// <param name="collider">The head bone collider of the fish object</param>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag != "Destroyable")
        {
            return;
        }

        // Destroy the main fish object
        Destroy(collider.transform.parent.transform.parent.gameObject);
    }
}
