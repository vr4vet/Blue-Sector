using UnityEngine;
using System.Collections.Generic;
using BNG;
using System.Linq;

public class DropWhenStretched : MonoBehaviour
{
    [SerializeField] private Transform Head;
    [SerializeField] private GameObject Armature;
    [SerializeField] private float StretchLimit = .15f;
    private GameObject GrabbedJoint;
    private List<float> distances = new();
    private int GrabCount = 0;
    private int FishCollides = 0;
    private Grabber playerGrabberLeft, playerGrabberRight;

    private float WaitBeforeDrop = 0;
    // Update is called once per frame

    private void Awake()
    {
        if (Head == null)
            Debug.LogError("Head field is null! This field should be set to the armature's head joint!");

        if (Armature == null)
            Debug.LogError("Armature field is null! Field should be set to fish object's armature!");

        InitialJointDistances();
    }

    void Update()
    {
        // getting the player hand grabbers. they are not immediately accessible, so they are fetched here
        if (playerGrabberLeft == null || playerGrabberRight == null)
        {
            GameObject playerRig = GameManager.Instance.GetPlayerRig();
            if (playerRig != null)
            {
                List<Grabber> grabbers = playerRig.GetComponentsInChildren<Grabber>().ToList();
                playerGrabberLeft = grabbers.Find(x => x.transform.parent.name == "LeftController");
                playerGrabberRight = grabbers.Find(x => x.transform.parent.name == "RightController");
            }
            return;
        }

        // return when fish is not grabbed, or if held with one hand while touching the sorting machine's sticky surface. prevents unneccesary calculations and unwanted anti-stretch corrections
        if (GrabCount == 0 || FishCollides > 0 && GrabCount < 2)
            return;

        // make anti-stretch corrections if fish is currently stretched and enough time has passed since last correction. waiting prevents wild and rapid corrections
        if (JointDistancesTooGreat() && Time.time - WaitBeforeDrop >= 1f)
        {
            WaitBeforeDrop = Time.time;
            if (GrabCount == 2) // select random hand and make it drop its held fish joint
            {
                if (Random.value >= 0.5)
                    playerGrabberLeft.TryRelease();
                else
                    playerGrabberRight.TryRelease();
                return;
            }
                
            if (GrabCount == 1) // find which joint the player is holding and move all joints towards it. prevents fish from latching into objects and becoming stretched when player moves held joint
            {
                foreach (WhichJointGrabbed Joint in GetComponentsInChildren<WhichJointGrabbed>())
                {
                    if (Joint.Grabbed)
                    {
                        GrabbedJoint = Joint.gameObject;
                        break;
                    }
                }

                foreach (WhichJointGrabbed Joint in GetComponentsInChildren<WhichJointGrabbed>())
                {
                    if (!Joint.Grabbed)
                        Joint.transform.position = GrabbedJoint.transform.position;
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
        if (distances.Count == 0)
        {
            foreach (WhichJointGrabbed Joint in GetComponentsInChildren<WhichJointGrabbed>())
            {
                if (Joint.gameObject.name != "Head")
                    distances.Add(Vector3.Distance(Head.transform.position, Joint.transform.position));
            }
        }
    }

    /// <summary>
    /// Called by fish joints' OnGrab event
    /// </summary>
    public void IncrementGrabCount() => GrabCount++;


    /// <summary>
    /// Called by fish joints' OnGrab event
    /// </summary>
    public void DecrementGrabCount() => GrabCount--;


    /// <summary>
    /// Calculates distance between head and all other joints.
    /// Returns true if distance is greater than what was calculated in InitialJointDistances() by a certain threshold, otherwise returns false.
    /// </summary>
    /// <returns></returns>
    private bool JointDistancesTooGreat()
    {
        if (distances.Count < 8)
            return false;

        int i = 0;
        foreach (WhichJointGrabbed Joint in GetComponentsInChildren<WhichJointGrabbed>())
        {
            if (i != 0)
            {
                float distance = Vector3.Distance(Armature.transform.GetChild(0).transform.position, Joint.transform.position);

                if (distance >= distances[i - 1] + StretchLimit)
                    return true;
            }
            i++;
        }
        return false;
    }

    /// <summary>
    /// Tell that a joint collides with some object that should disable certain corrections, like the sticky surface of the sorting maching.
    /// </summary>
    public void JointCollisionIncrease() => FishCollides++;


    /// <summary>
    /// Tell that a joint no longer collides with some object that should disable certain corrections, like the sticky surface of the sorting maching.
    /// </summary>
    public void JointCollisionDecrease() => FishCollides--;
}