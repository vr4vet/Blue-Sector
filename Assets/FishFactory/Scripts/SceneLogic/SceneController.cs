using System.Collections;
using System.Collections.Generic;
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
        // This can be turned off by setting requirements to false
        if (!GameManager.instance.HSERoomCompleted)
        {
            GameManager.instance.PlaySound("incorrect");
            return;
        }
        SceneManager.LoadScene(scene);
    }
}
