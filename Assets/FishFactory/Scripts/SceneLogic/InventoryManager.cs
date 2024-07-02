using System;
using Mono.Cecil;
using NUnit.Framework.Internal;
using UnityEngine;
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

    // ----------------- Private variables -----------------

    private GameObject rightObject;
    private GameObject leftObject;
    private UnityEngine.Object rightHandObject;
    private UnityEngine.Object leftHandObject;

    public UnityEvent<Rigidbody> heldItem;

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
    }

    public void GetRightGrabbable(UnityEngine.Object obj)
    {
        if (_bringGameObjectFromRightHand)
        {
            if (obj.ToString() != "HolsterRight (BNG.Grabbable)")
            {
                rightHandObject = obj;
                DontDestroyOnLoad(rightHandObject);
                Debug.Log(rightHandObject);
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
                DontDestroyOnLoad(leftHandObject);
            }
        }
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
            string test = rightHandObject.ToString().Replace(" (BNG.Grabbable)", "");
            GameObject rightHandGameObject = GameObject.Find(test).gameObject;

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
    }
} 
