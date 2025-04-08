using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Questionnaire : MonoBehaviour
{
    [SerializeField] private DialogueTree questionnaireDialogueTree;
    [SerializeField] private DialogueTree welcomeReceptionDialogueTree;
    private List<string> answers = new();
    private NPCSpawner _npcSpawner;
    private GameObject _receptionistNpc;
    private NPCMetadata metadata;
    private DialogueTree[] receptionistDialogueTrees;
    private DialogueBoxController _dialogueBoxController;
    private ConversationController _conversationController;


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
        
        _dialogueBoxController = GetComponent<DialogueBoxController>();
        _conversationController = GetComponentInChildren<ConversationController>();
        ButtonSpawner.OnAnswer += Questionnaire_OnAnswer;
        DialogueBoxController.OnDialogueEnded += DestroyQuestionnaire;
   
        Debug.Log("Collecting answers from the questionnaire.");
        Debug.Log(_receptionistNpc.name);
    }

    private void Questionnaire_OnAnswer(string answer, string question, string name) {
        if (name=="Receptionist Rachel")
        {
            if (question == "Would you like to answer the questionnaire?" && answer == "No") 
            {
                Debug.Log("Hei");
            }
                
            answers.Add(answer);
            Debug.Log($"{answer}, {question}, {name}");
        }
    }

    void DestroyQuestionnaire(string name) {

        ReceptionistQuestionnaireState.questionnaireEnded = true;
        NextDialogueTree();
        ButtonSpawner.OnAnswer -= Questionnaire_OnAnswer;
        Debug.Log($"Questionnaire ended: {name}, returning to regular reception dialogue. state: {ReceptionistQuestionnaireState.questionnaireEnded}");
        Destroy(this);
    }

    private void NextDialogueTree() {
        Debug.Log(welcomeReceptionDialogueTree);
        Debug.Log(metadata.npcData.NameOfNPC);
        _dialogueBoxController.StartDialogue(welcomeReceptionDialogueTree, 0, metadata.npcData.NameOfNPC, 0);
    }
    
}