using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Questionnaire : MonoBehaviour
{
    private List<string> _userInfo;
    private NPCSpawner _npcSpawner;
    private GameObject _receptionistNpc;
    private ActionManager _actionManager;

    /// <summary>
    /// NPC Receptionist Rachel will ask the user questions about themselves.
    /// The information will be sent to ActionManager, and the LLM in Chat-Service will make use of it.
    /// </summary>
    void Start()
    {
        _npcSpawner = GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }

        // Find NPC "Receptionist Rachel"
        _receptionistNpc = _npcSpawner._npcInstances[0];
        _userInfo = new List<string>();
        _actionManager = ActionManager.Instance;
        ButtonSpawner.OnAnswer += Questionnaire_OnAnswer;
        DialogueBoxController.OnDialogueEnded += Questionnaire_OnDialogueEnded;
    }

    /// <summary>
    /// This method is called when the user answers a question in NPC Receptionist Rachel's dialogue tree.
    /// Collects the user information that will be stored in ActionManager.
    /// </summary>
    /// <param name="answer"> The value of the button that the user presses</param>
    /// <param name="question"> The question connected to the answer </param>
    /// <param name="name"> The name of the NPC whose dialogue button was clicked </param>
    private void Questionnaire_OnAnswer(string answer, string question, string name)
    {
        // Only listen to when the buttons belonging to Receptionist Rachel's dialogue tree are pressed
        if (name == _receptionistNpc.name)
        {
            bool questionExists = false;

            // Iterate through the list to check if the question already exists
            for (int i = 0; i < _userInfo.Count; i++)
            {
                if (_userInfo[i].StartsWith($"{question}:"))
                {
                    // Update the answer to the existing question with the new answer
                    _userInfo[i] = $"{question}: {answer}";
                    questionExists = true;
                    break;
                }
            }

            // If the question doesn't exist, add it to the list
            if (!questionExists)
            {
                _userInfo.Add($"{question}: {answer}");
            }
        }
    
    }

    /// <summary>
    /// This method is called when the dialogue with NPC Receptionist Rachel ends.
    /// Sets the user information in ActionManager.
    /// </summary>
    /// <param name="name"></param>
    private void Questionnaire_OnDialogueEnded(string name)
    {
        // Only listen to when the buttons belonging to Receptionist Rachel's dialogue tree are pressed
        if (name == _receptionistNpc.name)
        {
            _actionManager.SetUserInfo(_userInfo);
            Debug.Log("Sending answers to ActionManager");
        }
    }

    /// <summary>
    /// Stops listening for questionnaire answers when the user is no longer in Reception scene.
    /// Stops listening to ended dialogue with NPC Receptionist Rachel when the user is no longer in Reception scene.
    /// </summary>
    void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= Questionnaire_OnAnswer;
        DialogueBoxController.OnDialogueEnded -= Questionnaire_OnDialogueEnded;
    }

}