using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGuidingHands : MonoBehaviour
{
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private GameObject guidingHandSplint;
    [SerializeField] private GameObject guidingHandRope;
    [SerializeField] private GameObject guidingHandBucket;

    public void IntoAnchor()
    {
        if (!mm.GetStep("Get Equipment", "Get splinter pin").IsCompeleted())
        {
            guidingHandSplint.SetActive(true);
        }
        if (!mm.GetStep("Get Equipment", "Get rope").IsCompeleted())
        {
            guidingHandRope.SetActive(true);
        }
        if (!mm.GetStep("Get Equipment", "Get bucket and spade").IsCompeleted())
        {
            guidingHandBucket.SetActive(true);
        }
    }
}
