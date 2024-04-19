using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGuidingHands : MonoBehaviour
{
    [SerializeField] private MaintenanceManager mm;
    [SerializeField] private GameObject guidingHandSplint;
    [SerializeField] private GameObject guidingHandRope;
    [SerializeField] private GameObject guidingHandBucket;

    void OnEnable()
    {
        if (!mm.GetStep("Hent Utstyr", "Hent splinter").IsCompeleted())
        {
            guidingHandSplint.SetActive(true);
        }
        if (!mm.GetStep("Hent Utstyr", "Hent tau").IsCompeleted())
        {
            guidingHandRope.SetActive(true);
        }
        if (!mm.GetStep("Hent Utstyr", "Hent bøtte og spade").IsCompeleted())
        {
            guidingHandBucket.SetActive(true);
        }
    }
}
