using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the state of the AI features toggle in the menu.
/// Updates the ActionManager based on the toggle state.
/// </summary>
public class MenuAIToggle : MonoBehaviour
{
    [SerializeField]
    private Toggle _aiFeaturesToggle;

    private ActionManager _actionManager;

    /// <summary>
    /// Initializes the script. Gets the ActionManager instance
    /// and sets the initial state of the toggle based on the ActionManager.
    /// </summary>
    void Start()
    {
        _actionManager = ActionManager.Instance;

        // Check if the ActionManager and Toggle are assigned
        if (_actionManager == null)
        {
            Debug.LogError("ActionManager instance not found. Make sure it exists and is accessible.");
            return;
        }

        if (_aiFeaturesToggle == null)
        {
            Debug.LogError("AI Features Toggle is not assigned in the inspector.");
            return;
        }

        // Set the initial state of the toggle based on the current AI features state
        _aiFeaturesToggle.isOn = _actionManager.GetToggleBool();

        // Add a listener to the toggle's onValueChanged event
        // This ensures our method is called whenever the toggle state changes
        _aiFeaturesToggle.onValueChanged.AddListener(OnAIToggleValueChanged);
    }

    /// <summary>
    /// This method is called when the UI Toggle's value changes.
    /// It updates the AI features state in the ActionManager.
    /// </summary>
    /// <param name="isOn">The new state of the toggle (true if on, false if off).</param>
    public void OnAIToggleValueChanged(bool isOn)
    {
        if (_actionManager == null)
        {
            Debug.LogError("ActionManager instance is null. Cannot set AI features.");
            return;
        }

        // Call the SetToggleBool method on the ActionManager
        _actionManager.SetToggleBool(isOn);
        Debug.Log($"AI features toggled: {isOn}");

        // Note: This script only updates the ActionManager's state.
        // The scene reload logic (if needed) should be handled elsewhere,
        // potentially in the ConsentForm script or a game flow manager,
        // which can check ActionManager.GetToggleBool() when appropriate.
    }

    /// <summary>
    /// Cleans up the event listener when the object is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        // Remove the listener to prevent memory leaks
        if (_aiFeaturesToggle != null)
        {
            _aiFeaturesToggle.onValueChanged.RemoveListener(OnAIToggleValueChanged);
        }
    }
}
