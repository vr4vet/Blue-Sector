using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weight : MonoBehaviour
{
    [Header("Weight settings")]
    public bool RandomWeight;

    [SerializeField]
    [Tooltip("The minimum weight that should be allowed if weight is random")]
    [Range(0, 10)]
    private float minWeight;

    [SerializeField]
    [Tooltip("The maximum weight that should be allowed if weight is random")]
    [Range(0, 10)]
    private float maxWeight;

    [SerializeField]
    private float _objectWeight;
    public float ObjectWeight
    {
        get { return _objectWeight; }
        set { _objectWeight = value; }
    }

    void Start()
    {
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
}
