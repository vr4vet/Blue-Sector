using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class Fish : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private int gillDamage = 3;
    [SerializeField]
    private int gillDamageGuessed = 0;
    [SerializeField]
    private int id;
    
    //The following variables are used for handeling behaviour with water
    private Vector3 waterBodyCenter;
    private Vector3 targetPosition;
    private Quaternion lookRotation;
    public float movementSpeed;
    public float rotationSpeed;
    private float waterBodyXLength;
    private float waterBodyZLength;
    private bool isInWater;

    private bool isGrabbed = false;
    private Rigidbody rigidBody;
    private Quaternion originalRotation;

    public BNG.UIPointer uIPointer;

    public GameObject marker;

    public GameObject pointerFinger;
    private List<GameObject> liceList = new List<GameObject>();

    InspectionTaskManager inspectionTaskManager;
    public LayerMask layer;

    public GameObject lastMarkedLouse;

    private int markedLice = 0;

    public int health = 10;

    private AudioSource hurtSound;

    // Start is called before the first frame update
    void Start()
    {
        //Skamløst kokt fra FishScript.cs:
        InvokeRepeating(nameof(PeriodicUpdates), Random.Range(0.0f, 5.0f), 1.0f);
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
        targetPosition = transform.position;
        rigidBody = GetComponent<Rigidbody>();
        originalRotation = transform.rotation;
        liceList = FindObjectwithTag("Louse");
        hurtSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInWater && !isGrabbed){
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            Move();
        } else {
            rigidBody.freezeRotation = false;
        }
    }

    void PeriodicUpdates() {
        if(isInWater && !isGrabbed){
            SetMoveTarget();    
        }
    }

    private void Move() {
        if( Vector3.Distance(transform.position, targetPosition) > .1 ) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
    }

    private void SetMoveTarget() {
        //Mer Skamløs koking fra FishScript.cs:
        float randX = Random.Range(waterBodyCenter.x -waterBodyXLength / 2,waterBodyCenter.x + waterBodyXLength / 2);
        float randZ = Random.Range(waterBodyCenter.z -waterBodyZLength / 2,waterBodyCenter.z + waterBodyZLength / 2);
        targetPosition = new Vector3(randX, transform.position.y, randZ);
        lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
    }

    public void UpdateWaterBody(float waterheight, Vector3 bodyCenter, float xLength, float zLength, bool isInWater){
        gameObject.GetComponent<Floating>().waterHeight = waterheight;
        waterBodyCenter = bodyCenter;
        waterBodyXLength = xLength;
        waterBodyZLength = zLength;
        this.isInWater = isInWater;
    }

    public void OnPointerClick(PointerEventData eventData) {
        lastMarkedLouse = checkForLouse(eventData.pointerCurrentRaycast.worldPosition);
        if(lastMarkedLouse != null){
            GameObject newmarker = Instantiate(marker,lastMarkedLouse.transform.position, new Quaternion(0,0,0,0));
            newmarker.transform.parent = transform;
        }
    }

    public GameObject checkForLouse(Vector3 origin) {
        if(Physics.SphereCast(origin - (pointerFinger.transform.forward*.5f), 0.02f, pointerFinger.transform.forward, out RaycastHit hitInfo, 10f, layer)) {
            GameObject hit = hitInfo.collider.gameObject;
            foreach(GameObject louse in liceList) {
                if (hit == louse && !louse.GetComponent<Louse>().marked) {
                    louse.GetComponent<Louse>().marked = true;
                    markedLice++;
                    return louse;
                }
            }
        }
        return null;
    }

    public void checkForDamage(bool hittingWater, float velocity) {
        float damageThreshold = 2f;
        if (hittingWater) {
            damageThreshold = 4f;
        }
        else if(isGrabbed){
            damageThreshold = 0.4f;
        }
        if(velocity > damageThreshold) {
            if(health > 0) {
                health--;
            }
            hurtSound.Play(0);
        }
    }

    private void OnCollisionEnter(Collision other) {
        SetMoveTarget();
        if(other.collider.isTrigger){
            checkForDamage(true, other.relativeVelocity.magnitude);
        }
        else {
            checkForDamage(false, other.relativeVelocity.magnitude);
        }
    }
    public void SetAsSelectedFish() {
        inspectionTaskManager.SetSelectedFish(this); 
    }

    public void SetgillDamageGuessed(int guess) {
        gillDamageGuessed = guess;
    }

    public void SetIsInWater( bool isInWater) {
        this.isInWater = isInWater;
    }

    public void SetIsGrabbed( bool isGrabbed) {
        this.isGrabbed = isGrabbed;
    }

    public int GetGillDamage() {
        return gillDamage;
    }

    public int GetGillDamageGuessed() {
        return gillDamageGuessed;
    }

    public int GetId() {
        return id;
    }

    public int GetMarkedLice() {
        return markedLice;
    }

    public List<GameObject> GetLiceList(){
        return liceList;
    }

    //Couple of util functions for finding children by tag
    public List<GameObject> FindObjectwithTag(string _tag) {
         List<GameObject> tempList = new List<GameObject>();
         Transform parent = transform;
         GetChildObject(parent, _tag, tempList);
         return tempList;
    }
 
     public void GetChildObject(Transform parent, string _tag, List<GameObject> list) {
         for (int i = 0; i < parent.childCount; i++)
         {
             Transform child = parent.GetChild(i);
             if (child.tag == _tag)
             {
                 list.Add(child.gameObject);
             }
             if (child.childCount > 0)
             {
                 GetChildObject(child, _tag, list);
             }
         }
     }
}
