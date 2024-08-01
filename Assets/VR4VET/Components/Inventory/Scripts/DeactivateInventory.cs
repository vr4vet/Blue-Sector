using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.InputSystem;

public class DeactivateInventory : MonoBehaviour
{
    private List<GameObject> heldItemsList = new List<GameObject>();
    public InputActionReference ActivateDeactivateInput;
        private void OnEnable() {
            ActivateDeactivateInput.action.performed += ToggleActive;
        }

        private void OnDisable() {
            ActivateDeactivateInput.action.performed -= ToggleActive;
        }
    
    public void ToggleActive(InputAction.CallbackContext context) {
        if (gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            heldItemsList = new List<GameObject>();
            foreach (SnapZone zone in gameObject.GetComponentsInChildren<SnapZone>())
            {
                if (zone.HeldItem != null && zone.HeldItem.gameObject.tag == "Bone")
                {
                    var obj = zone.HeldItem;
                    obj.ResetParent();
                    obj.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    var fish = obj.gameObject;
                    while (true) {
                        if (fish.CompareTag("Fish"))
                            break;
                        fish = fish.transform.parent.gameObject;
                    }
                    GameObject copy = Instantiate(fish);
                    Destroy(fish);
                    heldItemsList.Add(copy);
                }
                else
                {
                    GameObject nullObject = new GameObject();
                    heldItemsList.Add(nullObject);
                }
            }
            foreach (GameObject obj in heldItemsList)
            {
                obj.SetActive(false);
            }
            gameObject.transform.GetChild(0).gameObject.SetActive(!gameObject.transform.GetChild(0).gameObject.activeInHierarchy);
        }
        else
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(!gameObject.transform.GetChild(0).gameObject.activeInHierarchy);
            foreach (SnapZone zone in gameObject.GetComponentsInChildren<SnapZone>())
            {
                foreach (GameObject fish in heldItemsList)
                {
                    heldItemsList.Remove(fish);
                    if (fish.tag != "Fish")
                    {
                        Destroy(fish);
                        break;
                    }
                    zone.GrabGrabbable(fish.transform.GetChild(2).transform.GetChild(0).GetComponent<Grabbable>());
                    fish.SetActive(true);
                    break;
                }
            }  
        }
    }
}
