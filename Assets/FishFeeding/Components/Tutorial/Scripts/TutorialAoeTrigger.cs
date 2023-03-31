using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class TutorialAoeTrigger : MonoBehaviour
{
    /// <summary>
    /// Gets an event that is fired when the player enters the box collider.
    /// </summary>
    public UnityEvent OnTriggered;

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnTriggered.Invoke();
        }
    }
}
