using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weight : MonoBehaviour
{
    // ----------------- Editor Variables -----------------
    [SerializeField]
    public GameObject fish;
    [Header("Weight settings")]
    public bool RandomWeight;

    [SerializeField]
    [Tooltip("The minimum weight that should be allowed if weight is random")]
    [Range(100, 4000)]
    private float minWeight;

    [SerializeField]
    [Tooltip("The maximum weight that should be allowed if weight is random")]
    [Range(100, 4000)]
    private float maxWeight;

    [SerializeField]
    private float _objectWeight;

    private DialogueBoxController dialogueBoxController;
    private Dictionary<GameObject,float> fishLengths = new Dictionary<GameObject, float>();

    // ----------------- Public Variables -----------------
    public float ObjectWeight
    {
        get { return _objectWeight; }
        set { _objectWeight = value; }
    }

    void Start()
    {
        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        if (RandomWeight)
        {
            _objectWeight = RandomizeWeight();
        }
    }

    private float RandomizeWeight()
    {
        _objectWeight = UnityEngine.Random.Range(minWeight, maxWeight);
        return _objectWeight;
    }

    private void OnTriggerEnter(Collider collision)
    {

        if (collision.GetType() == typeof(CapsuleCollider) && collision.GetComponent<Weight>())
        {
            if (dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[3].dialogue[3])
            {
                dialogueBoxController.SkipLine();
            }





            float lengt = collision.GetComponent<Weight>().fish.transform.localScale.x;
            float fishLength = (float)(4.58 * Math.Exp(2.33 * lengt) + 10.31);
            if (!fishLengths.ContainsKey(collision.GetComponent<Weight>().fish))
            {
                fishLengths.Add(collision.GetComponent<Weight>().fish, fishLength);
            }
        }
    }
}
