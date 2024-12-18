using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This script is a modified version of the SpezializedNpcBehaviour script in fish feeding, this version is used in the laboratory scene and should be able to handle multiple points for the NPC to walk to.
public class WalkingNpc : MonoBehaviour
{
    [HideInInspector] private NPCSpawner _npcSpawner;
    [HideInInspector] private GameObject _npc;

    [HideInInspector] private NavMeshAgent _agent;
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _velocityYHash;
    private ConversationController _conversationController;
    [SerializeField] private GameObject dialogueTransitions;
    [SerializeField] private GameObject dialogueTransitions2;
    [SerializeField] private List<List<Transform>> paths;
    [SerializeField] private List<List<Transform>> paths2;
    private int currentPosition;
    private int currentPath;
    private bool walking;
    private BoxCollider boxCollider;
    private bool rotating;
    private GameObject _npcFeedback;

    // Start is called before the first frame update
    void Start()
    {
        _npcSpawner = GetComponent<NPCSpawner>();
        _npcFeedback = GameObject.Find("NPCFeedback");
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

        _npc = _npcSpawner._npcInstances[0];

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
        
        // Add the first path
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
        
        // Add the second path
        paths2 = new List<List<Transform>>();
        foreach (Transform child in dialogueTransitions2.transform)
        {
            List<Transform> path = new List<Transform>();
            foreach (Transform grandchild in child)
            {
                path.Add(grandchild);
            }
            paths2.Add(path);
        }
        

        DialogueBoxController.OnDialogueEnded += DialogueTransition;

        currentPosition = 0;
        currentPath = -1;

        walking = false;
        rotating = false;
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
                // The path is chosen based on the current dialogue
                if (_npcFeedback.GetComponent<NpcTriggerDialogue>().currentDialogue == "Follow me to the measuring equipment")
                {
                    if (currentPath == -1) { return; }
                    NavMeshPath path = new NavMeshPath();
                    _agent.CalculatePath(paths[currentPath][currentPosition].position, path);
                    _agent.SetPath(path);
                    if (_agent.remainingDistance < 1.8f)
                    {
                        currentPosition++;
                        currentPosition = 2;
                        if (currentPosition == paths[currentPath].Count)
                        {
                            rotating = true;
                            walking = false;
                            currentPosition = 0;
                            
                            _conversationController.StartDialogueTree("LarsDialogue");
                            
                            // Adjusts the size of the box collider when the NPC is in the control room
                            if (currentPath == 0)
                            {
                                boxCollider.center = new Vector3(-0.11f, 0f, 1.6f);
                                boxCollider.size = new Vector3(3.2f, 2f, 3.7f);
                            }
                        }
                    } 
                }
                else if (_npcFeedback.GetComponent<NpcTriggerDialogue>().currentDialogue == "Follow me to the microscope")
                {
                    if (currentPath == -1) { return; }
                    NavMeshPath path = new NavMeshPath();
                    _agent.CalculatePath(paths2[currentPath][currentPosition].position, path);
                    _agent.SetPath(path);
                    if (_agent.remainingDistance < 0.1f)
                    {
                        currentPosition++;
                        currentPosition = 2;
                        
                        if (currentPosition == paths2[currentPath].Count)
                        {
                            rotating = true;
                            walking = false;
                            currentPosition = 0;
                            
                            _conversationController.StartDialogueTree("MicroscopeDialogue");

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
        }
        else {
            _animator = _npc.GetComponentInChildren<Animator>();
        }
    }

    void FixedUpdate()
    {
        if (rotating)
        {
            if (_npcFeedback.GetComponent<NpcTriggerDialogue>().currentDialogue == "Follow me to the measuring equipment")
            {
                // Rotates NPC to last destination point
                Vector3 destination = paths[currentPath][paths[currentPath].Count - 1].position;
                Vector3 lookPos = destination - _npc.transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, rotation, 0.04f);

                float rotationOffset = Vector3.Angle((new Vector3(destination.x, _npc.transform.position.y, destination.z) - _npc.transform.position), _npc.transform.forward);

                if (rotationOffset < 0.01f) 
                    rotating = false;  
            } 
            else if (_npcFeedback.GetComponent<NpcTriggerDialogue>().currentDialogue == "Follow me to the microscope")
            {
                // Rotates NPC to last destination point
                Vector3 destination = paths2[currentPath][paths2[currentPath].Count - 1].position;
                Vector3 lookPos = destination - _npc.transform.position;
                lookPos.y = 0;
                Quaternion rotation = Quaternion.LookRotation(lookPos);
                _npc.transform.rotation = Quaternion.Slerp(_npc.transform.rotation, rotation, 0.04f);

                float rotationOffset = Vector3.Angle((new Vector3(destination.x, _npc.transform.position.y, destination.z) - _npc.transform.position), _npc.transform.forward);

                if (rotationOffset < 0.01f) 
                    rotating = false;    
            }
        }
    }

    public void DialogueTransition(string name)
    {
        currentPath = -1;
        if (_npcFeedback.GetComponent<NpcTriggerDialogue>().currentDialogue == "Follow me to the measuring equipment")
        {
            if (name != _npc.name) { return; }
            currentPath++;
            if (currentPath >= paths.Count) { return; }
            walking = true;  
        } 
        else if (_npcFeedback.GetComponent<NpcTriggerDialogue>().currentDialogue == "Follow me to the microscope")
        {
            if (name != _npc.name) { return; }
            currentPath++;
            if (currentPath >= paths2.Count) { return; }
            walking = true; 
        }
        
    }

    private void SetFalseNPCTrigger(Grabber grabber)
    {
        _conversationController.GetDialogueTree().shouldTriggerOnProximity = false;
    }

    private void SetTrueNPCTrigger()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        _conversationController.GetDialogueTree().shouldTriggerOnProximity = true;
    }

    void OnDestroy()
    {
        DialogueBoxController.OnDialogueEnded -= DialogueTransition;
    }
}
