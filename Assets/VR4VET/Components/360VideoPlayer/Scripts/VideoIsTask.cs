using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoIsTask : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> objectsActivatingAfterVideo = new List<GameObject>();

    [SerializeField]
    public MaintenanceManager gameManager;

    [SerializeField]
    public Task.Subtask subtask;

    public void activateGameObjects()
    {
        foreach (GameObject gameObject in objectsActivatingAfterVideo)
        {
            gameObject.SetActive(true);
        }
    }

    public void completeTask()
    {
        gameManager.CompleteStep(subtask.GetStep("Se Video"));
    }
}
