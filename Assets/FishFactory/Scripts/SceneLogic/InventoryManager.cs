using System;
using Mono.Cecil;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


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

    [Tooltip("Bring the game object carried in the inventory to the next scene")]
    [SerializeField]
    private GameObject inventoryObject;

    // ----------------- Editor Fields For Debug -----------------

    [Header("For Debug")]
    [Header("Pocket objects")]
    [SerializeField]
    private GameObject rightObject;
    [SerializeField]
    private GameObject leftObject;
    [Header("Hand Objects")]
    [SerializeField]
    private UnityEngine.Object rightHandObject;
    [SerializeField]
    private UnityEngine.Object leftHandObject;

    [Header("inventory objects")]
    [SerializeField]
    private UnityEngine.Object slot1;
    [SerializeField]
    private UnityEngine.Object slot2;
    [SerializeField]
    private UnityEngine.Object slot3;
    [SerializeField]
    private UnityEngine.Object slot4;
    [SerializeField]
    private UnityEngine.Object slot5;
    [SerializeField]
    private UnityEngine.Object slot6;

    public UnityEvent<Rigidbody> heldItem;

    private GameObject[] inventoryObjects;

    // ----------------- Unity Functions -----------------

    void Awake()
    {
        // Sets the instance of the BringCarriedGameObjects to this object if it does not already exist
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Makes the BringCarriedGameObjects object persist between scenes
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += PopulateInventory;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= PopulateInventory;
    }

    // ----------------- Public Functions -----------------

    /// <summary>
    /// This function needs to be called before the ChangeScene() function is called. 
    /// It is done by calling InventoryManager.Instance.SaveInventory() in the same function that calls the ChangeScene().
    /// An example of this can be found in the script SceneController.cs in the function OnTriggerEnter().
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
        if (rightHandObject)
        {
            DontDestroyOnLoad(rightHandObject);
        }
        if (leftHandObject)
        {
            DontDestroyOnLoad(leftHandObject);
        }
        if (_bringGameObjectFromInventory)
        {
            BringObjectsFromInventory();
        }
    }

    public void GetRightGrabbable(UnityEngine.Object obj)
    {
        if (_bringGameObjectFromRightHand)
        {
            if (obj.ToString() != "HolsterRight (BNG.Grabbable)")
            {
                rightHandObject = obj;
            }
        }
    }

    public void GetLeftGrabbable(UnityEngine.Object obj)
    {
        if (_bringGameObjectFromLeftHand)
        {
            if (obj.ToString() != "HolsterLeft (BNG.Grabbable)")
            {
                leftHandObject = obj;
            }
        }
    }

    public void discardRightGrabbable()
    {
        rightHandObject = null;
    }
    public void discardLeftGrabbable()
    {
        leftHandObject = null;
    }

    // ----------------- Private Functions -----------------

    private void BringObjectFromRightPocket()
    {
        rightObject = GameObject.Find("XR Rig Advanced VR4VET/Inventory/HolsterRight").transform.GetChild(1).gameObject;
        rightObject.transform.parent = null;
        DontDestroyOnLoad(rightObject);
    }

    private void BringObjectFromLeftPocket()
    {
        leftObject = GameObject.Find("XR Rig Advanced VR4VET/Inventory/HolsterLeft").transform.GetChild(1).gameObject;
        leftObject.transform.parent = null;
        DontDestroyOnLoad(leftObject);
    }

    private void BringObjectsFromInventory()
    {
        for (int i = 1; i < 7; i++)
        {
            inventoryObject = inventoryObject.transform.GetChild(i).transform.GetChild(1).gameObject;
        }
        Debug.Log(inventoryObject);
    }

    /// <summary>
    /// Once a new scene has been loaded this function populates the inventory with the saved items
    /// </summary>
    private void PopulateInventory(Scene scene, LoadSceneMode mode)
    {
        if (rightObject != null)
        {
            // Set object to be kinematic to stop it from falling
            Rigidbody rightRigidBody = rightObject.GetComponent<Rigidbody>();
            rightRigidBody.isKinematic = true;

            GameObject rightPocket = GameObject.Find("Inventory").transform.GetChild(2).gameObject;
            rightObject.transform.SetParent(rightPocket.transform);
            rightObject.transform.position = rightPocket.transform.position + new Vector3(0,0.02f,0);
            rightObject.transform.rotation = rightPocket.transform.rotation;
            rightObject = null;
        }

        if (leftObject != null)
        {
            // Set object to be kinematic to stop it from falling
            Rigidbody leftRigidBody = leftObject.GetComponent<Rigidbody>();
            leftRigidBody.isKinematic = true;

            GameObject leftPocket = GameObject.Find("Inventory").transform.GetChild(1).gameObject;
            leftObject.transform.SetParent(leftPocket.transform);
            leftObject.transform.position = leftPocket.transform.position + new Vector3(0,0.02f,0);
            leftObject.transform.rotation = leftPocket.transform.rotation;
            leftObject = null;
        }
        
        if (rightHandObject != null)
        {
            // Find the object
            string seachString = rightHandObject.ToString().Replace(" (BNG.Grabbable)", "");
            GameObject rightHandGameObject = GameObject.Find(seachString).gameObject;

            // Create a copy and destroy the old
            GameObject newObject = Instantiate(rightHandGameObject);
            Destroy(rightHandGameObject);

            Rigidbody objectRigidbody = newObject.GetComponent<Rigidbody>();
            objectRigidbody.isKinematic = true;

            GameObject rightHand = GameObject.Find("RightController").gameObject;
            newObject.transform.SetParent(rightHand.transform);
            newObject.transform.position = rightHand.transform.position;
            newObject.transform.rotation = rightHand.transform.rotation;
        }

        if (leftHandObject != null)
        {
            // Find the object
            string seachString = leftHandObject.ToString().Replace(" (BNG.Grabbable)", "");
            GameObject leftHandGameObject = GameObject.Find(seachString).gameObject;

            // Create a copy and destroy the old
            GameObject newObject = Instantiate(leftHandGameObject);
            Destroy(leftHandGameObject);

            Rigidbody objectRigidbody = newObject.GetComponent<Rigidbody>();
            objectRigidbody.isKinematic = true;

            GameObject leftHand = GameObject.Find("LeftController").gameObject;
            newObject.transform.SetParent(leftHand.transform);
            newObject.transform.position = leftHand.transform.position;
            newObject.transform.rotation = leftHand.transform.rotation;
        }
    }
} 
