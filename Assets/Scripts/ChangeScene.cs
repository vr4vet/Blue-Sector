using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    private void Start()
    {
    }

    public void ChangeByIndex(int sceneIndex)
    {
        Time.timeScale = 1;
        StartCoroutine("LoadAsyncScene", sceneIndex);
    }

    IEnumerator LoadAsyncScene(int sceneIndex)
    {
        // The Application loads the Scene in the background as the current Scene runs.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
