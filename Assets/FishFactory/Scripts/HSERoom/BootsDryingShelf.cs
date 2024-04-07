using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsDryingShelf : MonoBehaviour
{
    [Tooltip("Time in seconds before boots despawn")]
    [SerializeField]
    private float despawnTimer = 3f;

    // Once the boots are cleaned the boots dry off, before the player "puts them on"
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<BootsState>().Boots == BootsState.BootsStatus.Clean)
        {
            GameManager.instance.PlaySound("correct");
            StartCoroutine(DryBoots(collider.gameObject));
        }
    }

    private IEnumerator DryBoots(GameObject boots)
    {
        yield return new WaitForSeconds(despawnTimer);
        GameManager.instance.BootsOn = true;
        GameManager.instance.PlaySound("correct");
        Destroy(boots);
    }
}
