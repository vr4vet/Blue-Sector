using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VideoIsTask : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> objectsActivatingAfterVideo = new List<GameObject>();

    [SerializeField]
    public UnityEvent EventOnVideoPlay;

    public void activateGameObjects()
    {
        foreach (GameObject gameObject in objectsActivatingAfterVideo)
        {
            gameObject.SetActive(true);
        }
    }

    internal void invokeEventOnVideoPlay()
    {
        EventOnVideoPlay.Invoke();
    }
}
