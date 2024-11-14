using UnityEngine;

public class WhichJointGrabbed : MonoBehaviour
{
    public bool Grabbed = false;
    [SerializeField] private DropWhenStretched WhenStretched;
    public void SetGrabbedJoint()
    {
        Grabbed = true;
    }

    public void RemoveGrabbedJoint()
    {
        Grabbed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SortingSquare"))
            WhenStretched.JointCollisionIncrease();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SortingSquare"))
            WhenStretched.JointCollisionDecrease();
    }
}
