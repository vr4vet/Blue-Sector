using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Questionnaire : MonoBehaviour
{
    private Dictionary<string, string> _userInfo;
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
        _userInfo = new Dictionary<string, string>();
        _actionManager = ActionManager.Instance;
        ButtonSpawner.OnAnswer += Questionnaire_OnAnswer;
    }

    /// <summary>
    /// This method is called when the user answers a question in NPC Receptionist Rachel's dialogue tree.
    /// </summary>
    /// <param name="answer"> The value of the button that the user presses</param>
    /// <param name="question"> The question connected to the answer </param>
    /// <param name="name"> The name of the NPC whose dialogue button was clicked </param>
    private void Questionnaire_OnAnswer(string answer, string question, string name)
    {
        // Only listen to when the buttons belonging to Receptionist Rachel's dialogue tree are pressed
        if (name == _receptionistNpc.name)
        {
            if (_userInfo.ContainsKey(question))
            {
                _userInfo[question] = answer;
                Debug.Log($"Answer to question {question} changed: {answer}");
            }

            else if (question == "Are you satisfied with your answers?" && answer == "Yes")
            {
                _actionManager.SetUserInfo(_userInfo);
                Debug.Log("Sending answers to ActionManager");
            }

            else
            {
                _userInfo.Add(question, answer);
                Debug.Log($"{answer}, {question}, {name}");

            }

        }
    }

    /// <summary>
    /// Stop listening for questionnaire answers when the user is no longer in Reception scene.
    /// </summary>
    void OnDestroy()
    {
        ButtonSpawner.OnAnswer -= Questionnaire_OnAnswer;
    }

}