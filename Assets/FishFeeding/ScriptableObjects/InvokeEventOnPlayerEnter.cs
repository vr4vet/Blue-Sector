using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeEventOnPlayerEnter : MonoBehaviour
{
    public UnityEvent m_OnPlayerEnter;

    private void Start()
    {
        if (m_OnPlayerEnter == null)
            m_OnPlayerEnter = new();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            m_OnPlayerEnter.Invoke();
    }
}
