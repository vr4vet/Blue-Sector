using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    private List<DestroyedListener> listeners = new List<DestroyedListener>();

    public void UpdateListeners()
    {
        foreach (DestroyedListener listener in listeners)
        {
            listener.Destroyed(gameObject);
        }
    }

    public void AddListener(DestroyedListener listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }
}
