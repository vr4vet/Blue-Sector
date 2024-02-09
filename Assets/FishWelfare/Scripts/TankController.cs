using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public InspectionTaskManager inspectionTaskManager;
    public bool isGoal = false;
    private BoxCollider collider = new BoxCollider();
    private AudioSource waterSound;
    public float sedativeConsentration = 0f;

public TutorialEntry tutorialEntry;
    // Start is called before the first frame update
    void Start()
    {
        inspectionTaskManager = FindObjectOfType<InspectionTaskManager>();
        collider = GetComponent<BoxCollider>();
        waterSound = GetComponent<AudioSource>();
    }
}
