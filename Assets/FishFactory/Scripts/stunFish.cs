using UnityEngine;

public class StunFish : MonoBehaviour
{
    // counter for the number of fish that are stunned
    public int nrFishStun = 0;

    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.tag != "Destroyable")
        {
            return;
        }
        // Get the main fish object
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;

        // Get fish state and check if fish is alive, if fish is alive it's state is set to stunned
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        if (fishState.currentState == FactoryFishState.State.Alive)
        {
            // FIXME: commented out as animation is not working correctly and the component has been disabled
            // fish.GetComponent<Animator>().enabled = false;

            fishState.currentState = FactoryFishState.State.Stunned;
            GameManager.Instance.PlaySound("correct");
            nrFishStun++;
        }
        else
        {
            GameManager.Instance.PlaySound("incorrect");
        }
    }
}
