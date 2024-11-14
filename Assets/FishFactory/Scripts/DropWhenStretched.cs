using UnityEngine;
using System.Collections.Generic;

public class DropWhenStretched : MonoBehaviour
{
    [SerializeField] private Transform Head;
    [SerializeField] private Transform TailEnd;
    [SerializeField] private GameObject Armature;
    private GameObject FishHead;
    private GameObject GrabbedJoint;
    private float distance = Mathf.Infinity;
    private List<float> distances = new List<float>();
    private DropTheFish drop;
    private int GrabCount = 0;
    private bool FishCollides = false;

    // Start is called before the first frame update
    void Start()
    {
        drop = GameObject.Find("DropObject").GetComponent<DropTheFish>();
    }

    private float WaitBeforeDrop = 0;
    // Update is called once per frame
    void Update()
    { 
        if (GrabCount == 0 || FishCollides)
        {
            return;
        }

        if (DistancesBetweenJoints() && Time.time - WaitBeforeDrop >= 0.5f)
        {
            WaitBeforeDrop = Time.time;
            if (GrabCount == 2)
            {
                if (Random.value >= 0.5)
                    drop.StretchDropLeft.Invoke();
                else
                    drop.StretchDropRight.Invoke();
                return;
            }
                
            if (GrabCount == 1)
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
                    {
                        Joint.transform.position = GrabbedJoint.transform.position;
                    }
                }
            }
        }      
    }
    public void DistanceLength()
    {
        if (distance == Mathf.Infinity)
            distance = Vector3.Distance(Head.position, TailEnd.position);

        if (distances.Count == 0)
        {
            foreach (WhichJointGrabbed Joint in GetComponentsInChildren<WhichJointGrabbed>())
            {
                if (Joint.gameObject.name == "Head")
                    FishHead = Joint.gameObject;
                
                else if(Head != null)
                    distances.Add(Vector3.Distance(FishHead.transform.position, Joint.transform.position));
            }
        }
    }

    public void IncrementGrabCount()
    {
        GrabCount++;
    }

    public void DecrementGrabCount()
    {
        GrabCount--;
    }

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
                {
                    return true;
                }
            }
            i++;
        }
        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("SortingSquare"))
            FishCollides = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("SortingSquare"))
            FishCollides = false;
    }
}