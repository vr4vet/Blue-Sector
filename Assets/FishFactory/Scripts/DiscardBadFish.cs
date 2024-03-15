using System;
using System.Collections;
using System.Collections.Generic;
using POpusCodec.Enums;
using UnityEngine;

public class DiscardBadFish : MonoBehaviour
{
    // counter for the number of fish that are stunned
    [SerializeField]
    [Range(0, 15)]
    private int DeleteNewFishTreshold = 5;
    private int nrFishDiscarded = 0;
    public int NrFishDiscarded{ get {return nrFishDiscarded;}}
    
    // list of fish that are currently stunned
    private List<GameObject> discardedFish = new List<GameObject>();
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.tag != "Destroyable") return; 
        // get the parent's parent
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;
        // get fish state and check if fish is dead, if fish is dead it's deleted
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        if (!discardedFish.Contains(fish.gameObject)) 
        {
            discardedFish.Add(fish.gameObject);
            if (discardedFish.Count >= DeleteNewFishTreshold){
                GameObject fishDelete = discardedFish[0];
                discardedFish.RemoveAt(0);
                Destroy(fishDelete);
            }
            nrFishDiscarded++;
        }
    }
}
