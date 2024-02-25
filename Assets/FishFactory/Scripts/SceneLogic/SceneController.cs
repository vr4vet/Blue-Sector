using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField][Tooltip("The name of the scene to change to")]
    private string sceneName;
    
    [SerializeField][Tooltip("GameObjects that will trigger the scene change. This should be colliders on grabbers from BNG framework")]
    private List<Collider> triggers;


    private void OnTriggerEnter(Collider collidedObject)
    {
        foreach (Collider obj in triggers)
        {
            if (obj == collidedObject)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
