using UnityEngine;
using UnityEngine.Animations;
#if UNITY_EDITOR
using UnityEditor.Animations; // Contains AnimatorController, AnimatorStateMachine, etc.
#endif

/// <summary>
/// Creates a simple animator controller that can be used by the Animator component
/// Provides the essential state machine that other NPC scripts expect
/// </summary>
[CreateAssetMenu(fileName = "EmptyAnimatorController", menuName = "VR4VET/Animation/Empty Animator Controller", order = 1)]
public class EmptyAnimatorController : ScriptableObject
{
    // Reference to an existing animator controller asset to avoid runtime creation issues
    [SerializeField] private RuntimeAnimatorController baseAnimatorController;

    /// <summary>
    /// Returns the animator controller for use with fish NPCs
    /// </summary>
    public RuntimeAnimatorController GetController()
    {
        // Just return the assigned controller - simpler approach that works in both editor and runtime
        if (baseAnimatorController != null)
            return baseAnimatorController;
            
        // If no controller is assigned, log a warning
        Debug.LogWarning("No base animator controller assigned to EmptyAnimatorController. Fish animations may not work correctly.");
        return null;
    }

#if UNITY_EDITOR
    // This code only runs in the Unity Editor
    public RuntimeAnimatorController CreateControllerInEditor()
    {
        // Create a new animator controller asset
        AnimatorController controller = new AnimatorController();
        
        // Add required parameters that NPC scripts might look for
        controller.AddParameter("Talk", AnimatorControllerParameterType.Bool);
        controller.AddParameter("RandomTalk", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("HeadBobble", AnimatorControllerParameterType.Trigger);
        
        // Create a default layer
        AnimatorControllerLayer layer = new AnimatorControllerLayer();
        layer.name = "Base Layer";
        layer.stateMachine = new AnimatorStateMachine();
        controller.layers = new AnimatorControllerLayer[] { layer };
        
        // Create a simple idle state
        AnimatorState idleState = layer.stateMachine.AddState("Idle");
        // Make it the default state
        layer.stateMachine.defaultState = idleState;
        
        return controller;
    }
#endif
}