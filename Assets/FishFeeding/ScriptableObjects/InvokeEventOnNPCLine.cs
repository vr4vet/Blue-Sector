using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeEventOnNPCLine : MonoBehaviour
{
    [SerializeField] private string NPCName;
    private GameObject _npc;
    private DialogueBoxController _dialogueBoxController;

    public UnityEvent m_OnDialogueLine;
    // Start is called before the first frame update
    void Start()
    {
        if (m_OnDialogueLine == null)
            m_OnDialogueLine = new UnityEvent();
        
        _npc = GetComponent<NPCSpawner>()._npcInstances.Find(x => x.name.Contains(NPCName)); // look for NPC with name containing provided string in spawner list
        if (_npc == null)
            Debug.LogError("Could not find NPC with name containing '" + NPCName + "'!");

        _dialogueBoxController = _npc.GetComponent<DialogueBoxController>();

        _dialogueBoxController.m_DialogueChanged.AddListener(OnDialogueChanged);
        
    }

    private void OnDialogueChanged(string npcName, string dialogueTreeName, int section, int index) => m_OnDialogueLine.Invoke();
}
