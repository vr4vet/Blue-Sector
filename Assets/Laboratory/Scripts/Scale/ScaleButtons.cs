using UnityEngine;
using UnityEngine.Events;

public class ScaleButtons : MonoBehaviour
{
    private DialogueBoxController dialogueBoxController;
    public Scale scale;
    public ButtonType buttonType;
    public UnityEvent m_OnScaleReady; // event used to complete step on tablet/task structure

    public enum ButtonType
    {
        On,
        Off,
        Reset,
    }
    

    private void Start()
    {
        m_OnScaleReady ??= new UnityEvent();

        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
    }


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
                scale.Audio.Play();
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
                scale.Audio.Play();
                scale.scaleOn = false;
                break;
            case ButtonType.Reset:
                if (scale.scaleOn)
                {
                    StopAllCoroutines();
                    scale.totalWeight = 0;
                    scale.displayText.SetText("000.0");
                    scale.Audio.Play();

                    if (dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[1].dialogue[2]) 
                    {
                        dialogueBoxController.SkipLine();
                        m_OnScaleReady.Invoke();
                    } 
                }
                

                break;
        }
    }
}
