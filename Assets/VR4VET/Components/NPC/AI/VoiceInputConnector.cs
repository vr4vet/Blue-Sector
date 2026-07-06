using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages voice input connections between the Input System and ConversationController.
/// Place this script on your ExtraInput GameObject with the Player Input component.
/// </summary>
public class VoiceInputConnector : MonoBehaviour
{
    // The reference to the active NPC will be set dynamically when player enters a trigger
    private ConversationController activeConversationController;
    private bool isButtonHeld = false;

    // Called by Player Input component when VoiceRecord action is started (button pressed)
    public void OnVoiceRecordStarted(InputAction.CallbackContext context)
    {
        if (activeConversationController != null)
        {
            Debug.Log("VoiceInputConnector: Voice recording started via InputAction");
            activeConversationController.HandleRecordButton(true);
            isButtonHeld = true;
        }
        else
        {
            Debug.Log("VoiceInputConnector: No active conversation controller to handle voice recording");
        }
    }

    // Called by Player Input component when VoiceRecord action is canceled (button released)
    public void OnVoiceRecordEnded(InputAction.CallbackContext context)
    {
        if (activeConversationController != null && isButtonHeld)
        {
            Debug.Log("VoiceInputConnector: Voice recording ended via InputAction");
            activeConversationController.HandleRecordButton(false);
            isButtonHeld = false;
        }
    }

    // Called by ConversationController.OnTriggerEnter when player enters NPC trigger
    public void SetActiveConversationController(ConversationController controller)
    {
        // If we were holding the button down and switched controllers, stop recording on previous
        if (isButtonHeld && activeConversationController != null && activeConversationController != controller)
        {
            activeConversationController.HandleRecordButton(false);
        }

        activeConversationController = controller;
        Debug.Log($"Active conversation controller set to: {(controller != null ? controller.gameObject.name : "None")}");

        // If button is already held down when entering trigger, start recording on new controller
        if (isButtonHeld && controller != null)
        {
            controller.HandleRecordButton(true);
        }
    }

    // Called by ConversationController.OnTriggerExit when player leaves NPC trigger
    public void ClearActiveConversationController(ConversationController controller)
    {
        // Only clear if it's the same controller
        if (activeConversationController == controller)
        {
            // If we're holding the button, make sure we stop recording first
            if (isButtonHeld)
            {
                activeConversationController.HandleRecordButton(false);
                isButtonHeld = false;
            }

            activeConversationController = null;
            Debug.Log("Active conversation controller cleared");
        }
    }

    // Safety check - ensure we're not recording if the app/game pauses or loses focus
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && isButtonHeld && activeConversationController != null)
        {
            activeConversationController.HandleRecordButton(false);
            isButtonHeld = false;
        }
    }

    // Safety check if scene changes or application quits
    void OnDisable()
    {
        if (isButtonHeld && activeConversationController != null)
        {
            activeConversationController.HandleRecordButton(false);
            isButtonHeld = false;
        }
    }

#if UNITY_EDITOR
    // Editor helper for debugging
    public void TestConnectionStatus()
    {
        if (activeConversationController != null)
        {
            Debug.Log($"Currently connected to: {activeConversationController.gameObject.name}");
            Debug.Log($"Parent object: {activeConversationController.transform.parent?.name}");
            Debug.Log($"Button held: {isButtonHeld}");
        }
        else
        {
            Debug.Log("No active conversation controller currently connected");
        }
    }
#endif
}