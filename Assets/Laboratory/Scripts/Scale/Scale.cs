using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scale : MonoBehaviour
{
    // ----------------- Editor Variables -----------------
    [SerializeField]
    public AudioSource audio;
    [SerializeField]
    public float totalWeight;
    [SerializeField]
    public TMP_Text displayText;
    [SerializeField]
    public GameObject tray;

    // ----------------- Public Variables -----------------
    [HideInInspector]
    public bool scaleOn = false;
    
    [HideInInspector]
    public bool tubWasUsed = false;

    // ----------------- Private Variables -----------------
    private List<GameObject> objectsOnScale = new List<GameObject>();
    private IEnumerator corutine;

     private DialogueBoxController dialogueBoxController;

    private void Start() {

        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
    }

    private void OnTriggerEnter(Collider collisionObject)
    {
        if (!scaleOn)
        {
            return;
        }
        if (objectsOnScale.Contains(collisionObject.gameObject))
        {
            return;
        }
        else if (collisionObject.GetComponent<Weight>())
        {
            objectsOnScale.Add(collisionObject.gameObject);
            totalWeight += collisionObject.GetComponent<Weight>().ObjectWeight;
            StopAllCoroutines();
            corutine = SetScaleText(totalWeight - collisionObject.GetComponent<Weight>().ObjectWeight);
            StartCoroutine(corutine);
             if (collisionObject.gameObject.name == "basket_plastic" && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[3].dialogue[1])
            {
                dialogueBoxController.SkipLine();
            }
            else if (collisionObject.gameObject.tag == "Bone" && objectsOnScale.Contains(tray))
            {
                tubWasUsed = true;
            }
           
        }
    }
   
    void OnTriggerExit(Collider collisionObject)
    {
        if (!scaleOn)
        {
            return;
        }
        if (objectsOnScale.Contains(collisionObject.gameObject))
        {
            objectsOnScale.Remove(collisionObject.gameObject);
            totalWeight -= collisionObject.GetComponent<Weight>().ObjectWeight;
            if (totalWeight < 0)
            {
                totalWeight = 0;
            }
            StopAllCoroutines();
            corutine = SetScaleText(totalWeight + collisionObject.GetComponent<Weight>().ObjectWeight);
            StartCoroutine(corutine);
        }
        else
        {
            return;
        }
    }

    private IEnumerator SetScaleText(float previousWeight)
    {
        float textOut = previousWeight;
        if (totalWeight == 0)
        {
            while (textOut >= 0)
            {
                if (textOut - previousWeight * 0.3f <= 0)
                {
                    textOut -= previousWeight * 0.3f;
                    displayText.SetText("000.0");
                    yield return null;
                }
                else
                {
                    textOut -= previousWeight * 0.3f;
                    displayText.SetText(System.Math.Round(textOut, 1).ToString());
                    yield return new WaitForSeconds(0.04f);
                }
            }
            displayText.SetText("000.0");
            yield return null;
        }
        else if (previousWeight < totalWeight)
        {
            while (textOut < (totalWeight*1.1f))
            {
                textOut += totalWeight * 0.2f;
                displayText.SetText(System.Math.Round(textOut, 1).ToString());
                yield return new WaitForSeconds(0.1f);
            }
            while (textOut > totalWeight)
            {
                textOut -= textOut * 0.12f;
                displayText.SetText(System.Math.Round(textOut, 1).ToString());
                yield return new WaitForSeconds(0.08f);
            }
            displayText.SetText(System.Math.Round(totalWeight, 1).ToString());
            yield return null;
        }

        else if (previousWeight > totalWeight)
        {
            while (textOut > (totalWeight*1.1f))
            {
                textOut -= textOut * 0.2f;
                displayText.SetText(System.Math.Round(textOut, 1).ToString());
                yield return new WaitForSeconds(0.1f);
            }
            while (textOut < totalWeight)
            {
                textOut += textOut * 0.12f;
                displayText.SetText(System.Math.Round(textOut, 1).ToString());
                yield return new WaitForSeconds(0.08f);
            }
            if (totalWeight == 0)
            {
                displayText.SetText("000.0");
            }
            displayText.SetText(System.Math.Round(totalWeight, 1).ToString());
            yield return null;
        }
        yield return null;
    }
}