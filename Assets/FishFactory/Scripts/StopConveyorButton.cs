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
        {
            if (!(_dialogueBoxController = _bleedingGuide.GetComponent<DialogueBoxController>()))
                Debug.LogError("Could not find dialogue box controller");
        }
            
    }
    /// <summary>
    /// Toggles the main task on and off
    /// </summary>
    public void ToggleTaskOn()
    {
        GameManager.Instance.ToggleTaskOn();
        if (_dialogueBoxController && _dialogueBoxController.dialogueTreeRestart && _dialogueBoxController.dialogueTreeRestart.name == "BleedingInstruction" && _dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[4].dialogue[0])
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
