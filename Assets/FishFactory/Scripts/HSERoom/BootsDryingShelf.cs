using System.Collections;
using UnityEngine;

public class BootsDryingShelf : MonoBehaviour
{
    [Tooltip("Time in seconds before boots despawn")]
    [SerializeField]
    private float despawnTimer = 3f;

    /// <summary>
    /// If the boots are clean, dry them off, before the player "puts them on"
    /// </summary>
    /// <param name="collider">The collider of the boots</param>
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<BootsState>().Boots == BootsState.BootsStatus.Clean)
        {
            GameManager.Instance.PlaySound("correct");
            StartCoroutine(DryBoots(collider.gameObject));
        }
    }

    /// <summary>
    /// Coroutine to change boot status to dry and despawn them
    /// </summary>
    /// <param name="boots">The boots to dry</param>
    private IEnumerator DryBoots(GameObject boots)
    {
        yield return new WaitForSeconds(despawnTimer);
        GameManager.Instance.BootsOn = true;
        GameManager.Instance.PlaySound("correct");
        Destroy(boots);
    }
}
