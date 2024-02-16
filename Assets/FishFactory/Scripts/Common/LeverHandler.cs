using UnityEngine;
using UnityEngine.Events;

public class LeverHandler : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onAction;
    [SerializeField]
    private Transform handle;
    private bool toggle = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onAction.Invoke();
            handle.Rotate(new Vector3(toggle ? 60 : -60, 0, 0));
            toggle = !toggle;
        }
    }
}
