using UnityEngine;

public class StunFish : MonoBehaviour
{
    private void OnTriggerEnter(Collider collisionObject)
    {
        // Get the main fish object
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;

        if (fish.tag != "Fish")
        {
            return;
        }

        GetComponent<AudioSource>().Play();

        // Get fish state and check if fish is alive, if alive the fish state is set to stunned
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        if (fishState.CurrentState == FactoryFishState.State.Alive)
        {
            fishState.CurrentState = FactoryFishState.State.Stunned;
            GameManager.Instance.PlaySound("correct");
        }
        else
        {
            GameManager.Instance.PlaySound("incorrect");
        }
    }
}
