using BNG;
using UnityEngine;

public class TransitionSceneNPC : MonoBehaviour
{
    string destinationScene;

    // Start is called before the first frame update
    void Start()
    {
        ButtonSpawner.OnAnswer += ButtonSpawner_OnAnswer;
    }

    private void ButtonSpawner_OnAnswer(string answer)
    {
        if (answer == "Welfare station")
        {
            destinationScene = "FishWelfare";
        } 
        else if (answer == "Fish factory")
        {
            destinationScene = "HSERoom";
        }
        else if (answer == "Laboratory")
        {
            destinationScene = "Laboratory";
        }
        else if (answer == "Maintenance boat" || answer == "Fish maintenance")
        {
            destinationScene = "FishMaintenance";
        }
        else if (answer == "Feeding station")
        {
            destinationScene = "FishFeeding";
        }
        else if (answer == "Reception area")
        {
            destinationScene = "ReceptionOutdoor";
        }

        GetComponent<SceneController>().SceneName = destinationScene;
    }

    public void ChangeScene()
    {
        if (destinationScene != null)
        {
            GetComponent<AudioSource>().Play();
            //GetComponent<SceneLoader>().LoadScene(destinationScene);
            //GetComponent<SceneController>().SceneName = destinationScene;
        }
    }

    private void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= ButtonSpawner_OnAnswer;
    }
}