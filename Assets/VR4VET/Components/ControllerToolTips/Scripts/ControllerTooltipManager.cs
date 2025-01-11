using BNG;
using System.Collections.Generic;
using TMPro;
using Unity.VRTemplate;
using UnityEngine;


public class ControllerTooltipManager : MonoBehaviour
{
    private GameObject _player;
    [SerializeField] private Material LineMaterial;
    [SerializeField] private GameObject ToolTipPrefab;

    private struct ControllerButtonsLeft
    {
        public Transform oculus, thumbstick, x, y, trigger_front, trigger_grip;
    }
    private struct ControllerButtonsRight
    {
        public Transform oculus, thumbstick, a, b, trigger_front, trigger_grip;
    }

    public enum ControllerButtons
    {
        OculusLeft, ThumbstickLeft, X, Y, TriggerFrontLeft, TriggerGripLeft, OculusRight, ThumbstickRight, A, B, TriggerFrontRight, TriggerGripRight
    }
    public enum ButtonFunctions
    {
        None, Grab, Click, Start, Open
    }

    // struct used create a list of mapped buttons in ControllerTooltipActivator.cs
    public struct ButtonMapping
    {
        public ControllerButtons button;
        public ButtonFunctions function;

        // constructor
        public ButtonMapping(ControllerButtons btn, ButtonFunctions func)
        {
            button = btn;
            function = func;
        }

    }

    private ControllerButtonsLeft _controllerButtonsLeft = new();
    private ControllerButtonsRight _controllerButtonsRight = new();
    private Transform _controllerModelLeft, _controllerModelRight;
    private int _questControllerModelIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.PlayerHandModelsLoaded.AddListener(OnHandModelsReady);
    }

    private bool _oculusModelsActive = false;
    public void SetOculusHandModels(List<ControllerTooltipManager.ButtonMapping> buttonMappings)
    {
        if (!_oculusModelsActive)
        {
            _player.transform.GetChild(0).GetComponent<HandModelSelector>().ChangeHandsModel(_questControllerModelIndex);
            _oculusModelsActive = true;

            foreach (ButtonMapping mapping in buttonMappings)
            {
                //Debug.Log(mapping.button + " " + mapping.function);
                Transform button = mapping.button switch
                {
                    ControllerButtons.OculusLeft => _controllerButtonsLeft.oculus,
                    ControllerButtons.ThumbstickLeft => _controllerButtonsLeft.thumbstick,
                    ControllerButtons.X => _controllerButtonsLeft.x,
                    ControllerButtons.Y => _controllerButtonsLeft.y,
                    ControllerButtons.TriggerFrontLeft => _controllerButtonsLeft.trigger_front,
                    ControllerButtons.TriggerGripLeft => _controllerButtonsLeft.trigger_grip,
                    ControllerButtons.OculusRight => _controllerButtonsRight.oculus,
                    ControllerButtons.ThumbstickRight => _controllerButtonsRight.thumbstick,
                    ControllerButtons.A => _controllerButtonsRight.a,
                    ControllerButtons.B => _controllerButtonsRight.b,
                    ControllerButtons.TriggerFrontRight => _controllerButtonsRight.trigger_front,
                    ControllerButtons.TriggerGripRight => _controllerButtonsRight.trigger_grip,
                    _ => null,
                };

                if (button != null)
                {
                    if (mapping.function == ButtonFunctions.None)
                        button.GetChild(0).gameObject.SetActive(false);
                    else
                        button.GetComponentInChildren<TMP_Text>().text = mapping.function.ToString();
                }
                else
                    Debug.LogError("Could not map all buttons");
            }
        }
    }

    public void SetDefaultHandModels()
    {
        if (_oculusModelsActive)
        {
            _player.transform.GetChild(0).GetComponent<HandModelSelector>().ChangeHandsModel(0);
            _oculusModelsActive = false;
        }
    }

    public bool OculusModelsActive()
    {
        return _oculusModelsActive;
    }

    private void OnHandModelsReady()
    {
        _player = GameManager.Instance.GetPlayerRig();

        // finding controller model left
        int i = 0;
        foreach (Transform model in _player.transform.GetComponentInChildren<HandModelSelector>().LeftHandGFXHolder)
        {
            if (model.name.Equals("ControllerReferences_Left"))
            {
                _controllerModelLeft = model;
                _questControllerModelIndex = i;
            }
            i++;
        }

        // finding controller model right
        _controllerModelRight = _player.transform.GetComponentInChildren<HandModelSelector>().RightHandGFXHolder.GetChild(_questControllerModelIndex);
        if (!_controllerModelRight.name.Equals("ControllerReferences_Right"))
            Debug.LogError("Oculus models' position in hierarhcy differ between hands");

        // finding and setting up left controller buttons
        foreach (Transform button in _controllerModelLeft.Find("OculusTouchForQuest2_Left/left_oculus_controller_world"))
        {
            switch (button.name)
            {
                case "b_button_oculus":
                    _controllerButtonsLeft.oculus = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(-.05f, .015f, -.03f));
                    break;
                case "b_button_x":
                    _controllerButtonsLeft.x = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(-.08f, .03f, 0f));
                    break;
                case "b_button_y":
                    _controllerButtonsLeft.y = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(.04f, .05f, .01f));
                    break;
                case "b_thumbstick":
                    _controllerButtonsLeft.thumbstick = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(.03f, .1f, .05f));
                    break;
                case "b_trigger_front":
                    _controllerButtonsLeft.trigger_front = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(.08f, -.01f, .02f));
                    break;
                case "b_trigger_grip":
                    _controllerButtonsLeft.trigger_grip = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(.08f, .03f, -.02f));
                    break;
                default:
                    Debug.LogError("Could not find all Oculus controller buttons");
                    break;
            }
        }

        // finding and setting up right controller buttons
        foreach (Transform button in _controllerModelRight.Find("OculusTouchForQuest2_Right/right_oculus_controller_world"))
        {
            switch (button.name)
            {
                case "b_button_oculus":
                    _controllerButtonsRight.oculus = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(.05f, .015f, -.03f));
                    break;
                case "b_button_a":
                    _controllerButtonsRight.a = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(.08f, .03f, 0f));
                    break;
                case "b_button_b":
                    _controllerButtonsRight.b = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(-.04f, .05f, .01f));
                    break;
                case "b_thumbstick":
                    _controllerButtonsRight.thumbstick = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(-.03f, .1f, .05f));
                    break;
                case "b_trigger_front":
                    _controllerButtonsRight.trigger_front = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(-.08f, -.01f, .02f));
                    break;
                case "b_trigger_grip":
                    _controllerButtonsRight.trigger_grip = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(-.08f, .03f, -.02f));
                    break;
                default:
                    Debug.LogError("Could not find all Oculus controller buttons");
                    break;
            }
        }
    }

    private void SetUpToolTip(Transform button, Vector3 toolTipPos)
    {        
        GameObject toolTip = GameObject.Instantiate(ToolTipPrefab, Vector3.zero, button.rotation);
        toolTip.transform.SetParent(button.transform);
        toolTip.transform.localScale /= 500;
        toolTip.transform.localPosition = toolTipPos;

        LineRenderer line = toolTip.AddComponent<LineRenderer>();
        line.material = LineMaterial;
        line.startWidth = .003f;
        line.endWidth = .003f;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        colorKeys[0].color = new Color32(0x00, 0x8C, 0xFA, 0xFF);
        colorKeys[0].time = 0f;
        colorKeys[1].color = new Color32(0x00, 0x8C, 0xFA, 0xFF);
        colorKeys[1].time = 1f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[4];
        alphaKeys[0].alpha = 0f;
        alphaKeys[0].time = .075f;
        alphaKeys[1].alpha = 1f;
        alphaKeys[1].time = .18f;
        alphaKeys[2].alpha = 1f;
        alphaKeys[2].time = .96f;
        alphaKeys[3].alpha = 0f;
        alphaKeys[3].time = 1f;

        Gradient colorGradient = new();
        colorGradient.SetKeys(colorKeys, alphaKeys);
        line.colorGradient = colorGradient;

        BezierCurve curve = toolTip.AddComponent<BezierCurve>();
        curve.SetPoints(button, CalculateTooltipAnchor(button, toolTip.transform));
        curve.SetCurveFactor(.1f, .1f);
        curve.SetSegmentCount(8);   
    }


    private Transform CalculateTooltipAnchor(Transform button, Transform tooltip)
    {        
        float distX = tooltip.localPosition.x;
        float distY = tooltip.localPosition.y;

        Transform anchor;

        float maxDist = Mathf.Max(Mathf.Abs(distX), Mathf.Abs(distY));
        if (maxDist == Mathf.Abs(distX))
            anchor = distX >= 0 ? tooltip.Find("LeftAnchor") : tooltip.Find("RightAnchor");
        else
            anchor = distY >= 0 ? tooltip.Find("BottomAnchor") : tooltip.Find("TopAnchor");

        return anchor;
    }

}
