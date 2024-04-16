using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stunFish : MonoBehaviour
{
    // counter for the number of fish that are stunned
    public int nrFishStun = 0;

    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.tag != "Destroyable")
        {
            return;
        }
        // get the parent's parent
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;

        // get fish state and check if fish is alive, if fish is alive it's state is set to stunned
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        if (fishState.currentState == FactoryFishState.State.Alive)
        {
            // commented out as animation is not working correctly and the component has been disabled
            // fish.GetComponent<Animator>().enabled = false;

            fishState.currentState = FactoryFishState.State.Stunned;
            GameManager.instance.PlaySound("correct");
            nrFishStun++;
        }
        else
        {
            GameManager.instance.PlaySound("incorrect");
        }
    }
}
