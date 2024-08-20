using System;
using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEngine;
using UnityEngine.Events;

public class VideoIsTask : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> objectsActivatingAfterVideo = new List<GameObject>();

    [SerializeField]
    public Step step;

    [SerializeField]
    public UnityEvent EventOnVideoPlay;

    [SerializeField]
    public UnityEvent EventOnStepCompleted;

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
        if (step)
        {
            if (step.IsCompeleted())
            {
                EventOnStepCompleted.Invoke();
            }
        }  
    }
}
