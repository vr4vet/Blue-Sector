using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishSortingTrigger : MonoBehaviour
{
    // ------------------ Editor Variables ------------------

    [Tooltip("Door to open")]
    [SerializeField]
    private GameObject _door;

    [Tooltip("The QA machine tier manager")]
    [SerializeField]
    public FishSortingButton _tierManager;

    [SerializeField]
    public UnityEvent OnTier1;
    [SerializeField]
    public UnityEvent OnTier2;
    [SerializeField]
    public UnityEvent OnTier3;
    [SerializeField]
    public UnityEvent OnXSortedFish;

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
            _tierManager.correctlySortedFish++;
            GameManager.Instance.PlaySound("correct");
            if (OnTier1 != null)
            {
                switch (fishState)
                {
                    case "Tier1":
                        OnTier1.Invoke();
                        break;
                    case "Tier2":
                        OnTier2.Invoke();
                        break;
                    case "Tier3":
                        OnTier3.Invoke();
                        break;
                }
                if (_tierManager.correctlySortedFish > 25)
                {
                    OnXSortedFish.Invoke();
                }
            }
        }
        else
        {
            GameManager.Instance.PlaySound("incorrect");
        }
        
        if (_door)
        {
            StartCoroutine(CloseDoorAfterDelay(_door));
        }
        else
        {
            Debug.Log("No door assigned");
        }
    }

    /// <summary>
    /// Close the door after a delay
    /// </summary>
    /// <param name="door">The door to close</param>
    private IEnumerator CloseDoorAfterDelay(GameObject door)
    {
        
            yield return new WaitForSeconds(1);
            door.transform.localEulerAngles = new Vector3(-50, 0, 0);
            yield return new WaitForSeconds(2);
            door.transform.localEulerAngles = Vector3.zero;
    }
}
