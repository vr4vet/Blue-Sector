using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scale : MonoBehaviour
{
    [SerializeField]
    private BoxCollider boxCollider;

    public float totalWeight;

    public TMP_Text displayText;

    private void OnTriggerEnter(Collider collisionObject)
    {
        if (collisionObject.GetComponent<Weight>())
        {
            totalWeight = collisionObject.GetComponent<Weight>().ObjectWeight;
            displayText.SetText(System.Math.Round(totalWeight, 3).ToString());
        }
    }

}
