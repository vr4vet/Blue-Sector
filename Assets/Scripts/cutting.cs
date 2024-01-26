using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutting : MonoBehaviour
{

    public ExampleUseof_MeshCut cut;
    public static Valve.VR.InteractionSystem.Hand hand;
    bool pickedUp;

    public void SetPickedUp(bool s)
    {
        pickedUp = s;
    }
    // Sets if knife is picked up that it then can cut, to prevent cuting when lying still
    void OnCollisionEnter(Collision col)
    {   
        if (pickedUp)
        {
            hand = transform.parent.gameObject.GetComponent<Valve.VR.InteractionSystem.Hand>();
            if (hand.GetTrackedObjectAngularVelocity().magnitude > 2)
            {
                cut.cutting();
            }
        }
    
        
        
    }
}