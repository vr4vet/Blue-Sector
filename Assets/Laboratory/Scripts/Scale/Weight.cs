using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Weight : MonoBehaviour
{
    // ----------------- Editor Variables -----------------
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

    public UnityEvent m_OnWeighingFish; // event to complete step on tablet/task structure

    void Start()
    {
        m_OnWeighingFish ??= new UnityEvent();

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

        if (collision.gameObject.layer == LayerMask.NameToLayer("Bone") && collision.GetComponent<Weight>())
        {
            if ( dialogueBoxController.DialogueTreeRestart.name == "LarsDialogue" && dialogueBoxController._dialogueText.text == dialogueBoxController.DialogueTreeRestart.sections[1].dialogue[3])
            {
                dialogueBoxController.SkipLine();
                m_OnWeighingFish.Invoke();
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
