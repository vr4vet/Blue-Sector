using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopConveyorButton : MonoBehaviour
{
    private GameObject _bleedingGuide;
    private DialogueBoxController _dialogueBoxController;
    
    void Start()
    {
        _bleedingGuide = GameObject.Find("Bleeding station guide Bernard");
        if (_bleedingGuide != null)
            _dialogueBoxController = _bleedingGuide.GetComponent<DialogueBoxController>();
    }
    /// <summary>
    /// Toggles the main task on and off
    /// </summary>
    public void ToggleTaskOn()
    {
        GameManager.Instance.ToggleTaskOn();
        if (_dialogueBoxController != null && _dialogueBoxController.DialogueTreeRestart.name == "BleedingInstruction" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.DialogueTreeRestart.sections[4].dialogue[0])
        {
            _dialogueBoxController.SkipLine();
        }
    }

    /// <summary>
    /// Toggles the secondary task on and off
    /// </summary>
    public void ToggleSecondaryTaskOn()
    {
        GameManager.Instance.ToggleSecondaryTaskOn();
    }
}
