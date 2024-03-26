using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class SpecializedNPCBehavior : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;

    [HideInInspector] private NavMeshAgent _agent;
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _velocityYHash;
    private ConversationController _conversationController;
    [SerializeField] private GameObject dialogueTransitions;
    [SerializeField] private List<List<Transform>> paths;
    private int currentPosition;
    private int currentPath;
    private bool walking;


    // Start is called before the first frame update
    void Start()
    {
        _npcSpawner = GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }

        _npc = _npcSpawner._npcInstances[1];

        if (NPCToPlayerReferenceManager.Instance == null)
        {
            Debug.LogError("NPCManager instance was not found. Please ensure it exists in the scene.");
            return;
        }

        _agent = _npc.GetComponent<NavMeshAgent>();
        _animator = _npc.GetComponentInChildren<Animator>();
        _velocityYHash = Animator.StringToHash("VelocityY");
        if (_agent == null)
        {
            Debug.LogError("THe NPC is missing the NavMeshAgent");
            return;
        }
        if (_animator == null)
        {
            Debug.LogError("THe NPC is missing the Animator");
            return;
        }

        if (_npc != null)
        {
            _conversationController = _npc.GetComponentInChildren<ConversationController>();
        }

        paths = new List<List<Transform>>();
        foreach (Transform child in dialogueTransitions.transform)
        {
            List<Transform> path = new List<Transform>();
            foreach (Transform grandchild in child)
            {
                path.Add(grandchild);
            }
            paths.Add(path);
        }

        DialogueBoxController.OnDialogueEnded += DialogueTransition;

        currentPosition = 0;
        currentPath = -1;

        walking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator != null) 
        {
            _animator.SetFloat(_velocityYHash, _agent.velocity.magnitude);
            Debug.Log(_agent.remainingDistance);
            if (walking)
            {
                if (currentPath == -1) { return; }
                NavMeshPath path = new NavMeshPath();
                _agent.CalculatePath(paths[currentPath][currentPosition].position, path);
                _agent.SetPath(path);
                if (_agent.remainingDistance < 0.01f)
                {
                    currentPosition++;
                    if (currentPosition == paths[currentPath].Count)
                    {
                        walking = false;
                        currentPosition = 0;
                        _conversationController.NextDialogueTree();
                    }
                } 
            }
        }
        else {
            _animator = _npc.GetComponentInChildren<Animator>();
        }
    }

    public void DialogueTransition(string name) 
    {
        if (name != _npc.name) { return; }
        currentPath++;
        if (currentPath >= paths.Count) { return; }
        walking = true;
    }

    void OnDestroy()
    {
        DialogueBoxController.OnDialogueEnded -= DialogueTransition;
    }

}
