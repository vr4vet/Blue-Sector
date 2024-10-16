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

    public Vector3 _basketPosition;
    public Quaternion _basketRotation;
    public Vector3 _handheldCounterPosition;
    public Quaternion _handheldCounterRotation;
    public Vector3 _fishPosition;
    public Quaternion _fishRotation;
    public Vector3 _microscopeSlidePosition;
    public Quaternion _microscopeSlideRotation;
    
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
