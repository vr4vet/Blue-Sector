using BNG;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [Header("Scene Controller")]
    [Tooltip("The name of the scene to change to")]
    [SerializeField]
    private string sceneName;

    /// <summary>
    /// When the player collides with the scene controller, change the scene
    /// </summary>
    /// <param name="collisionObject">The player collider</param>
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.gameObject.name == "Grabber")
        {
            ChangeScene(sceneName);
        }
    }

    /// <summary>
    /// Change the scene. If the player is currently in the HSE room the requirements has to be fulfilled first.
    /// </summary>
    /// <param name="scene">The name of the scene to change to</param>
    public void ChangeScene(string scene)
    {
        if (
            !GameManager.Instance.HSERoomCompleted
            && SceneManager.GetActiveScene().name == "HSERoom"
        )
        {
            GameManager.Instance.PlaySound("incorrect");
            return;
        }

        GameManager.Instance.PlaySound("door");
        SceneManager.LoadScene(scene);
    }
}
