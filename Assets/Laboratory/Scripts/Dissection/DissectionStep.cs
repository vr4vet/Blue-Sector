using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DissectionStateFinished : UnityEvent<DissectionStep>
{
}

public class DissectionStep : MonoBehaviour
{
    [SerializeField] private float lineWidth = 1f;
    [SerializeField] private List<Transform> transforms = new();

    public DissectionStateFinished m_DissectionStateFinished = new();

    private LineRenderer _lineRenderer;
    private Collider _scalpel;
    private bool _scalpelEntered = false;
    private int _currentCutPoint = 0;
    private float _totalDistance = 0;
    private List<float> _distances = new() { 0 };

    private Gradient _colorGradient;
    private GradientColorKey[] _colorKeys;
    private GradientAlphaKey[] _alphaKeys;

    void Start()
    {
        //m_DissectionStateFinished ??= new();

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.widthMultiplier = lineWidth;
        _lineRenderer.colorGradient.mode = GradientMode.Fixed;
        _lineRenderer.positionCount = transforms.Count;
        _lineRenderer.SetPositions(transforms.Select(t => t.position).ToArray()); // passing an array of all the transform's positions

        // storing gradient data so arrows can be filled in when cutting
        _colorGradient = _lineRenderer.colorGradient;
        _colorKeys = _colorGradient.colorKeys;
        _alphaKeys = _colorGradient.alphaKeys;

        // storing total cut path distance, and distance at each point
        for (int i = 0; i < transforms.Count - 1; i++)
        {
            _totalDistance += Vector3.Distance(transforms[i].position, transforms[i + 1].position);
            _distances.Add(_totalDistance);
        }
    }

    private void Update()
    {
        if (_scalpelEntered)
        {
            if (_currentCutPoint < transforms.Count && Vector3.Distance(transforms[_currentCutPoint].position, _scalpel.transform.position) < .01f) // cut if close enough to current cutpoint in path
            {
                UpdateCutline(_currentCutPoint);
                _currentCutPoint++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Knife"))
        {
            _scalpel = other;
            _scalpelEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Knife"))
            _scalpelEntered = false;
    }


    private void UpdateCutline(int cutPoint)
    {
        _colorKeys[0].time = Mathf.Clamp(_distances[cutPoint] / _totalDistance, 0,  .99f);
        _colorGradient.SetKeys(_colorKeys, _alphaKeys);
        _lineRenderer.colorGradient = _colorGradient;

        // communicate that this dissection step is completed
        if (cutPoint >= transforms.Count - 1)
        {
            m_DissectionStateFinished.Invoke(this);
        }
    }

    // drawing cut point in Scene view for debugging purposes
    private void OnDrawGizmos()
    {
        if (_currentCutPoint < transforms.Count)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transforms[_currentCutPoint].position, .01f);
        }
    }
}
