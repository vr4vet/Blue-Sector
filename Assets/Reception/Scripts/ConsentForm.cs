using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class ConsentForm : MonoBehaviour
{
    private NPCSpawner _npcSpawner;
    private GameObject _aiConsentNpc;
    private ActionManager _actionManager;

    // Start is called before the first frame update
    void Start()
    {
        _npcSpawner = GetComponent<NPCSpawner>();
        _actionManager = ActionManager.Instance;
  
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }

        _aiConsentNpc = _npcSpawner._npcInstances[2];

        // Moves the dialogue canvas to the receptionist higher
        Transform dialogueCanvas = _aiConsentNpc.transform.GetChild(1);
        dialogueCanvas.localPosition = new Vector3(dialogueCanvas.localPosition.x, 1.68f, dialogueCanvas.localPosition.z);
        ButtonSpawner.OnAnswer += ConsentForm_OnAnswer;
        DialogueBoxController.OnDialogueEnded += ConsentForm_SetAIFeatures;
    }

    private void ConsentForm_OnAnswer(string answer, string question, string name)
    {
        if (name == _aiConsentNpc.name)
        {
            if (answer == "Yes") 
            { 
                _actionManager.SetToggleBool(true); 
                Debug.Log(name + " has consented to AI features");
            }
            else 
            { 
                _actionManager.SetToggleBool(false);
                Debug.Log(name + " has not consented to AI features");
            }
        }
    }

    private void ConsentForm_SetAIFeatures(string name)
    {
        if (name == _aiConsentNpc.name)
        {
            Debug.Log("Before reloading scene!");
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
            Debug.Log("Reloading scene to apply AI features");
        }
    }

    private void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= ConsentForm_OnAnswer;
        DialogueBoxController.OnDialogueEnded -= ConsentForm_SetAIFeatures;
    }

}