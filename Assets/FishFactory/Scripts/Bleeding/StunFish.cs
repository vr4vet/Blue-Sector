using UnityEngine;
using UnityEngine.Events;

public class StunFish : MonoBehaviour
{
    public UnityEvent OnStun;
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.transform.parent == null || collisionObject.transform.parent.parent == null)
            return;

        // Get the main fish object
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;

        if (fish.tag != "Fish")
        {
            return;
        }

        GetComponent<AudioSource>().Play();

        // Get fish state and check if fish is alive, if alive the fish state is set to stunned
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        if (fishState.Stunned == false)
        {
            fishState.Stunned = true;
            GameManager.Instance.PlaySound("correct");
            OnStun.Invoke();
        }
        else
        {
            GameManager.Instance.PlaySound("incorrect");
        }
    }
}
