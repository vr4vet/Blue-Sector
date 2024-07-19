using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BNG;
using Unity.VisualScripting;
using System.Collections;
using System.Linq;


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


    [Header("Blacllisted Items")]
    [Tooltip("Drag and drop existing game objects that should not be transferable between scenes")]
    [SerializeField]
    private List<GameObject> blacklistedGameObjects = new List<GameObject>();

    [Tooltip("Write tags whose objects should not be transferable between scenes")]
    [SerializeField]
    private List<String> blacklistedTags = new List<String>();

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


    private List<GameObject> finalFish = new List<GameObject>();

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

    /// <summary>
    /// As the old instance is deleted to allow for different settings between scenes the new instance inherits values from the old.
    /// </summary>
    private void InheritValuesFromOldInstance(InventoryManager oldInstance)
    {
        rightObject = oldInstance.rightObject;
        leftObject = oldInstance.leftObject;
        rightHandObject = oldInstance.rightHandObject;
        leftHandObject = oldInstance.leftHandObject;
        inventoryObjects = new Dictionary<int, Grabbable>(oldInstance.inventoryObjects);
        finalFish = new List<GameObject>(oldInstance.finalFish);
    }

    /// <summary>
    /// Function gets the held item in the right hand
    /// </summary>
    private void GetRightGrabbable()
    {
        Grabber rightPocket = GameObject.Find("PlayerController/CameraRig/TrackingSpace/RightHandAnchor/RightControllerAnchor/RightController/Grabber").gameObject.GetComponent<Grabber>();
        if (rightPocket.HeldGrabbable != null)
        {
            rightHandObject = rightPocket.HeldGrabbable;
            if (isItemBlacklisted(rightHandObject))
            {
                rightHandObject = null;
                return;
            }
            if (rightHandObject.tag == "Bone")
            {
                rightHandObject = null;
                return;
            }
            if (rightHandObject.name != "HolsterRight (BNG.Grabbable)")
            {
                rightHandObject.transform.parent = null;
                DontDestroyOnLoad(rightHandObject);
            }
        }
    }

    /// <summary>
    /// Function gets the held item in the left hand
    /// </summary>
    private void GetLeftGrabbable()
    {
        Grabber leftPocket = GameObject.Find("PlayerController/CameraRig/TrackingSpace/LeftHandAnchor/LeftControllerAnchor/LeftController/Grabber").gameObject.GetComponent<Grabber>();
        if (leftPocket.HeldGrabbable != null)
        {
            leftHandObject = leftPocket.HeldGrabbable;
            if (isItemBlacklisted(leftHandObject))
            {
                leftHandObject = null;
                return;
            }
            if (leftHandObject.tag == "Bone")
            {
                leftHandObject = null;
                return;
            }
            if (leftHandObject.name != "HolsterLeft (BNG.Grabbable)")
            {
                leftHandObject.transform.parent = null;
                DontDestroyOnLoad(leftHandObject);
            }
        }
    }

    /// <summary>
    /// The fish does not function as other grabbables as it has many grabbables and gets split once its put in the pocket.
    /// This function finds split fish and puts them back together before saving them
    /// </summary>
    private GameObject handleFish(Grabbable obj)
    {
        obj.ResetParent();
        var fish = obj.gameObject;
        while (true) {
            fish = fish.transform.parent.gameObject;
            if (fish.CompareTag("Fish"))
                break;
            continue;
                
        }
        fish.transform.parent = null;
        finalFish.Add(fish);
        DontDestroyOnLoad(fish);
        return fish;
        }

    private void BringObjectFromRightPocket()
    {
        rightObject = null;
        SnapZone rightPocket = GameObject.Find("Inventory").transform.GetChild(2).gameObject.GetComponent<SnapZone>();
        if (rightPocket.HeldItem != null)
        {
            rightObject = rightPocket.HeldItem;
            if (isItemBlacklisted(rightObject))
            {
                rightObject = null;
                return;
            }
            if (rightObject.tag == "Bone")
            {
                GameObject fishObject = handleFish(rightObject);
                try
                {
                    rightPocket.HeldItem = fishObject.transform.GetChild(2).transform.GetChild(0).GetComponent<Grabbable>();
                    rightObject = fishObject.transform.GetChild(2).transform.GetChild(0).GetComponent<Grabbable>();
                }
                catch (Exception e)
                {}
            }
            else
            {
                rightObject.transform.parent = null;
                DontDestroyOnLoad(rightObject);
            }
        }
    }

    private void BringObjectFromLeftPocket()
    {
        leftObject = null;
        SnapZone leftPocket = GameObject.Find("Inventory").transform.GetChild(1).gameObject.GetComponent<SnapZone>();
        if (leftPocket.HeldItem != null)
        {
            leftObject = leftPocket.HeldItem;
            if (isItemBlacklisted(leftObject))
            {
                leftObject = null;
                return;
            }
            if (leftObject.tag == "Bone")
            {
                GameObject fishObject = handleFish(leftObject);
                try
                {
                    leftPocket.HeldItem = fishObject.transform.GetChild(2).transform.GetChild(0).GetComponent<Grabbable>();
                    leftObject = fishObject.transform.GetChild(2).transform.GetChild(0).GetComponent<Grabbable>();
                }
                catch (Exception e)
                {}
            }
            else
            {
                leftObject.transform.parent = null;
                DontDestroyOnLoad(leftObject);
            }
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
                    if (isItemBlacklisted(objectInInventory))
                    {
                        objectInInventory = null;
                        continue;
                    }
                    
                    if (objectInInventory.tag == "Bone")
                    {
                        GameObject fishObject = handleFish(leftObject);
                        try
                        {
                            objectInInventory = fishObject.transform.GetChild(2).transform.GetChild(0).GetComponent<Grabbable>();
                        }
                        catch (Exception e)
                        {}
                    }
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
        foreach (GameObject fish in finalFish)
        {
            SceneManager.MoveGameObjectToScene(fish, SceneManager.GetActiveScene());
        }
        finalFish.Clear();

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

    // ----------------- Helper Functions -----------------
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

    /// <summary>
    /// Function finds the last bone not in the inventory of the player.
    /// This is used to put a fish that is in the pocket back together with the part that is outside the pocket.
    /// </summary>
    private GameObject findBottom(GameObject fish)
    {
        GameObject child = fish.transform.GetChild(2).gameObject;
        while (true)
        {
            if (child.transform.childCount > 0)
            {
                child = child.transform.GetChild(0).gameObject;
            }
            else
            {
                return child;
            }
        }
    }

    private bool isItemBlacklisted(Grabbable item)
    {
        foreach (GameObject blacklistedGamObject in blacklistedGameObjects)
        {
            if (item.gameObject == blacklistedGamObject)
            {
                Debug.Log("Blacklisted gameobject");
                return true;
            }
        }
        foreach (String blacklistedTag in blacklistedTags)
        {
            if (item.gameObject.tag == blacklistedTag)
            {
                Debug.Log("Blacklisted tag");
                return true;
            }
        }
        return false;
    }
} 
