using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BNG;
using Unity.VisualScripting;
using System.Collections;


public class InventoryManager : MonoBehaviour
{
    // ----------------- InventoryManager Instance -----------------

    // The Inventory Manager follows the Singleton pattern to ensure that only one instance of the InventoryManager exists at any given time
    // The instance can be accessed from any script by calling InventoryManager.Instance

    public static InventoryManager Instance;

    
    // ----------------- Editor Fields -----------------
    
    [Header("Inventory manager")]
    [Header("Pockets")]
    [Tooltip("Bring the game object carried in the right pocket to the next scene")]
    [SerializeField]
    private bool _bringGameObjectFromRightPocket; 

    [Tooltip("Bring the game object carried in the left pocket to the next scene")]
    [SerializeField]
    private bool _bringGameObjectFromLeftPocket; 

    [Header("Hands")]
    [Tooltip("Bring the game object carried in the right hand to the next scene")]
    [SerializeField]
    private bool _bringGameObjectFromRightHand; 

    [Tooltip("Bring the game object carried in the left hand to the next scene")]
    [SerializeField]
    private bool _bringGameObjectFromLeftHand; 

    [Header("Inventory")]
    [Tooltip("Bring the game object carried in the inventory to the next scene")]
    [SerializeField]
    private bool _bringGameObjectFromInventory;

    // ----------------- Editor Fields For Debug -----------------

    [Header("For Debug")]
    [Header("Pocket objects")]
    [SerializeField]
    private Grabbable rightObject;
    [SerializeField]
    private Grabbable leftObject;

    [Header("Hand Objects")]
    [SerializeField]
    private Grabbable rightHandObject;
    [SerializeField]
    private Grabbable leftHandObject;

    // ----------------- Private variables -----------------

    private Dictionary<int, Grabbable> inventoryObjects = new Dictionary<int, Grabbable>();

    // ----------------- Unity Functions -----------------

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            InheritValuesFromOldInstance(Instance);
            Destroy(Instance.gameObject);
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PopulateInventory(scene, mode);
    }

    // ----------------- Public Functions -----------------

    /// <summary>
    /// This function needs to be called before the ChangeScene() function is called. 
    /// It can be done by calling InventoryManager.Instance.SaveInventory() in the same function that calls the ChangeScene().
    /// A better aproach is to use UnityEvent and add this before the scene change.
    /// </summary>
    public void SaveInventory()
    {
        if (_bringGameObjectFromRightPocket)
        {
            BringObjectFromRightPocket();
        }
        if (_bringGameObjectFromLeftPocket)
        {
            BringObjectFromLeftPocket();
        }
        if (_bringGameObjectFromRightHand)
        {
            GetRightGrabbable();
        }
        if (_bringGameObjectFromLeftHand)
        {
            GetLeftGrabbable();
        }
        if (_bringGameObjectFromInventory)
        {
            BringObjectsFromInventory();
        }
    }

    // ----------------- Private Functions -----------------

    private void InheritValuesFromOldInstance(InventoryManager oldInstance)
    {
        rightObject = oldInstance.rightObject;
        leftObject = oldInstance.leftObject;
        rightHandObject = oldInstance.rightHandObject;
        leftHandObject = oldInstance.leftHandObject;
        inventoryObjects = new Dictionary<int, Grabbable>(oldInstance.inventoryObjects);
    }
    /// <summary>
    /// Function should be an event on the "on grab event" for the right grabber 
    /// </summary>
    private void GetRightGrabbable()
    {
        Grabber rightPocket = GameObject.Find("PlayerController/CameraRig/TrackingSpace/RightHandAnchor/RightControllerAnchor/RightController/Grabber").gameObject.GetComponent<Grabber>();
        if (rightPocket.HeldGrabbable != null)
        {
            rightHandObject = rightPocket.HeldGrabbable;
            if (rightHandObject.name != "HolsterRight (BNG.Grabbable)")
            {
                rightHandObject.transform.parent = null;
                DontDestroyOnLoad(rightHandObject);
            }
        }
    }

    /// <summary>
    /// Function should be an event on the "on grab event" for the left grabber 
    /// </summary>
    private void GetLeftGrabbable()
    {
        Grabber leftPocket = GameObject.Find("PlayerController/CameraRig/TrackingSpace/LeftHandAnchor/LeftControllerAnchor/LeftController/Grabber").gameObject.GetComponent<Grabber>();
        if (leftPocket.HeldGrabbable != null)
        {
            leftHandObject = leftPocket.HeldGrabbable;
            if (leftHandObject.name != "HolsterLeft (BNG.Grabbable)")
            {
                leftHandObject.transform.parent = null;
                DontDestroyOnLoad(leftHandObject);
            }
        }
    }

    private void BringObjectFromRightPocket()
    {
        SnapZone rightPocket = GameObject.Find("Inventory").transform.GetChild(2).gameObject.GetComponent<SnapZone>();
        if (rightPocket.HeldItem != null)
        {
            rightObject = rightPocket.HeldItem;
            rightObject.transform.parent = null;
            DontDestroyOnLoad(rightObject);
        }
    }

    private void BringObjectFromLeftPocket()
    {
        SnapZone leftPocket = GameObject.Find("Inventory").transform.GetChild(1).gameObject.GetComponent<SnapZone>();
        if (leftPocket.HeldItem != null)
        {
            leftObject = leftPocket.HeldItem;
            leftObject.transform.parent = null;
            DontDestroyOnLoad(leftObject);
        }
    }

    private void BringObjectsFromInventory()
    {
        for (int i = 1; i < 7; i++)
        {
            try
            {
                Grabbable objectInInventory = GameObject.Find("PlayerController/CameraRig/TrackingSpace/LeftHandAnchor/LeftControllerAnchor/LeftController/PopupInventoryAnchor/PopupInventory").transform.GetChild(i).gameObject.GetComponent<SnapZone>().HeldItem;
                if (objectInInventory)
                {
                    inventoryObjects.Add(i, objectInInventory);
                    objectInInventory.transform.parent = null;
                    DontDestroyOnLoad(objectInInventory);
                }  
            }
            catch (Exception e)
            {}
        }
    }

    /// <summary>
    /// Once a new scene has been loaded this function populates the inventory with the saved items
    /// </summary>
    private void PopulateInventory(Scene scene, LoadSceneMode mode)
    {
        if (rightObject != null)
        {
            SnapZone rightPocket = GameObject.Find("Inventory").transform.GetChild(2).gameObject.GetComponent<SnapZone>();
            rightPocket.StartingItem = rightObject;
            rightPocket.HeldItem = rightObject;
        }

        if (leftObject != null)
        {
            SnapZone leftPocket = GameObject.Find("Inventory").transform.GetChild(1).gameObject.GetComponent<SnapZone>();
            leftPocket.StartingItem = leftObject;
            leftPocket.HeldItem = leftObject;
        }
        
        if (rightHandObject != null)
        {
            Grabber rightHand = GameObject.Find("RightController/Grabber").gameObject.GetComponent<Grabber>();
            Grabbable newRightObject = Instantiate(rightHandObject);
            Destroy(rightHandObject.GameObject().gameObject);
            rightHandObject = null;

            newRightObject.transform.SetParent(rightHand.transform);
            newRightObject.GameObject().gameObject.GetComponent<Rigidbody>().isKinematic = true;
            newRightObject.GameObject().transform.position = rightHand.transform.position;
            newRightObject.GameObject().transform.rotation = rightHand.transform.rotation;

            rightHand.ForceGrab = true;
            rightHand.EquipGrabbableOnStart = newRightObject;
            rightHand.HeldGrabbable = newRightObject;
            StartCoroutine(CheckForReGrab(rightHand, newRightObject, true));
        }

        if (leftHandObject != null)
        {
            Grabber leftHand = GameObject.Find("LeftController/Grabber").gameObject.GetComponent<Grabber>();
            Grabbable newLeftObject = Instantiate(leftHandObject);
            Destroy(leftHandObject.GameObject().gameObject);

            newLeftObject.transform.SetParent(leftHand.transform);
            newLeftObject.GameObject().gameObject.GetComponent<Rigidbody>().isKinematic = true;
            newLeftObject.GameObject().transform.position = leftHand.transform.position;
            newLeftObject.GameObject().transform.rotation = leftHand.transform.rotation;

            leftHand.ForceGrab = true;
            leftHand.EquipGrabbableOnStart = newLeftObject;
            leftHand.HeldGrabbable = newLeftObject;
            StartCoroutine(CheckForReGrab(leftHand, newLeftObject, false));
        }
        if (inventoryObjects.Count > 0)
        {
            GameObject inventory = GameObject.Find("PlayerController/CameraRig/TrackingSpace/LeftHandAnchor/LeftControllerAnchor/LeftController/PopupInventoryAnchor/PopupInventory").gameObject;
            foreach (int key in inventoryObjects.Keys)
            {
                SnapZone correctSlot = inventory.transform.GetChild(key).gameObject.GetComponent<SnapZone>();
                correctSlot.HeldItem = inventoryObjects[key];
            }
            inventoryObjects.Clear();
        }
    }

    // ----------------- Corutine Functions -----------------
    IEnumerator CheckForReGrab(Grabber hand, Grabbable newObject, bool right)
    {
        HandCollision grip;
        if (right)
        {
            grip = GameObject.Find("RightController/ModelsRight/Green Gloves Right").gameObject.GetComponent<HandCollision>();
        }
        else{
            grip = GameObject.Find("LeftController/ModelsLeft/Green Gloves Left").gameObject.GetComponent<HandCollision>();
        }
        while (grip.GripAmount >= 0)
        {
            if (grip.GripAmount == 1)
            {
                while (grip.GripAmount == 1)
                {
                    yield return new WaitForSeconds(0.05f);
                }
                hand.ForceGrab = false;
                newObject.DropItem(hand, true, true);
                newObject.GameObject().gameObject.GetComponent<Rigidbody>().isKinematic = false;
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
        yield break;
    }
} 
