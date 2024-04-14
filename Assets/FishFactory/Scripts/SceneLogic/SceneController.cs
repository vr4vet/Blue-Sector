using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.gameObject.name == "Grabber")
        {
            ChangeScene(sceneName);
        }
    }

    public void ChangeScene(string scene)
    {
        // Check if the player has met the requirements to move to the next scene
        if (
            !GameManager.instance.HSERoomCompleted
            && SceneManager.GetActiveScene().name == "HSERoom"
        )
        {
            GameManager.instance.PlaySound("incorrect");
            return;
        }
        SceneManager.LoadScene(scene);
    }
}
