using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private string sceneName;
    [SerializeField][Tooltip("GameObjects that will trigger the scene change")]
    private List<Collider> triggers;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered me");
        foreach (Collider obj in triggers)
        {
            if (obj == other)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
