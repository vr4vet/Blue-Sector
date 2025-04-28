using UnityEngine;

/// <summary>
/// Handles animations for fish models
/// Works with various animator setups by checking for parameter existence
/// </summary>
public class SimpleFishAnimatorController : MonoBehaviour
{
    [SerializeField] private string[] possibleTalkParameters = { "isTalking", "Talk", "talking" };
    [SerializeField] private string[] possibleMoveParameters = { "SwimmingSpeed", "VelocityY", "Speed", "Move" };
    
    private Animator animator;
    private string usableTalkParameter = null;
    private string usableMoveParameter = null;
    private bool isBoolTalkParameter = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        
        if (animator == null)
        {
            Debug.LogWarning("No animator found on fish or its children");
            return;
        }
        
        // Find the first talking parameter that exists
        foreach (string paramName in possibleTalkParameters)
        {
            if (HasAnimatorParameter(paramName))
            {
                usableTalkParameter = paramName;
                AnimatorControllerParameter param = GetAnimatorParameter(paramName);
                isBoolTalkParameter = param != null && param.type == AnimatorControllerParameterType.Bool;
                Debug.Log($"Found talk parameter: {paramName}, isBool: {isBoolTalkParameter}");
                break;
            }
        }
        
        // Find the first movement parameter that exists
        foreach (string paramName in possibleMoveParameters)
        {
            if (HasAnimatorParameter(paramName))
            {
                usableMoveParameter = paramName;
                Debug.Log($"Found movement parameter: {paramName}");
                break;
            }
        }
    }
    
    /// <summary>
    /// Start talking animation
    /// </summary>
    public void OnTalkAnimationStarted()
    {
        if (animator == null || usableTalkParameter == null) return;
        
        if (isBoolTalkParameter)
        {
            animator.SetBool(usableTalkParameter, true);
        }
        else
        {
            animator.SetTrigger(usableTalkParameter);
        }
    }
    
    /// <summary>
    /// End talking animation
    /// </summary>
    public void OnTalkAnimationEnded()
    {
        if (animator == null || usableTalkParameter == null || !isBoolTalkParameter) return;
        
        animator.SetBool(usableTalkParameter, false);
    }
    
    /// <summary>
    /// Set movement speed for animation
    /// </summary>
    public void SetMovementSpeed(float speed)
    {
        if (animator == null || usableMoveParameter == null) return;
        
        animator.SetFloat(usableMoveParameter, speed);
    }
    
    /// <summary>
    /// Check if an animator parameter exists
    /// </summary>
    private bool HasAnimatorParameter(string paramName)
    {
        if (animator == null) return false;
        
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
    
    /// <summary>
    /// Get an animator parameter
    /// </summary>
    private AnimatorControllerParameter GetAnimatorParameter(string paramName)
    {
        if (animator == null) return null;
        
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return param;
        }
        return null;
    }
}