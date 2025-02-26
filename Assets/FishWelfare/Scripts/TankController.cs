using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public InspectionTaskManager inspectionTaskManager;
    public bool isGoal = false;
    public float sedativeConsentration = 0f;

public TutorialEntry tutorialEntry;
    // Start is called before the first frame update
    void Start()
    {
        inspectionTaskManager = FindObjectOfType<InspectionTaskManager>();
    }
}
