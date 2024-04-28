using UnityEngine;

public class StunFish : MonoBehaviour
{
    // counter for the number of fish that are stunned
    public int nrFishStun = 0;

    private void OnTriggerEnter(Collider collisionObject)
    {
        // Get the main fish object
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;

        if (fish.tag != "Fish")
        {
            return;
        }

        // Get fish state and check if fish is alive, if fish is alive it's state is set to stunned
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        if (fishState.CurrentState == FactoryFishState.State.Alive)
        {
            fishState.CurrentState = FactoryFishState.State.Stunned;
            GameManager.Instance.PlaySound("correct");
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            nrFishStun++;
        }
        else
        {
            GameManager.Instance.PlaySound("incorrect");
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }
    }
}
