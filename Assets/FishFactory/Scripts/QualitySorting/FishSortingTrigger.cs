using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSortingTrigger : MonoBehaviour
{
    // ------------------ Editor Variables ------------------

    [Tooltip("Door to open")]
    [SerializeField]
    private GameObject _door;

    [Tooltip("The QA machine tier manager")]
    [SerializeField]
    public FishSortingButton _tierManager;

    /// <summary>
    /// When the fish enters the trigger, check if the fish has been sorted and if it has the correct tier
    /// </summary>
    /// <param name="collisionObject">The fish collider</param>
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.name != "Head")
        {
            return;
        }

        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;
        List<GameObject> sortedFish = _tierManager.SortedFish;

        if (sortedFish.Contains(fish.gameObject))
        {
            return;
        }
        sortedFish.Add(fish.gameObject);
        string fishState = fish.GetComponent<FactoryFishState>().fishTier.ToString();
        if (fishState == _tierManager.CurrentTier.ToString())
        {
            GameManager.Instance.PlaySound("correct");
        }
        else
        {
            GameManager.Instance.PlaySound("incorrect");
        }

        StartCoroutine(CloseDoorAfterDelay(_door));
    }

    /// <summary>
    /// Close the door after a delay
    /// </summary>
    /// <param name="door">The door to close</param>
    private IEnumerator CloseDoorAfterDelay(GameObject door)
    {
        if (_door)
        {
            yield return new WaitForSeconds(1);
            door.transform.Translate(Vector3.back * 0.177f);
            yield return new WaitForSeconds(2);
            door.transform.Translate(Vector3.forward * 0.177f);
        }
        else
        {
            Debug.Log("No door assigned");
        }
    }
}
