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
    private DialogueBoxController _dialogueBoxController;
    private ConversationController _conversationController;

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
        _actionManager = ActionManager.Instance;

        // Find NPC "Receptionist Rachel"
        _receptionistNpc = _npcSpawner._npcInstances[0];
        _dialogueBoxController = _receptionistNpc.GetComponent<DialogueBoxController>();
        _conversationController = GetConversationController(_receptionistNpc);
        Debug.Log("DialogueBoxController found: " + _dialogueBoxController);
        Debug.Log("ConversationController found: " + _conversationController);

        
        DialogueTree activeDialogueTree = null;
        if (_actionManager.GetToggleBool() == false)
        {
            Debug.Log("Changing dialogue tree to WelcomeReception");
            activeDialogueTree = GetDialogueTreeFromName("WelcomeReception");
            ChangeDialogueTree(activeDialogueTree);
        }
        else
        {
            activeDialogueTree = GetDialogueTreeFromName("QuestionnaireDialogueTree");
            ChangeDialogueTree(activeDialogueTree);
            _userInfo = new List<string>();
            ButtonSpawner.OnAnswer += Questionnaire_OnAnswer;
            DialogueBoxController.OnDialogueEnded += Questionnaire_OnDialogueEnded;
        }
        
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
    /// Changes dialogue tree list of NPC receptionist Rachel.
    /// </summary>
    /// <param name="dialogueTree">The dialogue tree asset to be set as NPC's DialogueTreeSO.</param>
    private void ChangeDialogueTree(DialogueTree dialogueTree)
    {
        _conversationController.SetDialogueTreeList(dialogueTree);

    }

    /// <summary>
    /// Gets the ConversationController of the NPC.
    /// </summary>
    /// <param name="npc">The NPC that the ConversationController belongs to </param>
    /// <returns>ConversationController</returns>
    private ConversationController GetConversationController(GameObject npc)
    {
        if (npc == null)
        {
            Debug.LogError("No NPCSpawner found");
            return null;
        }
        else 
        {
            // Finds the ConversationController in the Hierarchy
            GameObject collisionTriggerHandler = npc.transform.Find("CollisionTriggerHandler").gameObject;
            ConversationController conversationController = collisionTriggerHandler.GetComponent<ConversationController>();
            return conversationController;

        }
        
    }


    /// <summary>
    /// Gets the specified DialogueTree from NPC Receptionist Rachel's ConversationController.
    /// </summary>
    /// <param name="name">Name of the DialogueTree to be retrieved</param>
    /// <returns>DialogueTree</returns>
    private DialogueTree GetDialogueTreeFromName(string name)
    {
        DialogueTree returnTree = null;

        foreach (DialogueTree tree in _conversationController.GetDialogueTrees())
        {
            if (tree.name.Equals(name))
                returnTree = tree;
        }

        if (returnTree != null)
            return returnTree;
        return null;
    }

    /// <summary>
    /// Stops listening for questionnaire answers when the user is no longer in Reception scene.
    /// Stops listening to ended dialogue with NPC Receptionist Rachel when the user is no longer in Reception scene.
    /// </summary>
    void OnDestroy()
    {
        //if (_actionManager.GetToggleBool() == false)
        //{
        //    ButtonSpawner.OnAnswer -= Questionnaire_OnAnswer;
        //    DialogueBoxController.OnDialogueEnded -= Questionnaire_OnDialogueEnded;
        //}
        ButtonSpawner.OnAnswer -= Questionnaire_OnAnswer;
        DialogueBoxController.OnDialogueEnded -= Questionnaire_OnDialogueEnded;
    }

}