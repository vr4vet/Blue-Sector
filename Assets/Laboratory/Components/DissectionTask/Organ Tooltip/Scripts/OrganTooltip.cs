using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganTooltip : MonoBehaviour
{
    [SerializeField] private Transform closedPosition;

    private bool _moving = false;

    private Vector3 _targetPosition;
    private Vector3 _targetScale;
    private Vector3 _openPosition;
    private Vector3 _openScale;
    private Vector3 _closedPosition;
    private Vector3 _velocityPosition;
    private Vector3 _velocityScale;

    // Start is called before the first frame update
    private void Start()
    {
        _openPosition = transform.localPosition;
        _openScale = transform.localScale;
        _closedPosition = closedPosition ? closedPosition.localPosition : Vector3.zero;
        GetComponent<LineRenderer>().widthMultiplier = .004f;
        CloseImmediately();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_moving)
            return;

        if (Vector3.Distance(_targetPosition, transform.localPosition) >= .0001f)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, _targetPosition, ref _velocityPosition, .3f);
            transform.localScale = Vector3.SmoothDamp(transform.localScale, _targetScale, ref _velocityScale, .3f);
        }
        else
            ReachedTarget();

    }

    public void Close()
    {
        InitiateMoving(false);
    }

    public void CloseImmediately()
    {
        transform.localPosition = _closedPosition;
        transform.localScale = Vector3.zero;
        Close();
    }

    public void Open()
    {
        GetComponent<Canvas>().enabled = true;
        GetComponent<LineRenderer>().enabled = true;

        InitiateMoving(true);
    }

    public void ToggleOpen()
    {
        if (_targetPosition == _closedPosition)
            Open();
        else
            Close();
    }

    private void ReachedTarget()
    {
        _moving = false;

        if (_targetPosition == _closedPosition)
        {
            GetComponent<Canvas>().enabled = false;
            GetComponent<LineRenderer>().enabled = false;
        }
    }

    private void InitiateMoving(bool opening)
    {
        if (opening)
        {
            _targetPosition = _openPosition;
            _targetScale = _openScale;
        }
        else
        {
            _targetPosition = _closedPosition;
            _targetScale = Vector3.zero;
        }

        _moving = true;
        
    }
}
