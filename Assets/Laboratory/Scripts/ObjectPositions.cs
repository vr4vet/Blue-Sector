using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPositions : MonoBehaviour
{
    public static ObjectPositions Instance { get; private set; }

    public GameObject basket;
    public GameObject fish;
    public GameObject handheldCounter;
    public GameObject microscopeSlide;
    public GameObject planktonNotepad;
    public GameObject chaetocerosPoster;
    public GameObject pseudoNitzschiaPoster;
    public GameObject skeletonemaPoster;
    public GameObject planktonBasicsPoster;

    [HideInInspector] public Vector3 _basketPosition;
    [HideInInspector] public Quaternion _basketRotation;
    [HideInInspector] public Vector3 _handheldCounterPosition;
    [HideInInspector] public Quaternion _handheldCounterRotation;
    [HideInInspector] public Vector3 _fishPosition;
    [HideInInspector] public Quaternion _fishRotation;
    [HideInInspector] public Vector3 _microscopeSlidePosition;
    [HideInInspector] public Quaternion _microscopeSlideRotation;
    [HideInInspector] public Vector3 _planktonNotepadPosition;
    [HideInInspector] public Quaternion _planktonNotepadRotation;
    [HideInInspector] public Vector3 _chaetocerosPosterPosition;
    [HideInInspector] public Quaternion _chaetocerosPosterRotation;
    [HideInInspector] public Vector3 _pseudoNitzschiaPosterPosition;
    [HideInInspector] public Quaternion _pseudoNitzschiaPosterRotation;
    [HideInInspector] public Vector3 _skeletonemaPosterPosition;
    [HideInInspector] public Quaternion _skeletonemaPosterRotation;
    [HideInInspector] public Vector3 _planktonBasicsPosterPosition;
    [HideInInspector] public Quaternion _planktonBasicsPosterRotation;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _basketPosition = basket.transform.position;
        _basketRotation = basket.transform.rotation;
        _handheldCounterPosition = handheldCounter.transform.position;
        _handheldCounterRotation = handheldCounter.transform.rotation;
        _fishPosition = fish.transform.position;
        _fishRotation = fish.transform.rotation;
        _microscopeSlidePosition = microscopeSlide.transform.position;
        _microscopeSlideRotation = microscopeSlide.transform.rotation;
        _planktonNotepadPosition = planktonNotepad.transform.position;
        _planktonNotepadRotation = planktonNotepad.transform.rotation;
        _chaetocerosPosterPosition = chaetocerosPoster.transform.position;
        _chaetocerosPosterRotation = chaetocerosPoster.transform.rotation;
        _pseudoNitzschiaPosterPosition = pseudoNitzschiaPoster.transform.position;
        _pseudoNitzschiaPosterRotation = pseudoNitzschiaPoster.transform.rotation;
        _skeletonemaPosterPosition = skeletonemaPoster.transform.position;
        _skeletonemaPosterRotation = skeletonemaPoster.transform.rotation;
        _planktonBasicsPosterPosition = planktonBasicsPoster.transform.position;
        _planktonBasicsPosterRotation = planktonBasicsPoster.transform.rotation;
    }
}
