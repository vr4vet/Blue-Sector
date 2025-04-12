using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class Questionnaire : MonoBehaviour
{
    private Dictionary<String, String> _answers;
    private NPCSpawner _npcSpawner;
    private GameObject _receptionistNpc;
    private ActionManager actionManager;

    /// <summary>
    /// Receptionist Rachel will ask the user questions about themselves.
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

        actionManager = ActionManager.Instance;
        _answers = new Dictionary<String, String>();

        // Find NPC "Receptionist Rachel"
        _receptionistNpc = _npcSpawner._npcInstances[0];
        ButtonSpawner.OnAnswer += Questionnaire_OnAnswer;
    }

    /// <summary>
    /// This method is called when the user answers a question in NPC Receptionist Rachel's dialogue tree.
    /// </summary>
    /// <param name="answer"> The value of the button that the user presses</param>
    /// <param name="question"> The question connected to the answer </param>
    /// <param name="name"> The name of the NPC whose dialogue button was clicked </param>
    private void Questionnaire_OnAnswer(String answer, String question, String name) {
        // Only listen to when the buttons belonging to Receptionist Rachel's dialogue tree are pressed
        if (name==_receptionistNpc.name)
        {
            if (_answers.ContainsKey(question))
            {
                _answers[question] = answer;
                Debug.Log($"Answer to question {question} changed: {answer}");
                var asString = string.Join(Environment.NewLine, _answers.Select(kv => $"{kv.Key}: {kv.Value}"));
                Debug.Log(asString);
            }

            else if (question == "Are you satisfied with your answers?" && answer == "Yes")
            {
                actionManager.SetUserInfo(_answers);
                var asString = string.Join(Environment.NewLine, _answers.Select(kv => $"{kv.Key}: {kv.Value}"));
                Debug.Log($"Answers to string: {asString}");
                Debug.Log("Sending answers to ActionManager");
            }

            else
            {
                _answers.Add(question, answer);
                Debug.Log($"{answer}, {question}, {name}");
                var asString = string.Join(Environment.NewLine, _answers.Select(kv => $"{kv.Key}: {kv.Value}"));
                Debug.Log(asString);


            }
            // Listen to the end of the questionnaire
            
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