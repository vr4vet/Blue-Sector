using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildOfBoat : MonoBehaviour {

    public GameObject speedBoat;

	// Sets the player as child to the boat to prevent it from driving away from the player
    
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "BoatTrigger")
        {
            gameObject.transform.root.SetParent(speedBoat.transform, false);
        }
    }
}
