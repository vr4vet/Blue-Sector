using BNG;
using System.Collections;
using UnityEngine;

public class HandControllerToolTip : MonoBehaviour
{
    private bool _moving = false;
    private bool _closed = false;
    private Vector3 _target;
    public ControllerHand HandSide;
    public Vector3 OpenPosition;

    private ControllerTooltipManager _controllerTooltipManager;

    // Start is called before the first frame update
    private void Start()
    {
        _controllerTooltipManager = GameObject.Find("ControllerToolTipManager").GetComponent<ControllerTooltipManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_moving)
        {
            // move tooltip
            if (Vector3.Distance(transform.localPosition, _target) < (_target != Vector3.zero ? 0.0005f : 0.025f))
            {
                _moving = false;
                transform.localPosition = _target;
                return;
            }
            transform.localPosition = Vector3.Lerp(transform.localPosition, _target, 4f * Time.deltaTime);
        }
    }

    /// <summary>
    /// Make tooltip move towards target and tell ControllerTooltipManager when it's done.
    /// Used to move tooltip back and forth when opening and closing.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public IEnumerator InterpolateTowards(Vector3 target)
    {
        _target = target;
        _moving = true;

        if (target != Vector3.zero)
            _closed = false;

        while (!(Vector3.Distance(transform.localPosition, _target) < (_target != Vector3.zero ? 0.0005f : 0.025f)))
            yield return null;

        if (target == Vector3.zero)
        {
            Debug.Log("Closed");
            _closed = true;
            GetComponent<Canvas>().enabled = false;
            GetComponent<LineRenderer>().enabled = false;
            _controllerTooltipManager.OnTooltipClosed(HandSide);
        }
        else
        {
            _controllerTooltipManager.OnTooltipOpened(HandSide);
        }
    }

    /// <summary>
    /// Public method giving ControllerTooltipManager the ability to close tooltips
    /// </summary>
    public void Close()
    {
        if (!_closed)
        {
            StopAllCoroutines();
            StartCoroutine(InterpolateTowards(Vector3.zero));
        }
    }

    /// <summary>
    /// Public method giving ControllerTooltipManager the ability to open tooltips
    /// </summary>
    public void Open()
    {
            StopAllCoroutines();
            GetComponent<Canvas>().enabled = true;
            GetComponent<LineRenderer>().enabled = true;
            StartCoroutine(InterpolateTowards(OpenPosition));            
    }
}
