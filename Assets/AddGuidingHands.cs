using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGuidingHands : MonoBehaviour
{
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private GameObject guidingHandSplint;
    [SerializeField] private GameObject guidingHandRope;
    [SerializeField] private GameObject guidingHandBucket;

    public void intoAnchor()
    {
        if (!mm.GetStep("Get equipment", "Get splinter pin").IsCompeleted())
        {
            guidingHandSplint.SetActive(true);
        }
        if (!mm.GetStep("Get equipment", "Get rope").IsCompeleted())
        {
            guidingHandRope.SetActive(true);
        }
        if (!mm.GetStep("Get equipment", "Get bucket and spade").IsCompeleted())
        {
            guidingHandBucket.SetActive(true);
        }
    }
}
