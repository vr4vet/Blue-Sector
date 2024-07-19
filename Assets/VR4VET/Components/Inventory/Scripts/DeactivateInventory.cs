using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.InputSystem;

public class DeactivateInventory : MonoBehaviour
{
    public InputActionReference ActivateDeactivateInput;
        private void OnEnable() {
            ActivateDeactivateInput.action.performed += ToggleActive;
        }

        private void OnDisable() {
            ActivateDeactivateInput.action.performed -= ToggleActive;
        }
    
    public void ToggleActive(InputAction.CallbackContext context) {
            foreach (SnapZone zone in gameObject.GetComponentsInChildren<SnapZone>())
            {
                if (zone.HeldItem != null)
                {
                    var obj = zone.HeldItem;
                    obj.ResetParent();
                    var fish = obj.gameObject;
                    while (true) {
                        fish = fish.transform.parent.gameObject;
                        if (fish.CompareTag("Fish"))
                            break;
                    }
                    fish.SetActive(!fish.activeInHierarchy);
                    var fishHead = fish.transform.GetChild(2).transform.GetChild(0).GetComponent<Grabbable>();
                   zone.GrabGrabbable(fishHead);
                }
            }
            gameObject.transform.GetChild(0).gameObject.SetActive(!gameObject.transform.GetChild(0).gameObject.activeInHierarchy);
    }
}
