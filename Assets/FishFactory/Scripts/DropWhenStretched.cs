using UnityEngine;
using System.Collections.Generic;
using BNG;
using System.Linq;

public class DropWhenStretched : MonoBehaviour
{
    [SerializeField] private Transform Head;
    [SerializeField] private Transform TailEnd;
    [SerializeField] private GameObject Armature;
    private GameObject FishHead;
    private GameObject GrabbedJoint;
    private List<float> distances = new();
    private int GrabCount = 0;
    private int FishCollides = 0;
    private Grabber playerGrabberLeft, playerGrabberRight;

    private float WaitBeforeDrop = 0;
    // Update is called once per frame

    private void Awake()
    {
        DistanceLength();
    }

    private float _greatestVelocity = 0;
    void Update()
    {
        // getting the player hand grabbers. they are not immediately accessible, so they are fetched here
        if (playerGrabberLeft == null || playerGrabberRight == null)
        {
            List<Grabber> grabbers = GameManager.Instance.GetPlayerRig().GetComponentsInChildren<Grabber>().ToList();
            playerGrabberLeft = grabbers.Find(x => x.transform.parent.name == "LeftController");
            playerGrabberRight = grabbers.Find(x => x.transform.parent.name == "RightController");
            return;
        }

        // return when fish is not grabbed, or if held with one hand while touching the sorting machine's sticky surface. prevents unneccesary calculations and unwanted anti-stretch corrections
        if (GrabCount == 0 || FishCollides > 0 && GrabCount < 2)
            return;

        // make anti-stretch corrections if fish is currently stretched and enough time has passed since last correction. waiting prevents wild and rapid corrections
        if (DistancesBetweenJoints() && Time.time - WaitBeforeDrop >= 1f)
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
    /// Calculates distance from head to all joints, which is used for checking if the fish being stretched
    /// </summary>
    public void DistanceLength()
    {
        if (distances.Count == 0)
        {
            foreach (WhichJointGrabbed Joint in GetComponentsInChildren<WhichJointGrabbed>())
            {
                if (Joint.gameObject.name == "Head")
                    FishHead = Joint.gameObject;
                
                else if (Head != null)
                    distances.Add(Vector3.Distance(FishHead.transform.position, Joint.transform.position));
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
    /// Returns true if distance is greater by a certain threshold, otherwise returns false.
    /// </summary>
    /// <returns></returns>
    private bool DistancesBetweenJoints()
    {
        if (distances.Count < 8)
            return false;

        int i = 0;
        foreach (WhichJointGrabbed Joint in GetComponentsInChildren<WhichJointGrabbed>())
        {
            if (i != 0)
            {
                float DistanceRetrieval = Vector3.Distance(Armature.transform.GetChild(0).transform.position, Joint.transform.position);

                if (DistanceRetrieval >= distances[i - 1] + 0.15f)
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