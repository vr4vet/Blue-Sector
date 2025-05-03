using UnityEngine;
using UnityEngine.SceneManagement;



public class ConsentForm : MonoBehaviour
{
    private NPCSpawner _npcSpawner;
    private GameObject _aiConsentNpc;
    private ActionManager _actionManager;
    private bool _isEnabled;

    void Start()
    {
        _npcSpawner = GetComponent<NPCSpawner>();
        _actionManager = ActionManager.Instance;
        _isEnabled = _actionManager.GetToggleBool();
  
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }

        _aiConsentNpc = _npcSpawner.NpcInstances[2];

        // Moves the dialogue canvas to the receptionist higher
        Transform dialogueCanvas = _aiConsentNpc.transform.GetChild(1);
        dialogueCanvas.localPosition = new Vector3(dialogueCanvas.localPosition.x, 1.68f, dialogueCanvas.localPosition.z);
        ButtonSpawner.OnAnswer += ConsentForm_OnAnswer;
        DialogueBoxController.OnDialogueEnded += ConsentForm_SetAIFeatures;
    }

    /// <summary>
    /// Listens to player's answer to the AI consent question.
    /// </summary>
    /// <param name="answer"></param>
    /// <param name="question"></param>
    /// <param name="name"></param>
    private void ConsentForm_OnAnswer(string answer, string question, string name)
    {
        if (name == _aiConsentNpc.name)
        {
            if (answer == "Yes") 
            { 
                _isEnabled = true; 
                Debug.Log("User has consented to AI features");
            }
            else 
            { 
                _isEnabled = false;
                Debug.Log("User has not consented to AI features");
            }
        }
    }

    private void ConsentForm_SetAIFeatures(string name)
    {
        AISceneController sceneController = _npcSpawner.GetComponent<AISceneController>();
        if (name == _aiConsentNpc.name)
        {
            // Check if AI features are already set to preferred state
            // If they are, do nothing
            if (_isEnabled == _actionManager.GetToggleBool())
            {
                Debug.Log("AI features are already set to " + _isEnabled);
                return;
            }
            else
            {
                _actionManager.SetToggleBool(_isEnabled);
                Debug.Log("Before reloading scene!");
                Scene currentScene = SceneManager.GetActiveScene();
                StartCoroutine(sceneController.LoadSceneWithLoadingScreen(currentScene.name));
                Debug.Log("Reloading scene to apply AI features");
                
            }
            
        }
    }

    private void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= ConsentForm_OnAnswer;
        DialogueBoxController.OnDialogueEnded -= ConsentForm_SetAIFeatures;
    }

}