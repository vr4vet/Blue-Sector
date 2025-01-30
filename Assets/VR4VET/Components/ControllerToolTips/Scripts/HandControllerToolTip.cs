using BNG;
using System.Collections;
using TMPro;
using UnityEngine;

public class HandControllerToolTip : MonoBehaviour
{
    private bool _moving = false;
    private bool _closed = false;
    private Vector3 _target;
    private TMP_Text _label;

    [HideInInspector] public ControllerHand HandSide;
    [HideInInspector] public Vector3 OpenPosition;

    private ControllerTooltipManager _controllerTooltipManager;
    [SerializeField] private Transform _anchorLeft, _anchorBottom, _anchorRight, _anchorTop;

    // Start is called before the first frame update
    private void Start()
    {
        _controllerTooltipManager = GameObject.Find("ControllerToolTipManager").GetComponent<ControllerTooltipManager>();
        _label = GetComponentInChildren<TMP_Text>();

        // inform manager that tooltip is ready 
        _controllerTooltipManager.OnTooltipReady();
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
        {
            _closed = false;
            _controllerTooltipManager.OnTooltipStartOpening(HandSide);
        }

        while (!(Vector3.Distance(transform.localPosition, _target) < (_target != Vector3.zero ? 0.0005f : 0.025f)))
            yield return null;

        if (target == Vector3.zero)
        {
            _closed = true;
            GetComponent<Canvas>().enabled = false;
            GetComponent<LineRenderer>().enabled = false;
            _controllerTooltipManager.OnTooltipClosed(HandSide);
        }
        else
            _controllerTooltipManager.OnTooltipOpened(HandSide);
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
    /// Public method giving ControllerTooltipManager the ability to close tooltips immediately without interpolating position first
    /// </summary>
    public void CloseImmediately()
    {
        StopAllCoroutines();
        _closed = true;
        _target = Vector3.zero;
        transform.localPosition = Vector3.zero;
        GetComponent<Canvas>().enabled = false;
        GetComponent<LineRenderer>().enabled = false;
    }

    /// <summary>
    /// Public method giving ControllerTooltipManager the ability to open tooltips
    /// </summary>
    public void Open()
    {
        if (_closed)
        {
            StopAllCoroutines();
            GetComponent<Canvas>().enabled = true;
            GetComponent<LineRenderer>().enabled = true;
            StartCoroutine(InterpolateTowards(OpenPosition));                        
        }
    }

    /// <summary>
    /// Public method giving ControllerTooltipManager the ability to set the label's text content
    /// </summary>
    /// <param name="label"></param>
    public void SetLabel(string label) => _label.text = label;

    /// <summary>
    /// Returns the left tooltip anchor
    /// </summary>
    /// <returns></returns>
    public Transform AnchorLeft() => _anchorLeft;

    /// <summary>
    /// Returns the bottom tooltip anchor
    /// </summary>
    /// <returns></returns>
    public Transform AnchorBottom() => _anchorBottom;

    /// <summary>
    /// Returns the right tooltip anchor
    /// </summary>
    /// <returns></returns>
    public Transform AnchorRight() => _anchorRight;

    /// <summary>
    /// Returns the top tooltip anchor
    /// </summary>
    /// <returns></returns>
    /// 
    public Transform AnchorTop() => _anchorTop;
}
