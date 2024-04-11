using System.Collections;
using System.Collections.Generic;
using BNG;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    [SerializeField]
    [Tooltip("GameObjects that will trigger the scene change")]
    private List<Collider> triggers;

    private void OnTriggerEnter(Collider collisionObject)
    {
        foreach (Collider obj in triggers)
        {
            if (obj == collisionObject)
            {
                ChangeScene(sceneName);
            }
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
