using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.Events;
//using System.Diagnostics;

public class Fish : MonoBehaviour
{
    [SerializeField]
    private int gillDamage = 3;
    [SerializeField]
    private int gillDamageGuessed = 0;
    [SerializeField]
    private int id;
    [HideInInspector]
    public Vector3 waterBodyCenter;
    private Vector3 targetPosition;
    private Quaternion lookRotation;
    public float movementSpeed = .5f;
    private float originalMovementSpeed = .5f;
    public float rotationSpeed = 10f;
    private float originalRotationSpeed = 10f;
    [HideInInspector]
    public float waterBodyXLength;
    [HideInInspector]
    public float waterBodyZLength;
    [HideInInspector]
    public float waterBodyYLength;
    private Quaternion originalRotation;
    [HideInInspector]
    public float waterHeight;
    [HideInInspector]
    public BNG.UIPointer uIPointer;
    public GameObject marker;
    private GameObject pointerFinger;
    private List<GameObject> liceList = new List<GameObject>();
    private List<GameObject> boneList = new List<GameObject>();
    //private GameObject[] boneList;// = new List<GameObject>(); //;)
    InspectionTaskManager inspectionTaskManager;
    public LayerMask layer;
    [HideInInspector]
    public GameObject lastMarkedLouse;
    [HideInInspector]
    public int markedLice = 0;

    public int numberOfLice = 5;
    public int health = 10;
    private AudioSource hurtSound;
    [HideInInspector]
    public AudioSource markSound;
    [HideInInspector]
    public int isInWaterCount = 0;
    //[HideInInspector]
    //public bool isInWater = true;
    [HideInInspector]
    public int isGrabbedCount = 0;
    private bool kinematicBones = false;
    private Animator animator;
    private Transform fishbone;
    [HideInInspector]
    public RowUi scoreBoardEntry;
    private bool damageInvulerability = true;
    private float damageInvulnerabilityTimer = 1f;
    public float unsediatedLevel = 1f;
    public TankController tank;
    public TankController startTank;
    public TankController endTank;
    //with a sedativeConsentration of 0.01 the sedationTimer will take 5 minutes to count down.
    public float sedationTimer = 3;

    private bool putInWater = true;

    // following variables are used to prevent physics from going amok
    private float transitionToAnimationTime;
    private Vector3 transitionToAnimationPosition;
    private List<Vector3> transitionToAnimationBonesPosition = new List<Vector3> ();
    private List<Quaternion> transitionToAnimationBonesRotation = new List<Quaternion>();

    public UnityEvent m_OnFishCalmedDown; // event to set waiting step of subtask to completed when fish has calmed down
    private bool _waitingStepCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        if (m_OnFishCalmedDown == null)
            m_OnFishCalmedDown = new();

        InvokeRepeating(nameof(PeriodicUpdates), Random.Range(0.0f, 3.0f), 1.0f);
        inspectionTaskManager = GameObject.FindObjectOfType<InspectionTaskManager>();
        targetPosition = transform.position;
        originalRotation = transform.rotation;
        liceList = FindObjectwithTag("Louse");
        boneList = FindObjectwithTag("Bone");//FindGameObjectsWithTag("Bone");
        AudioSource[] sounds = GetComponents<AudioSource>();
        hurtSound = sounds[0];
        //markSound = sounds[1];
        //The point from which the raycast targeting lice on fishbodie will have its origin. In this case it is RightHandPointer in XR Rig Advanced
        pointerFinger = GameObject.FindGameObjectWithTag("Pointer");
        animator = GetComponent<Animator>();
        fishbone = boneList[0].transform;
        originalMovementSpeed = movementSpeed;
        originalRotationSpeed = rotationSpeed;
        findClosestTank();
    }

    // using LateUpdate instead of Update because corrective maneuvers in Move() need happen after physics is processed
    void LateUpdate()
    {
        //Debug.Log(transform.name);
        //Debug.Log(isGrabbedCount);
        followchild();
        if (isGrabbedCount > 0 || !IsInWater())
            Stop();
        if (IsInWater() && isGrabbedCount <= 0)
            Move();




        damageInvulnerabilityTimer -= Time.deltaTime;
        if (damageInvulnerabilityTimer <= 0f)
        {
            damageInvulerability = false;
        }
        checkForOverSedation();
        if (scoreBoardEntry != null)
            scoreBoardEntry.handling.text = health.ToString();



    }

    void PeriodicUpdates()
    {
        findClosestTank();
        if (IsInWater() && isGrabbedCount <= 0)
        {
            SetMoveTarget();
        }
        if (isGrabbedCount > 0 && Random.Range(0, 1) < unsediatedLevel && health > 0)
        {
            health -= 1;
        }
    }

    public void Move()
    {
        //Debug.Log("Moving");
        if (!kinematicBones && transform.position.y <= waterBodyCenter.y)
        {
            transitionToAnimationTime = Time.time;
            foreach (GameObject bone in boneList)
            {
                bone.GetComponent<Rigidbody>().isKinematic = true;
                kinematicBones = true;
                animator.enabled = true;

                // storing positions to lock fish here
                transitionToAnimationPosition = fishbone.position;
                transitionToAnimationBonesPosition.Add(bone.transform.localPosition);
                transitionToAnimationBonesRotation.Add(bone.transform.localRotation);
            }
        }

        float currentTime = Time.time;
        if (currentTime - transitionToAnimationTime < 0.25f)   // prevent stretching of body
        {
            //Debug.Log("Fish bones are locked in place!");
            foreach (Vector3 position in transitionToAnimationBonesPosition)
            {
                boneList[transitionToAnimationBonesPosition.IndexOf(position)].transform.localPosition = position;
            }
            foreach (Quaternion rotation in transitionToAnimationBonesRotation)
            {
                boneList[transitionToAnimationBonesRotation.IndexOf(rotation)].transform.localRotation = rotation;
            }
        }
        if (currentTime - transitionToAnimationTime < 1.0f)   // prevent drifting towards previous tank position
        {
            animator.rootPosition = transitionToAnimationPosition;
        }
        else if (Vector3.Distance(transform.position, targetPosition) > 0.1f)   // move fish as normal
        {
            //Debug.Log("Done transitioning");
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        updateSedation();
    }

    private void Stop()
    {
        //Debug.Log("Stopping");
        if (kinematicBones)
        {
            foreach (GameObject bone in boneList)
            {
                bone.GetComponent<Rigidbody>().isKinematic = false;
                bone.GetComponent<Rigidbody>().velocity = Vector3.zero;
                kinematicBones = false;
                animator.enabled = false;
            }
            transitionToAnimationBonesPosition.Clear();
            transitionToAnimationBonesRotation.Clear();
        }
    }

    public void findClosestTank()
    {
        float startdist = Vector3.Distance(startTank.transform.position, transform.position);
        float endDist = Vector3.Distance(endTank.transform.position, transform.position);

        if (startdist < endDist)
        {
            tank = startTank;
        }
        else
        {
            tank = endTank;
        }

        Bounds waterBounds = tank.transform.Find("Water").gameObject.GetComponent<BoxCollider>().bounds;
        waterBodyXLength = waterBounds.size.x;
        waterBodyZLength = waterBounds.size.z;
        waterBodyYLength = waterBounds.size.y;
        waterBodyCenter = waterBounds.center;
    }

    private void updateSedation()
    {
        if (tank.sedativeConsentration == 0f)
        {
            if (unsediatedLevel < 1f)
            {
                unsediatedLevel += Time.deltaTime * 0.01f;
            }
        }
        else
        {
            if (unsediatedLevel > 0f && tank != null)
            {
                unsediatedLevel -= Time.deltaTime * tank.sedativeConsentration;
            }
            else if (unsediatedLevel < 0f)
            {
                unsediatedLevel = 0f;
            }
        }
        animator.speed = unsediatedLevel >= 0 ? unsediatedLevel : 0;
        movementSpeed = originalMovementSpeed * unsediatedLevel;
        rotationSpeed = (originalRotationSpeed * unsediatedLevel) / 1.5f;
    }

    private void followchild()
    {
        Vector3 originalPosition = fishbone.position;
        transform.position = fishbone.position;
        fishbone.position = originalPosition;
    }

    private void checkForOverSedation()
    {
        if (unsediatedLevel < 1f)
        {
            sedationTimer -= Time.deltaTime * tank.sedativeConsentration;
        }
        if (sedationTimer <= 0 && health > 0)
        {
            health -= 1;
            sedationTimer = .1f;
        }

        if (!_waitingStepCompleted && sedationTimer <= 0) // invoke event to tell tablet to complete waiting step
        {
            _waitingStepCompleted = true;
            m_OnFishCalmedDown.Invoke();
        }    
    }

    public void SetMoveTarget()
    {

        float XLength = waterBodyXLength - 0.1f;
        float ZLength = waterBodyZLength - 0.1f;
        float YLength = waterBodyYLength - 0.2f;

        float randX = Random.Range(waterBodyCenter.x - XLength / 2, waterBodyCenter.x + XLength / 2);
        float randZ = Random.Range(waterBodyCenter.z - ZLength / 2, waterBodyCenter.z + ZLength / 2);
        float randY = Random.Range(waterBodyCenter.y - YLength / 2, waterBodyCenter.y + YLength / 2);

        targetPosition = new Vector3(randX, randY, randZ);
        lookRotation = Quaternion.LookRotation(targetPosition - transform.position);
    }

    public void checkForDamage(bool hittingWater, float velocity)
    {
        if (!damageInvulerability)
        {
            damageInvulerability = true;
            damageInvulnerabilityTimer = 1f;
            float damageThreshold = 2f;
            if (hittingWater)
            {
                damageThreshold = 3f;
            }
            else if (isGrabbedCount > 0)
            {
                damageThreshold = 2f;
            }
            if (velocity > damageThreshold)
            {
                if (health > 0)
                {
                    health--;
                }
                hurtSound.Play(0);
            }
            //Debug.Log("Taking Damage");
        }
    }

    public bool IsInWater()
    {
        foreach (Bone bone in gameObject.transform.GetComponentsInChildren<Bone>())
        {
            if (bone.isInWater)
                return true;
        }
        return false;
    }

    private void OnCollisionEnter(Collision other)
    {
        findClosestTank();
        SetMoveTarget();
        //Debug.Log(tank.name);
        if (other.collider.isTrigger)
        {
            checkForDamage(true, other.relativeVelocity.magnitude);
        }
        else
        {
            checkForDamage(false, other.relativeVelocity.magnitude);
        }
    }
    public void SetAsSelectedFish()
    {
        inspectionTaskManager.SetSelectedFish(this);
    }

    public void SetgillDamageGuessed(int guess)
    {
        gillDamageGuessed = guess;
    }

    public int GetGillDamage()
    {
        return gillDamage;
    }

    public int GetGillDamageGuessed()
    {
        return gillDamageGuessed;
    }

    public int GetId()
    {
        return id;
    }

    public int GetMarkedLice()
    {
        return markedLice;
    }

    public List<GameObject> GetLiceList()
    {
        return liceList;
    }

    //Couple of util functions for finding children by tag
    public List<GameObject> FindObjectwithTag(string _tag)
    {
        List<GameObject> tempList = new List<GameObject>();
        Transform parent = transform;
        GetChildObject(parent, _tag, tempList);
        return tempList;
    }

    public void GetChildObject(Transform parent, string _tag, List<GameObject> list)
    {
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



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPosition, 0.05f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(waterBodyCenter, 0.1f);
    }
}