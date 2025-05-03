using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpecializedNPCBehavior : MonoBehaviour
{
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
    


    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("DialogueTransitions") == null)
        {
            Debug.LogError("No DialogueTransitions found");
        }
        else
        {
            dialogueTransitions = GameObject.Find("DialogueTransitions");
        }
        
        _npc = gameObject;
        // Adjusts the size of the NPCs
        //npc.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        Transform dialogueCanvas = _npc.transform.GetChild(1);
        dialogueCanvas.localPosition = new Vector3(dialogueCanvas.localPosition.x, dialogueCanvas.localPosition.y, 0.55f);
        dialogueCanvas.localScale = new Vector3(0.008f, 0.008f, 0.008f);
        

        

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
            Debug.LogError("The NPC is missing the Animator");
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
            
            if (walking)
            {
                if (currentPosition < paths[currentPath].Count)
                {
                    NavMeshPath path = new NavMeshPath();
                    _agent.CalculatePath(paths[currentPath][currentPosition].position, path);
                    _agent.SetPath(path);
                    if (_agent.remainingDistance < 0.2f)
                    {
                        currentPosition++;
                    }   
                }
                
                else if (currentPosition >= paths[currentPath].Count)
                {
                    walking = false;
                    _conversationController.NextDialogueTree();
                    currentPosition = 0;
                    
                }
            }
        }
        else 
        {
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
        _conversationController.GetDialogueTree().ShouldTriggerOnProximity = false;
    }

    private void SetTrueNPCTrigger()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        _conversationController.GetDialogueTree().ShouldTriggerOnProximity = true;
    }

    void OnDestroy()
    {
        DialogueBoxController.OnDialogueEnded -= DialogueTransition;
    }

    private void CreatePaths()
    {
        
    }

}
