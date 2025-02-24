using UnityEngine;
using System.Collections.Generic;
using BNG;
using System.Linq;

public class DropWhenStretched : MonoBehaviour
{
    [SerializeField] private Transform Head;
    [SerializeField] private float StretchLimit = .15f;
    private GameObject _grabbedJoint;
    private List<float> _distances = new();
    private int _grabCount = 0;
    private int _fishCollides = 0;
    private Grabber _playerGrabberLeft, _playerGrabberRight;
    private float _waitTime = 0;


    private void Awake()
    {
        if (Head == null)
            Debug.LogError("Head field is null! This field should be set to the armature's head joint!");

        InitialJointDistances();
    }

    // Update is called once per frame
    void Update()
    {
        // getting the player hand grabbers. they are not immediately accessible, so they are fetched here
        if (_playerGrabberLeft == null || _playerGrabberRight == null)
        {
            GameObject playerRig = GameManager.Instance.GetPlayerRig();
            if (playerRig != null)
            {
                List<Grabber> grabbers = playerRig.GetComponentsInChildren<Grabber>().ToList();
                _playerGrabberLeft = grabbers.Find(x => x.transform.parent.name == "LeftController");
                _playerGrabberRight = grabbers.Find(x => x.transform.parent.name == "RightController");
            }
            return;
        }

        // return when fish is not grabbed, or if held with one hand while touching the sorting machine's sticky surface. prevents unneccesary calculations and unwanted anti-stretch corrections
        if (_grabCount == 0 || _fishCollides > 0 && _grabCount < 2)
            return;

        // make anti-stretch corrections if fish is currently stretched and enough time has passed since last correction. waiting prevents wild and rapid corrections
        if (JointDistancesTooGreat() && Time.time - _waitTime >= 2f)
        {
            _waitTime = Time.time;
            if (_grabCount == 2) // select random hand and make it drop its held fish joint
            {
                if (Random.value >= 0.5)
                    _playerGrabberLeft.TryRelease();
                else
                    _playerGrabberRight.TryRelease();
            }
            else if (_grabCount == 1) // find which joint the player is holding and move all joints towards it. prevents fish from latching into objects and becoming stretched when player moves held joint
            {
                foreach (WhichJointGrabbed Joint in GetComponentsInChildren<WhichJointGrabbed>())
                {
                    if (Joint.Grabbed)
                    {
                        _grabbedJoint = Joint.gameObject;
                        break;
                    }
                }

                foreach (WhichJointGrabbed Joint in GetComponentsInChildren<WhichJointGrabbed>())
                {
                    if (!Joint.Grabbed)
                        Joint.GetComponent<Rigidbody>().position = _grabbedJoint.transform.position;
                }
            }
        }      
    }

    /// <summary>
    /// Calculates initial distance from head to all joints, which is used for checking if the fish being stretched.
    /// The values are placed in the distances list, and values are considered the natural/intended distance from head to each joint.
    /// In other words, a "bone" in the fish's spine should not surpass this length by much, otherwise the fish will stretch.
    /// </summary>
    public void InitialJointDistances()
    {
        if (_distances.Count == 0)
        {
            foreach (WhichJointGrabbed Joint in GetComponentsInChildren<WhichJointGrabbed>())
            {
                if (Joint.gameObject.name != "Head")
                    _distances.Add(Vector3.Distance(Head.transform.position, Joint.transform.position));
            }
        }
    }

    /// <summary>
    /// Called by fish joints' OnGrab event
    /// </summary>
    public void IncrementGrabCount() => _grabCount++;


    /// <summary>
    /// Called by fish joints' OnGrab event
    /// </summary>
    public void DecrementGrabCount() => _grabCount--;


    /// <summary>
    /// Calculates distance between head and all other joints.
    /// Returns true if distance is greater than what was calculated in InitialJointDistances() by a certain threshold, otherwise returns false.
    /// </summary>
    /// <returns></returns>
    private bool JointDistancesTooGreat()
    {
        if (_distances.Count < 8)
            return false;

        int i = 0;
        foreach (WhichJointGrabbed Joint in GetComponentsInChildren<WhichJointGrabbed>())
        {
            if (i != 0)
            {
                float distance = Vector3.Distance(Head.transform.position, Joint.transform.position);

                if (distance >= _distances[i - 1] + StretchLimit)
                    return true;
            }
            i++;
        }
        return false;
    }

    /// <summary>
    /// Tell that a joint collides with some object that should disable certain corrections, like the sticky surface of the sorting maching.
    /// </summary>
    public void JointCollisionIncrease() => _fishCollides++;


    /// <summary>
    /// Tell that a joint no longer collides with some object that should disable certain corrections, like the sticky surface of the sorting maching.
    /// </summary>
    public void JointCollisionDecrease() => _fishCollides--;
}