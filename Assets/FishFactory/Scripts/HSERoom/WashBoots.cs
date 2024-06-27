using UnityEngine;

public class WashBoots : MonoBehaviour
{
    /// <summary>
    /// When the player collides with the boots, wash them
    /// </summary>
    /// <param name="collider">The player boots collider</param>
    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Boots")
        {
            collider.GetComponent<BootsState>().BootWashing();
        }
    }
}
