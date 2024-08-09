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
        switch (buttonType)
        {
            case ButtonType.On:
                StopAllCoroutines();
                scale.displayText.SetText("000.0");
                scale.audio.Play();
                scale.scaleOn = true;

                if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[2].dialogue[1]) {
                    
                    dialogueBoxController.SkipLine();
                    Debug.Log(dialogueBoxController.dialogueTreeRestart.sections[2].dialogue[1]);

                    
                }

                break;
            case ButtonType.Off:
                StopAllCoroutines();
                scale.displayText.SetText("");
                scale.audio.Play();
                scale.scaleOn = true;
                break;
            case ButtonType.Reset:
                StopAllCoroutines();
                scale.totalWeight = 0;
                scale.displayText.SetText("000.0");
                scale.audio.Play();

                if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[2].dialogue[3]) {
                    dialogueBoxController.SkipLine();
                    
                }

                break;
        }
    }
}
