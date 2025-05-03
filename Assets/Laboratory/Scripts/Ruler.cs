using UnityEngine;
using UnityEngine.Events;

public class Ruler : MonoBehaviour
{
    private DialogueBoxController dialogueBoxController;
    public UnityEvent m_OnFishPlaced;

    private void Start()
    {
        m_OnFishPlaced ??= new UnityEvent();
        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        
    }

    private void OnTriggerEnter(Collider collision) 
    {
        if (collision.GetType() == typeof(CapsuleCollider) && collision.GetComponent<Weight>())
        {
            if (dialogueBoxController._dialogueText.text == dialogueBoxController.DialogueTreeRestart.sections[3].dialogue[0]) 
            {
                dialogueBoxController.SkipLine();
                m_OnFishPlaced.Invoke();
            }
        }
    }
}