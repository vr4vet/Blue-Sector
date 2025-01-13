using BNG;
using System.Collections.Generic;
using TMPro;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.Animations;


public class ControllerTooltipManager : MonoBehaviour
{
    [Tooltip("The material used for the lines between buttons and tooltips")]
    [SerializeField] private Material LineMaterial;
    [Tooltip("The tooltip prefab that will hover above buttons")]
    [SerializeField] private GameObject ToolTipPrefab;
    [Tooltip("3D models of the Oculus hand controllers")]
    [SerializeField] private List<GameObject> OculusControllerModels;

    // the player rig
    private GameObject _player;

    // keep track of hand controller models and their button models
    private ControllerButtonsTransforms _controllerButtonsLeft = new(), _controllerButtonsRight = new();
    private GameObject _controllerModelLeft, _controllerModelRight;

    // Start is called before the first frame update
    void Start()
    {
        // wait until the GameManager instance is set up correctly before accessing the player and controller models
        GameManager.Instance.PlayerHandModelsLoaded.AddListener(OnHandModelsReady);
    }

    /// <summary>
    /// Takes a list of mappings between Quest controller buttons and their actions.
    /// Activates Quest hand controller model and tooltips that hover above each button showing their actions for an object.
    /// Is called by objects in the scene having the ControllerTooltipActivator prefab as a child object.
    /// </summary>
    /// <param name="buttonMappings"></param>
    /// <param name="rightHand"></param>
    public void SetOculusHandModel(List<ButtonActionMapping> buttonMappings, ControllerHand controllerHand)
    {
        //_player.transform.GetChild(0).GetComponent<HandModelSelector>().ChangeHandsModel(_questControllerModelIndex);
        //_oculusModelRightActive = true;

        if (controllerHand == ControllerHand.Left)
        {
            GameManager.Instance.LeftHandGameObj.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            _controllerModelLeft.SetActive(true);

            foreach (ButtonActionMapping mapping in buttonMappings)
            {
                // store all left hand button model transforms
                Transform button = mapping.Button switch
                {
                    ControllerButtons.OculusLeft => _controllerButtonsLeft.oculus,
                    ControllerButtons.ThumbstickLeft => _controllerButtonsLeft.thumbstick,
                    ControllerButtons.X => _controllerButtonsLeft.primary,
                    ControllerButtons.Y => _controllerButtonsLeft.secondary,
                    ControllerButtons.TriggerFrontLeft => _controllerButtonsLeft.trigger_front,
                    ControllerButtons.TriggerGripLeft => _controllerButtonsLeft.trigger_grip,
                    _ => null
                };

                if (button != null)
                {
                    if (mapping.Action == ButtonActions.None) // deactivate tooltip if button has no action
                        button.transform.GetChild(1).gameObject.SetActive(false);
                    else // otherwise make tooltip display the selected action
                        button.GetComponentInChildren<TMP_Text>().text = mapping.Action.ToString();
                }
                else // something wrong happened as button could not be mapped
                    Debug.LogError("Could not map all left hand controller buttons");
            }
        }
        else if (controllerHand == ControllerHand.Right)
        {
            GameManager.Instance.RightHandGameObj.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            _controllerModelRight.SetActive(true);

            foreach (ButtonActionMapping mapping in buttonMappings)
            {
                // store all right hand button model transforms
                Transform button = mapping.Button switch
                {
                    ControllerButtons.OculusRight => _controllerButtonsRight.oculus,
                    ControllerButtons.ThumbstickRight => _controllerButtonsRight.thumbstick,
                    ControllerButtons.A => _controllerButtonsRight.primary,
                    ControllerButtons.B => _controllerButtonsRight.secondary,
                    ControllerButtons.TriggerFrontRight => _controllerButtonsRight.trigger_front,
                    ControllerButtons.TriggerGripRight => _controllerButtonsRight.trigger_grip,
                    _ => null
                };

                if (button != null)
                {
                    if (mapping.Action == ButtonActions.None) // deactivate tooltip if button has no action
                        button.transform.GetChild(1).gameObject.SetActive(false);
                    else // otherwise make tooltip display the selected action
                        button.GetComponentInChildren<TMP_Text>().text = mapping.Action.ToString();
                }
                else // something wrong happened as button could not be mapped
                    Debug.LogError("Could not map all right hand controller buttons");
            }
        }
        
    }

    /// <summary>
    /// Deactivates Quest hand controller models and buttons tooltips.
    /// </summary>
    public void SetDefaultHandModel(ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.Left)
        {
            GameManager.Instance.LeftHandGameObj.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            _controllerModelLeft.SetActive(false);
        }
        else if (controllerHand == ControllerHand.Right)
        {
            GameManager.Instance.RightHandGameObj.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            _controllerModelRight.SetActive(false);
        }
    }

    /// <summary>
    /// Stores each Quest hand controller model's buttons in _controllerButtonsLeft and _controllerButtonsRight.
    /// </summary>
    private void OnHandModelsReady()
    {
        _player = GameManager.Instance.GetPlayerRig();

        // setting up Quest hand controller models
        for (int i = 0; i < OculusControllerModels.Count; i++)
        {
            GameObject controllerModel = GameObject.Instantiate(OculusControllerModels[i], transform);
            if (OculusControllerModels[i].name.Contains("left"))
                _controllerModelLeft = controllerModel;
            else
                _controllerModelRight = controllerModel;

            // placing constraints onto model
            controllerModel.AddComponent<PositionConstraint>();
            controllerModel.AddComponent<RotationConstraint>();

            // giving layer 'Player' to allow real time lighting
            controllerModel.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = LayerMask.NameToLayer("Player");

            controllerModel.SetActive(false);

        }

        // placing position and rotation constraints on Quest controller models to make them follow player hands
        foreach (Grabber grabber in _player.GetComponentsInChildren<Grabber>())
        {
            ConstraintSource constraintSource = new();
            constraintSource.sourceTransform = grabber.transform;
            constraintSource.weight = 1;
            PositionConstraint positionConstraint;
            RotationConstraint rotationConstraint;

            if (grabber.HandSide == ControllerHand.Left)
            {
                positionConstraint = _controllerModelLeft.GetComponent<PositionConstraint>();
                rotationConstraint = _controllerModelLeft.GetComponent<RotationConstraint>();
            }
            else if (grabber.HandSide == ControllerHand.Right)
            {
                positionConstraint = _controllerModelRight.GetComponent<PositionConstraint>();
                rotationConstraint = _controllerModelRight.GetComponent<RotationConstraint>();
            }
            else
            {
                Debug.LogError("One or both hand grabbers have 'HandSide' set to None");
                return;
            }
            positionConstraint.AddSource(constraintSource);
            positionConstraint.constraintActive = true;
            rotationConstraint.AddSource(constraintSource);
            rotationConstraint.constraintActive = true;
                
        }

        // finding and setting up left controller buttons
        foreach (Transform button in _controllerModelLeft.transform.Find("left_quest2_controller_world"))
        {
            switch (button.name)
            {
                case "b_button_oculus":
                    _controllerButtonsLeft.oculus = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(-.05f, .015f, -.03f));
                    break;
                case "b_button_x":
                    _controllerButtonsLeft.primary = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(-.08f, .03f, 0f));
                    break;
                case "b_button_y":
                    _controllerButtonsLeft.secondary = button;
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
                    Debug.LogError("Unknown object found among buttons");
                    break;
            }
        }

        // finding and setting up right controller buttons
        foreach (Transform button in _controllerModelRight.transform.Find("right_quest2_controller_world"))
        {
            switch (button.name)
            {
                case "b_button_oculus":
                    _controllerButtonsRight.oculus = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(.05f, .015f, -.03f));
                    break;
                case "b_button_a":
                    _controllerButtonsRight.primary = button;
                    SetUpToolTip(button, button.localPosition + new Vector3(.08f, .03f, 0f));
                    break;
                case "b_button_b":
                    _controllerButtonsRight.secondary = button;
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
                    Debug.LogError("Unknown object found among buttons");
                    break;
            }
        }
    }

    /// <summary>
    /// Sets up a tooltip above the provided button at the provided position and creates a line/curve between button and tooltip
    /// </summary>
    /// <param name="button"></param>
    /// <param name="toolTipPos"></param>
    private void SetUpToolTip(Transform button, Vector3 toolTipPos)
    {        
        // create a tooltip and make it child of the button model
        GameObject toolTip = GameObject.Instantiate(ToolTipPrefab, Vector3.zero, button.rotation);
        toolTip.transform.SetParent(button.transform);
        toolTip.transform.localScale /= 500;
        toolTip.transform.localPosition = toolTipPos;

        // create a line
        LineRenderer line = toolTip.AddComponent<LineRenderer>();
        line.material = LineMaterial;
        line.startWidth = .003f;
        line.endWidth = .003f;
        line.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        line.receiveShadows = false;
        
        // create a blue gradient and apply it to line
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

        // attach the line's ends at the provided button and one of the tooltip's edges
        BezierCurve curve = toolTip.AddComponent<BezierCurve>();
        curve.SetPoints(button, CalculateTooltipAnchor(button, toolTip.transform));
        curve.SetCurveFactor(.1f, .1f);
        curve.SetSegmentCount(8);   
    }

    // decides which edge of tooltip the drawn line/curve should attach to
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
