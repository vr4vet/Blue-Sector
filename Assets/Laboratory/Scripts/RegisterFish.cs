using System;
using UnityEngine;
using UnityEngine.Events;

public class RegisterFish : MonoBehaviour
{
    public ResultLogger resultLogger;
    public float fishWeight;
    public float fishLength;
    public float conditionRight;
    public GameObject fishObject;
    public Weight weight;
    private DialogueBoxController _dialogueBoxController;
    public NpcTriggerDialogue NpcTriggerDialogue;
    public UnityEvent m_OnFishPlacedOnPlate;

    private void Start()
    {
        m_OnFishPlacedOnPlate ??= new UnityEvent();
        _dialogueBoxController = FindObjectOfType<DialogueBoxController>();
    }
    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.GetComponent<Weight>() && collisionObject.gameObject.CompareTag("Bone"))
        {
            if (_dialogueBoxController.dialogueTreeRestart != null && _dialogueBoxController.dialogueTreeRestart.name == "LarsDialogue")
            {
                if (_dialogueBoxController._dialogueText.text == _dialogueBoxController.dialogueTreeRestart.sections[5].dialogue[1])
                {
                    _dialogueBoxController.SkipLine();
                    m_OnFishPlacedOnPlate.Invoke();

                }
            }
            weight = collisionObject.GetComponent<Weight>();
            fishObject = weight.fish;

            //resultLogger.activeFishText.SetText("Fish "+ resultLogger.getFishNumber(fishObject).ToString());
            resultLogger.SetActiveFish(resultLogger.getFishNumber(fishObject));
            
            // Get weight
            fishWeight = weight.ObjectWeight;
            fishWeight = (float)System.Math.Round(fishWeight, 1);

            // Get length
            float lengt = weight.fish.transform.localScale.x;
            fishLength = (float)(3388341 + (-5.016348 - 3388341) / (1 + Math.Pow(lengt / 282116.9, 0.882021)));
            fishLength = (float)Math.Round(fishLength * 2, MidpointRounding.AwayFromZero) / 2;
            
            // Calculate condition factor
            conditionRight = ((fishWeight/fishLength)/fishLength)/fishLength;
            conditionRight *= 100;
            conditionRight = (float)Math.Truncate(conditionRight * 100) / 100;
            
        }
        else if (collisionObject.gameObject.name == "basket_plastic")
        {
            collisionObject.transform.position = ObjectPositions.Instance._basketPosition;
            collisionObject.transform.rotation = ObjectPositions.Instance._basketRotation;
            NpcTriggerDialogue.Error5();
        }
        else if (collisionObject.gameObject.name == "counter_handheld")
        {
            collisionObject.transform.position = ObjectPositions.Instance._handheldCounterPosition;
            collisionObject.transform.rotation = ObjectPositions.Instance._handheldCounterRotation;
            NpcTriggerDialogue.Error5();
        }
        else if (collisionObject.gameObject.name == "MicroscopeSlideModel")
        {
            collisionObject.transform.position = ObjectPositions.Instance._microscopeSlidePosition;
            collisionObject.transform.rotation = ObjectPositions.Instance._microscopeSlideRotation;
            NpcTriggerDialogue.Error5();
        }
        
    }

    public void activeFishChanged(GameObject fish)
    {
        fishObject = fish;
        weight = fish.GetComponentInChildren<Weight>();
        
        // Get weight
        fishWeight = weight.ObjectWeight;
        fishWeight = (float)System.Math.Round(fishWeight, 1);

        // Get length
        float lengt = weight.fish.transform.localScale.x;
        fishLength = (float)(3388341 + (-5.016348 - 3388341) / (1 + Math.Pow(lengt / 282116.9, 0.882021)));
        fishLength = (float)Math.Round(fishLength * 2, MidpointRounding.AwayFromZero) / 2;
        
        // Calculate condition factor
        conditionRight = ((fishWeight/fishLength)/fishLength)/fishLength;
        conditionRight *= 100;
        conditionRight = (float)Math.Round(conditionRight, 2);
    }

    // private void OnTriggerExit(Collider collisionObject)
    // {
    //     if (collisionObject.GetComponent<Weight>())
    //     {
    //         fishWeight = 0;
    //         fishLength = 0;
    //         conditionRight = 0;
    //         fishObject = null;
    //     }
    // }
}