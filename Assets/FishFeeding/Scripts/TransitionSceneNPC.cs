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
        bool validDestination = true;
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
        else
            validDestination = false;

        // Check if the player's given answer is a valid destination scene before passing it to SceneController
        // Prevents SceneController from attempting to load non-existent scenes. 
        if (validDestination)
            GetComponent<SceneController>().SceneName = destinationScene;
    }

    private void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= ButtonSpawner_OnAnswer;
    }
}