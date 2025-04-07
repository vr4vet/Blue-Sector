using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Questionnaire : MonoBehaviour
{
    [SerializeField] private DialogueTree questionnaireDialogueTree;
    [SerializeField] private DialogueTree welcomeReceptionDialogueTree;
    private List<string> answers = new List<string>();
    private NPCSpawner _npcSpawner;
    private GameObject _receptionistNpc;
    private NPCMetadata metadata;
    private DialogueTree[] receptionistDialogueTrees;


    void Start()
    {
        if (ReceptionistQuestionnaireState.questionnaireEnded) {
            Debug.Log($"skipping questionnaire: {ReceptionistQuestionnaireState.questionnaireEnded}");
            Destroy(this);
            return;
        }

        _npcSpawner = GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }

        _receptionistNpc = _npcSpawner._npcInstances[0];
        metadata = _receptionistNpc.GetComponent<NPCMetadata>();
        if (metadata != null) {
            receptionistDialogueTrees = metadata.npcData.DialogueTreesSO;
        }
        else {
            Debug.LogError("NPC has no Metadata!");
        }

        insertDialogueTree(questionnaireDialogueTree);
        ButtonSpawner.OnAnswer += questionnaire_OnAnswer;
        DialogueBoxController.OnDialogueEnded += destroyQuestionnaire;
   
        Debug.Log("Collecting answers from the questionnaire.");
        Debug.Log(_receptionistNpc.name);
    }

    private void questionnaire_OnAnswer(string answer) {
        answers.Add(answer);
        Debug.Log($"Answer: {answer} added to ActionManager. state: {ReceptionistQuestionnaireState.questionnaireEnded}");
        answers.ForEach(x => Debug.Log(x));

    }

    void destroyQuestionnaire(string name) {
        ReceptionistQuestionnaireState.questionnaireEnded = true;
        insertDialogueTree(welcomeReceptionDialogueTree);
        ButtonSpawner.OnAnswer -= questionnaire_OnAnswer;
        Debug.Log($"Questionnaire ended: {name}, returning to regular reception dialogue. state: {ReceptionistQuestionnaireState.questionnaireEnded}");
        Destroy(this);
    }

    private void insertDialogueTree(DialogueTree dialogueTree) {
        receptionistDialogueTrees[0] = dialogueTree;
        Debug.Log($"State: {ReceptionistQuestionnaireState.questionnaireEnded}");
    }
    
}