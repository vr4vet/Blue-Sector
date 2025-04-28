using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor.Animations; // For AnimatorController in editor
#endif

/// <summary>
/// Handles the setup of a fish NPC, providing compatibility components 
/// needed by the NPC systems that expect humanoid characters.
/// </summary>
public class FishNPCSetup : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private EmptyAnimatorController emptyAnimatorController;
    [SerializeField] private RuntimeAnimatorController fallbackAnimatorController;
    [Tooltip("If true, will create a dummy Animator component at runtime")]
    [SerializeField] private bool createDummyAnimator = true;
    
    [Header("Required Components")]
    [SerializeField] private bool addAudioSource = true;
    [SerializeField] private AudioClip defaultAudioClip;
    
    private void Awake()
    {
        // Check for the Animator component
        Animator animator = GetComponentInChildren<Animator>();
        if (animator == null && createDummyAnimator)
        {
            // Create a dummy animator component on the GameObject itself
            animator = gameObject.AddComponent<Animator>();
            Debug.Log("Added dummy Animator component to Fish NPC");
            
            // Use the empty animator controller if available
            if (emptyAnimatorController != null)
            {
                animator.runtimeAnimatorController = emptyAnimatorController.GetController();
                Debug.Log("Assigned empty animator controller from scriptable object");
            }
            else if (fallbackAnimatorController != null)
            {
                animator.runtimeAnimatorController = fallbackAnimatorController;
                Debug.Log("Assigned fallback animator controller");
            }
            else
            {
                // We can't create animator controllers at runtime in builds
                // Just log a warning
                Debug.LogWarning("No animator controller available. Some NPC systems may not work correctly.");
            }
        }
        
        // Add audio source if needed
        if (addAudioSource && GetComponent<AudioSource>() == null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f; // Use 2D audio by default
            audioSource.clip = defaultAudioClip;
            Debug.Log("Added AudioSource component to Fish NPC");
        }
        
        // Force enable SimpleFishController if present
        SimpleFishController fishController = GetComponent<SimpleFishController>();
        if (fishController != null)
        {
            fishController.enabled = true;
        }
        else 
        {
            Debug.LogWarning("No SimpleFishController found on Fish NPC. Adding one...");
            gameObject.AddComponent<SimpleFishController>();
        }
        
        // Ensure FishAnimatorController is present
        FishAnimatorController fishAnimator = GetComponent<FishAnimatorController>();
        if (fishAnimator == null)
        {
            fishAnimator = gameObject.AddComponent<FishAnimatorController>();
            Debug.Log("Added FishAnimatorController to handle animation events");
        }
        
        // Notify any DialogueBoxController to update its animator reference
        DialogueBoxController dialogueController = GetComponent<DialogueBoxController>();
        if (dialogueController != null && animator != null)
        {
            dialogueController.updateAnimator(animator);
            Debug.Log("Updated DialogueBoxController with animator reference");
        }
        
        // We don't need to create controllers at runtime, we'll rely on the FishAnimatorController
        // to handle all the animation states through code
        if (animator != null && fishAnimator != null)
        {
            fishAnimator.EnsureParametersHandled();
        }
    }

#if UNITY_EDITOR
    // Editor-only method for creating animator controllers
    // This should only be used in the Unity Editor, not at runtime
    private RuntimeAnimatorController CreateEditorAnimatorController()
    {
        try
        {
            // Create a new AnimatorController asset in memory
            AnimatorController controller = new AnimatorController();
            controller.name = "EditorFishController";
            
            // Add required parameters
            controller.AddParameter("isTalking", AnimatorControllerParameterType.Bool);
            controller.AddParameter("hasNewDialogueOptions", AnimatorControllerParameterType.Bool);
            controller.AddParameter("isPointing", AnimatorControllerParameterType.Bool);
            controller.AddParameter("isListening", AnimatorControllerParameterType.Bool);
            
            // Create a default state
            var layer = new AnimatorControllerLayer();
            layer.name = "Base Layer";
            layer.stateMachine = new AnimatorStateMachine();
            controller.layers = new[] { layer };
            
            return controller;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to create editor animator controller: {e.Message}");
            return null;
        }
    }
#endif
}