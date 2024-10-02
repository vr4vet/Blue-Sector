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
        } else if (answer == "Fish factory")
        {
            destinationScene = "HSERoom";
        }
        else if (answer == "Laboratory")
        {
            destinationScene = "Laboratory";
        }
        else if (answer == "Maintenance boat")
        {
            destinationScene = "FishMaintenance";
        }
        else if (answer == "Feeding station")
        {
            destinationScene = "FishFeeding";
        }
    }

    public void ChangeScene()
    {
        if (destinationScene != null)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<SceneLoader>().LoadScene(destinationScene);
        }
    }

    private void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= ButtonSpawner_OnAnswer;
    }
}