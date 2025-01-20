using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleButtons : MonoBehaviour
{
    private DialogueBoxController dialogueBoxController;
    public Scale scale;
    public enum ButtonType
    {
        On,
        Off,
        Reset,
    }

    private void Start()
    {
        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
    }

    [SerializeField]
    public ButtonType buttonType;

    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.name == "CollisionTriggerHandler")
        {
            return;
        }
        switch (buttonType)
        {
            case ButtonType.On:
                StopAllCoroutines();
                scale.displayText.SetText("000.0");
                scale.audio.Play();
                scale.scaleOn = true;
                if (dialogueBoxController.dialogueTreeRestart != null)
                {
                    if (dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[1].dialogue[0])
                    {
                        dialogueBoxController.SkipLine();
                    }
                }

                break;
            case ButtonType.Off:
                StopAllCoroutines();
                scale.displayText.SetText("");
                scale.audio.Play();
                scale.scaleOn = false;
                break;
            case ButtonType.Reset:
                if (scale.scaleOn)
                {
                    StopAllCoroutines();
                    scale.totalWeight = 0;
                    scale.displayText.SetText("000.0");
                    scale.audio.Play();

                    if (dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[1].dialogue[2]) {
                        dialogueBoxController.SkipLine();
                    
                    } 
                }
                

                break;
        }
    }
}
