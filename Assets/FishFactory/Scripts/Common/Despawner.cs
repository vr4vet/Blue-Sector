using UnityEngine;

public class Despawner : MonoBehaviour
{
    /// <summary>
    /// Destroy the fish object when it collides with the despawner
    /// </summary>
    /// <param name="collider">The head bone collider of the fish object</param>
    private void OnTriggerEnter(Collider collider)
    {
        GameObject colliderObject = collider.transform.parent.transform.parent.gameObject;
        if (colliderObject.tag != "Fish")
        {
            return;
        }
        FactoryFishState fishState = colliderObject.GetComponent<FactoryFishState>();
        if (fishState.CurrentState == FactoryFishState.State.ContainsMetal)
        {
            GameManager.Instance.PlaySound("incorrect");
        }

        // Destroy the main fish object
        Destroy(colliderObject);
    }
}
