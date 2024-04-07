using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsDryingShelf : MonoBehaviour
{
    [Tooltip("Time in seconds before boots despawn")]
    [SerializeField]
    private float despawnTimer = 3f;

    void OnTriggerEnter(Collider collider)
    {
        if (
            collider.gameObject.name == "Boots"
            && collider.gameObject.GetComponent<BootsState>().Boots == BootsState.BootsStatus.Clean
        )
        {
            Debug.Log("Player boots dried with " + gameObject.name);
            GameManager.instance.PlaySound("correct");
            StartCoroutine(DryBoots(collider.gameObject));
        }
    }

    private IEnumerator DryBoots(GameObject boots)
    {
        yield return new WaitForSeconds(despawnTimer);
        GameManager.instance.PlaySound("taskComplete");
        Destroy(boots);
    }
}
