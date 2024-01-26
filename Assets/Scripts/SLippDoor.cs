using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Valve.VR.InteractionSystem
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]

    public class SLippDoor : MonoBehaviour
    {

        public GameObject door;
        public GameObject cabinett;

        void Start()
        {
        }


        private void HandHoverUpdate(Hand hand)
        {
            if (hand.GetStandardInteractionButtonDown())
            {
                hand.HoverLock(GetComponent<Interactable>());
            }

            if (hand.GetStandardInteractionButtonUp())
            {
                hand.HoverUnlock(GetComponent<Interactable>());
                door.transform.SetParent(cabinett.transform);
            }

            if (hand.GetStandardInteractionButton())
            {
                door.transform.SetParent(hand.transform);
            }
        }
        void Update()
        {
        }
    }
}