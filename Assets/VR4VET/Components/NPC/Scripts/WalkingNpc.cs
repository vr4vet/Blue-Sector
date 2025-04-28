using BNG;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A general purpose script to make an NPC walk along a path.
/// </summary>
public class WalkingNpc : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private int _velocityYHash;
    private ConversationController _conversationController;
    private bool rotating = false;
    private bool agentDestroyed = false;

    // Start is called before the first frame update
    void Start()
    {
        // Setting up navigation agent
        _agent = GetComponent<NavMeshAgent>();
        _velocityYHash = Animator.StringToHash("VelocityY");
        if (_agent == null)
        {
            Debug.LogError("The NPC is missing the NavMeshAgent");
            return;
        }

        _conversationController = GetComponentInChildren<ConversationController>();
    }

    private Transform _rotationDestination;
    // Update is called once per frame
    void Update()
    {
        // First check if we need to find the animator
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
            return;
        }

        // Check if the agent is still valid
        if (_agent == null || !_agent || agentDestroyed)
        {
            // Try to re-acquire the agent if it was lost
            if (!agentDestroyed)
            {
                _agent = GetComponent<NavMeshAgent>();
                if (_agent == null)
                {
                    agentDestroyed = true;
                    Debug.LogWarning("NavMeshAgent is missing or destroyed. Movement functionality disabled.");
                }
            }
            return;
        }

        try
        {
            // Check if agent is valid before accessing velocity
            if (_agent != null && _agent.isActiveAndEnabled)
            {
                // ensure animation speed matches walking speed
                _animator.SetFloat(_velocityYHash, _agent.velocity.magnitude);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Error accessing NavMeshAgent: " + e.Message);
            agentDestroyed = true;
            return;
        }

        // preventing model from getting off-centered
        _animator.transform.localPosition = Vector3.zero;

        // rotation in Update() to ensure smooth rotation
        if (rotating && _rotationDestination != null)
        {
            Vector3 lookPos = _rotationDestination.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 3f * Time.deltaTime);

            float rotationOffset = Vector3.Angle(new Vector3(_rotationDestination.transform.position.x, transform.position.y, _rotationDestination.transform.position.z) 
                                                  - transform.position, transform.forward);

            if (rotationOffset < 0.01f)
                rotating = false;
        }
    }

    /// <summary>
    /// Makes NPC walk along path, one target/point at a time sequentially. 
    /// The path can contain an arbitrary amount of targets/points >= 1.
    /// Each target makes the NPC either walk or turn towards itself, and has the option to change the NPC's current dialogue tree on arrival.
    /// </summary>
    /// <param name="pathToFollow"></param>
    /// <returns></returns>
    private IEnumerator FollowPath(GameObject pathToFollow)
    {
        // Check if the agent is still valid before attempting navigation
        if (_agent == null || !_agent || !_agent.isActiveAndEnabled)
        {
            Debug.LogWarning("Cannot follow path: NavMeshAgent is missing or disabled");
            yield break;
        }

        NavMeshPath path = new();

        foreach (NPCPathPoint target in pathToFollow.GetComponentsInChildren<NPCPathPoint>())
        {
            // Safety check before each point
            if (_agent == null || !_agent || !_agent.isActiveAndEnabled)
            {
                Debug.LogWarning("NavMeshAgent was destroyed during path following. Aborting path.");
                yield break;
            }

            // determine if current destination should be walked or rotated towards
            if (target.GetTargetType() == NPCPathPoint.TargetType.Walk)
            {
                try
                {
                    _agent.CalculatePath(target.transform.position, path);
                    _agent.SetPath(path);
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Error setting NavMeshAgent path: " + e.Message);
                    yield break;
                }
            }
            else if (target.GetTargetType() == NPCPathPoint.TargetType.Rotate)
            {
                _rotationDestination = target.transform;
                rotating = true;
            }

            // wait until NPC has arrived and/or finished rotating
            while (rotating || 
                   (_agent != null && _agent.isActiveAndEnabled && _agent.remainingDistance >= 0.1f))
            {
                // Check again if agent is still valid
                if (_agent == null || !_agent || !_agent.isActiveAndEnabled)
                {
                    Debug.LogWarning("NavMeshAgent was destroyed while waiting for path completion.");
                    yield break;
                }
                
                yield return null;
            }

            // change to the target's conversation tree if set
            if (!target.GetNextConversationTree().Equals(string.Empty) && _conversationController != null)
                _conversationController.StartDialogueTree(target.GetNextConversationTree());
        }
    }

    private void SetFalseNPCTrigger(Grabber grabber)
    {
        if (_conversationController != null && _conversationController.GetDialogueTree() != null)
            _conversationController.GetDialogueTree().shouldTriggerOnProximity = false;
    }

    private void SetTrueNPCTrigger()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        
        if (_conversationController != null && _conversationController.GetDialogueTree() != null)
            _conversationController.GetDialogueTree().shouldTriggerOnProximity = true;
    }

    /// <summary>
    /// Public method to externally make NPC walk along a path.
    /// </summary>
    /// <param name="pathName"></param>
    public void WalkPath(string pathName)
    {
        GameObject path = GameObject.Find(pathName);
        if (path == null)
        {
            Debug.LogError($"Cannot find path with name: {pathName}");
            return;
        }
        
        StartCoroutine(FollowPath(path));
    }

    private void OnDrawGizmos()
    {
        if (_agent == null || !_agent || agentDestroyed)
            return;

        try
        {
            if (rotating && _rotationDestination != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_rotationDestination.position, .25f);
                Gizmos.DrawLine(_agent.transform.position, _rotationDestination.position);
            }
            else if (_agent.isActiveAndEnabled)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_agent.destination, .25f);
                Gizmos.DrawLine(_agent.transform.position, _agent.destination);
            }
        }
        catch (System.Exception)
        {
            // Silently ignore errors in the Gizmo drawing
        }
    }
}
