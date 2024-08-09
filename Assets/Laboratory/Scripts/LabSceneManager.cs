using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LabSceneManager : MonoBehaviour
{
    public UnityEvent unityEvent;
    private void OnTriggerEnter(Collider other) {
        unityEvent.Invoke();
        SceneManager.LoadScene("Laboratory");
    }
}
