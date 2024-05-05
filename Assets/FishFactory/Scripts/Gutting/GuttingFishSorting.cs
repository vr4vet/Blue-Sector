using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuttingFishSorting : MonoBehaviour
{
    [SerializeField]
    [Tooltip(
        "If toggled, the trigger will give correct if fish has state GuttingSuccess. If not it will give correct for GuttingIncomplete state"
    )]
    private bool _successOnGuttingSuccess = true;

    // ------------------ Unity Functions ------------------

    private void OnTriggerEnter(Collider collisionObject)
    {

        // Get the main fish object
        GameObject fish = collisionObject.transform.parent.gameObject.transform.parent.gameObject;
        
        if (fish.tag != "Fish"){
            return;
        }
        
        if (_successOnGuttingSuccess)
        {
            checkFishState(FactoryFishState.State.GuttingSuccess, fish);
        }
        else
        {
            checkFishState(FactoryFishState.State.GuttingIncomplete, fish);
        }

   }

   private void checkFishState(FactoryFishState.State successCondition, GameObject fish)
   {
        FactoryFishState fishState = fish.GetComponent<FactoryFishState>();
        if (fishState.CurrentState == successCondition)
        {
            GameManager.Instance.PlaySound("correct");  
        } else {
            GameManager.Instance.PlaySound("incorrect");
        }
   }
}
