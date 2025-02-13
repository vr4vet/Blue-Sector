using UnityEngine;

public class WhichJointGrabbed : MonoBehaviour
{
    [HideInInspector] public bool Grabbed = false;
    [SerializeField] private DropWhenStretched DropWhenStretchedScript;

    public void SetGrabbedJoint() => Grabbed = true;

    public void UnsetGrabbedJoint() => Grabbed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SortingSquare"))
            DropWhenStretchedScript.JointCollisionIncrease();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SortingSquare"))
            DropWhenStretchedScript.JointCollisionDecrease();
    }
}
