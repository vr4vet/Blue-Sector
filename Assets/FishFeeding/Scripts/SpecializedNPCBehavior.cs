using BNG;
using System.Collections;
using System.Collections.Generic;
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
    private BoxCollider boxCollider;
    public GrabbableUnityEvents grabbableUnityEvents;


    // Start is called before the first frame update
    void Start()
    {
        _npcSpawner = GetComponent<NPCSpawner>();
        if (_npcSpawner == null)
        {
            Debug.LogError("No NPCSpawner found");
            return;
        }

        // Adjusts the size of the NPCs
        foreach (GameObject npc in _npcSpawner._npcInstances)
        {
            //npc.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            Transform dialogueCanvas = npc.transform.GetChild(1);
            dialogueCanvas.localPosition = new Vector3(dialogueCanvas.localPosition.x, dialogueCanvas.localPosition.y, 0.55f);
            dialogueCanvas.localScale = new Vector3(0.008f, 0.008f, 0.008f);
        }

        _npc = _npcSpawner._npcInstances[1];

        // Disables capsule collider and adds box collider to NPC
        GameObject collisionTrigger = _npc.transform.GetChild(0).gameObject;
        collisionTrigger.GetComponent<CapsuleCollider>().enabled = false;
        boxCollider = collisionTrigger.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.center = new Vector3(-0.415f, 0f, 1.3f);
        boxCollider.size = new Vector3(3.467f, 2f, 2.67f);

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

        grabbableUnityEvents.onGrab.AddListener(SetFalseNPCTrigger);
        grabbableUnityEvents.onRelease.AddListener(SetTrueNPCTrigger);

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
            //Debug.Log(_agent.remainingDistance);
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

                        // Adjusts the size of the box collider when the NPC is in the control room
                        if (currentPath == 0)
                        {
                            boxCollider.center = new Vector3(-0.11f, 0f, 1.6f);
                            boxCollider.size = new Vector3(3.2f, 2f, 3.7f);
                        }
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

    private void SetFalseNPCTrigger(Grabber grabber)
    {
        _conversationController.shouldTrigger = false;
    }

    private void SetTrueNPCTrigger()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        _conversationController.shouldTrigger = true;
    }

    void OnDestroy()
    {
        DialogueBoxController.OnDialogueEnded -= DialogueTransition;
    }

}
