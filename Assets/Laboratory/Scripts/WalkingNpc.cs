using BNG;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A general purpose script to make an NPC walk along a path.
/// </summary>
public class WalkingNpc : MonoBehaviour
{
    [HideInInspector] private NavMeshAgent _agent;
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _velocityYHash;
    private ConversationController _conversationController;
    private BoxCollider boxCollider;
    private bool rotating = false;

    // Start is called before the first frame update
    void Start()
    {
        // Setting up navigation agent
        _agent = GetComponent<NavMeshAgent>();
        _velocityYHash = Animator.StringToHash("VelocityY");
        if (_agent == null)
        {
            Debug.LogError("THe NPC is missing the NavMeshAgent");
            return;
        }

        _conversationController = GetComponentInChildren<ConversationController>();
    }

    private Transform _rotationDestination;
    // Update is called once per frame
    void Update()
    {
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
            return;
        }

        // ensure animation speed matches walking speed
        _animator.SetFloat(_velocityYHash, _agent.velocity.magnitude);

        // preventing model from getting off-centered
        _animator.transform.localPosition = Vector3.zero;

        // rotation in Update() to ensure smooth rotation
        if (rotating)
        {
            Vector3 lookPos = _rotationDestination.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 3f * Time.deltaTime);

            float rotationOffset = Vector3.Angle(new Vector3(_rotationDestination.transform.position.x, transform.position.y, _rotationDestination.transform.position.z) 
                                                  - transform.position, transform.forward);
            Debug.Log(rotationOffset);
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
        NavMeshPath path = new();

        foreach (NPCPathPoint target in pathToFollow.GetComponentsInChildren<NPCPathPoint>())
        {
            // determine if current destination should be walked or rotated towards
            if (target.GetTargetType() == NPCPathPoint.TargetType.Walk)
            {
                _agent.CalculatePath(target.transform.position, path);
                _agent.SetPath(path);
            }
            else if (target.GetTargetType() == NPCPathPoint.TargetType.Rotate)
            {
                _rotationDestination = target.transform;
                rotating = true;
            }

            // wait until NPC has arrived and/or finished rotating
            while (rotating || _agent.remainingDistance >= 0.1f)
                yield return null;

            // change to the target's conversation tree if set
            if (!target.GetNextConversationTree().Equals(string.Empty))
                _conversationController.StartDialogueTree(target.GetNextConversationTree());
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

    /// <summary>
    /// Public method to externally make NPC walk along a path.
    /// </summary>
    /// <param name="pathName"></param>
    public void WalkPath(string pathName)
    {
        GameObject path = GameObject.Find(pathName);
        StartCoroutine(FollowPath(path));
    }

    private void OnDrawGizmos()
    {
        if (_agent == null)
            return;

        if (rotating)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_rotationDestination.position, .25f);
            Gizmos.DrawLine(_agent.transform.position, _rotationDestination.position);
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_agent.destination, .25f);
            Gizmos.DrawLine(_agent.transform.position, _agent.destination);
        }
    }
}
