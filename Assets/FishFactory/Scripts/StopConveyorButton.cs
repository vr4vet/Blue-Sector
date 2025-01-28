using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopConveyorButton : MonoBehaviour
{
    private DialogueBoxController _dialogueBoxController;
    
    void Start()
    {
        _dialogueBoxController = FindObjectOfType<DialogueBoxController>();
    }
    /// <summary>
    /// Toggles the main task on and off
    /// </summary>
    public void ToggleTaskOn()
    {
        GameManager.Instance.ToggleTaskOn();
        /*if (_dialogueBoxController.dialogueTreeRestart.name == "BleedingInstruction" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[3].dialogue[2])
        {
            _dialogueBoxController.SkipLine();
        }*/
    }

    /// <summary>
    /// Toggles the secondary task on and off
    /// </summary>
    public void ToggleSecondaryTaskOn()
    {
        GameManager.Instance.ToggleSecondaryTaskOn();
    }
}
