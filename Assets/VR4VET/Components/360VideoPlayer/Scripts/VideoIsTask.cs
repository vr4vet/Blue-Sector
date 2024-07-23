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
    public MaintenanceManager gameManager;

    [SerializeField]
    public Task.Subtask subtask;

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
