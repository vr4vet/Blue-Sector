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

    // variables for configuring each tooltip's position relative to its button on the left controller using the inspector
    [Header("Left controller button tooltip offsets")]
    [SerializeField] private Vector3 OculusLeft = new(-.05f, .015f, -.03f);
    [SerializeField] private Vector3 X = new(-.08f, .03f, 0f);
    [SerializeField] private Vector3 Y = new(.04f, .05f, .01f);
    [SerializeField] private Vector3 ThumbstickLeft = new(.03f, .1f, .05f);
    [SerializeField] private Vector3 TriggerFrontLeft = new(.08f, -.01f, .02f);
    [SerializeField] private Vector3 TriggerGripLeft = new(.08f, .03f, -.02f);

    // variables for configuring each tooltip's position relative to its button on the right controller using the inspector
    [Header("Right controller button tooltip offsets")]
    [SerializeField] private Vector3 OculusRight = new(.05f, .015f, -.03f);
    [SerializeField] private Vector3 A = new(.08f, .03f, 0f);
    [SerializeField] private Vector3 B = new(-.04f, .05f, .01f);
    [SerializeField] private Vector3 ThumbstickRight = new(-.03f, .1f, .05f);
    [SerializeField] private Vector3 TriggerFrontRight = new(-.08f, -.01f, .02f);
    [SerializeField] private Vector3 TriggerGripRight = new(-.08f, .03f, -.02f);

    // the player rig
    private GameObject _player;

    // keep track of hand controller models and their button models
    private ControllerButtonsTransforms _controllerButtonsLeft = new(), _controllerButtonsRight = new();
    private GameObject _controllerModelLeft, _controllerModelRight;
    private List<InterractableObject> _interractableObjectsLeft = new(), _interractableObjectsRight = new();

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
    /// Adds the object and its tooltips to a list keeping track of currently intersecting ControllerTooltipActivators.
    /// </summary>
    /// <param name="buttonMappings"></param>
    /// <param name="rightHand"></param>
    public void OnHandEntered(Transform InterractableObject, List<ButtonActionMapping> buttonMappings, ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.None)
        {
            Debug.LogError("The provided controller hand must be either ControllerHand.Left or ControllerHand.Right!");
            return;
        }

        if (controllerHand == ControllerHand.Left)
        {
            List<HandControllerToolTip> controllerTooltips = new();
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
                    HandControllerToolTip toolTip = button.transform.GetChild(1).GetComponent<HandControllerToolTip>();
                    controllerTooltips.Add(toolTip);
                    if (mapping.Action == ButtonActions.None) // deactivate tooltip if button has no action
                    {
                        toolTip.Close();
                    }
                    else // otherwise make tooltip display the selected action
                    {
                        _controllerModelLeft.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                        GameManager.Instance.LeftHandGameObj.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                        toolTip.Open();
                        toolTip.GetComponentInChildren<TMP_Text>().text = mapping.Action.ToString();
                    }
                }
                else // something wrong happened as button could not be mapped
                    Debug.LogError("Could not map all left hand controller buttons");
            }

            _interractableObjectsLeft.Add(new InterractableObject(InterractableObject, controllerTooltips));
        }
        else if (controllerHand == ControllerHand.Right)
        {
            List<HandControllerToolTip> controllerTooltips = new();
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
                    HandControllerToolTip toolTip = button.transform.GetChild(1).GetComponent<HandControllerToolTip>(); 
                    controllerTooltips.Add(toolTip);
                    if (mapping.Action == ButtonActions.None) // deactivate tooltip if button has no action
                    {
                        toolTip.Close();
                    }
                    else // otherwise make tooltip display the selected action
                    {
                        _controllerModelRight.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                        GameManager.Instance.RightHandGameObj.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
                        toolTip.Open();
                        toolTip.GetComponentInChildren<TMP_Text>().text = mapping.Action.ToString();
                    }
                }
                else // something wrong happened as button could not be mapped
                    Debug.LogError("Could not map all right hand controller buttons");
            }

            _interractableObjectsRight.Add(new InterractableObject(InterractableObject, controllerTooltips));
        }

    }

    /// <summary>
    /// Is called from an ControllerTooltipActivator when a hand exits its trigger.
    /// A list of currently intersecting ControllerTooltipActivators is scanned, and said object is removed from the list.
    /// The relevant tooltips are closed.
    /// </summary>
    /// <param name="InterractableObject"></param>
    /// <param name="buttonMappings"></param>
    /// <param name="controllerHand"></param>
    public void OnHandExited(Transform InterractableObject, List<ButtonActionMapping> buttonMappings, ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.None)
        {
            Debug.LogError("The provided controller hand must be either ControllerHand.Left or ControllerHand.Right!");
            return;
        }

        List<InterractableObject> interractableObjects = controllerHand == ControllerHand.Left ? _interractableObjectsLeft : _interractableObjectsRight;
        foreach (InterractableObject intObject in interractableObjects)
        {
            if (intObject.Object == InterractableObject)
            {
                foreach (HandControllerToolTip toolTip in intObject.Tooltips)
                    toolTip.Close();

                interractableObjects.Remove(intObject);
                break;
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
            _controllerModelLeft.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
        else if (controllerHand == ControllerHand.Right)
        {
            GameManager.Instance.RightHandGameObj.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            _controllerModelRight.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
        else
            Debug.LogError("The provided controller hand must be either ControllerHand.Left or ControllerHand.Right!");
    }

    /// <summary>
    /// Stores each Quest hand controller model's buttons in _controllerButtonsLeft and _controllerButtonsRight.
    /// </summary>
    private void OnHandModelsReady()
    {
        _player = GameManager.Instance.GetPlayerRig();

        // checking if two and only two Quest controller models are provided (one for each hand)
        if (OculusControllerModels.Count != 2)
        {
            Debug.LogError("There should be exactly 2 hand controller models (one for each hand), but there are " + OculusControllerModels.Count + "!");
            return;
        }

        // instantiating both Quest controller models as children of this ControllerTooltipManager object
        foreach (GameObject model in OculusControllerModels)
        {
            GameObject controllerModel = GameObject.Instantiate(model, transform);
            if (model.name.Contains("left"))
                _controllerModelLeft = controllerModel;
            else
                _controllerModelRight = controllerModel;

            // placing constraints onto model
            controllerModel.AddComponent<PositionConstraint>();
            controllerModel.AddComponent<RotationConstraint>();

            // giving layer 'Player' to allow real time lighting
            controllerModel.GetComponentInChildren<SkinnedMeshRenderer>().gameObject.layer = LayerMask.NameToLayer("Player");

            // hiding controller model
            //controllerModel.SetActive(false);
            controllerModel.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }

        // configuring position and rotation constraints for both Quest controller models to make them follow player hands
        foreach (Grabber grabber in _player.GetComponentsInChildren<Grabber>())
        {
            if (grabber.HandSide == ControllerHand.None)
            {
                Debug.LogError("One or both player hand grabbers have 'HandSide' set to None");
                return;
            }

            // finding the correct (left or right) hand controller constraints for this iteration of foreach loop
            PositionConstraint positionConstraint = grabber.HandSide == ControllerHand.Left 
                                                    ? _controllerModelLeft.GetComponent<PositionConstraint>() 
                                                    : _controllerModelRight.GetComponent<PositionConstraint>(); 
            RotationConstraint rotationConstraint = grabber.HandSide == ControllerHand.Left
                                                    ? _controllerModelLeft.GetComponent<RotationConstraint>()
                                                    : _controllerModelRight.GetComponent<RotationConstraint>();

            // creating a constraint source (transform that the model should follow), and setting source transform to the XR rig's oculus hands' positions
            ConstraintSource constraintSource = new()
            {
                weight = 1,
                sourceTransform = grabber.HandSide == ControllerHand.Left 
                                                    ? grabber.transform.parent.Find("ModelsLeft/ControllerReferences_Left") 
                                                    : grabber.transform.parent.Find("ModelsRight/ControllerReferences_Right")
            };

            // adding constraint source to position and rotation constraints
            positionConstraint.AddSource(constraintSource);
            positionConstraint.constraintActive = true;
            rotationConstraint.AddSource(constraintSource);
            rotationConstraint.constraintActive = true;                
        }

        // find and store left controller button models and attach tooltips
        foreach (Transform button in _controllerModelLeft.transform.Find("left_quest2_controller_world"))
        {
            switch (button.name)
            {
                case "b_button_oculus":
                    _controllerButtonsLeft.oculus = button;
                    SetUpToolTip(button, button.localPosition + OculusLeft, ControllerHand.Left);
                    break;
                case "b_button_x":
                    _controllerButtonsLeft.primary = button;
                    SetUpToolTip(button, button.localPosition + X, ControllerHand.Left);
                    break;
                case "b_button_y":
                    _controllerButtonsLeft.secondary = button;
                    SetUpToolTip(button, button.localPosition + Y, ControllerHand.Left);
                    break;
                case "b_thumbstick":
                    _controllerButtonsLeft.thumbstick = button;
                    SetUpToolTip(button, button.localPosition + ThumbstickLeft, ControllerHand.Left);
                    break;
                case "b_trigger_front":
                    _controllerButtonsLeft.trigger_front = button;
                    SetUpToolTip(button, button.localPosition + TriggerFrontLeft, ControllerHand.Left);
                    break;
                case "b_trigger_grip":
                    _controllerButtonsLeft.trigger_grip = button;
                    SetUpToolTip(button, button.localPosition + TriggerGripLeft, ControllerHand.Left);
                    break;
                default:
                    Debug.LogError("Unknown object found among buttons");
                    break;
            }
        }

        // find and store right controller button models and attach tooltips
        foreach (Transform button in _controllerModelRight.transform.Find("right_quest2_controller_world"))
        {
            switch (button.name)
            {
                case "b_button_oculus":
                    _controllerButtonsRight.oculus = button;
                    SetUpToolTip(button, button.localPosition + OculusRight, ControllerHand.Right);
                    break;
                case "b_button_a":
                    _controllerButtonsRight.primary = button;
                    SetUpToolTip(button, button.localPosition + A, ControllerHand.Right);
                    break;
                case "b_button_b":
                    _controllerButtonsRight.secondary = button;
                    SetUpToolTip(button, button.localPosition + B, ControllerHand.Right);
                    break;
                case "b_thumbstick":
                    _controllerButtonsRight.thumbstick = button;
                    SetUpToolTip(button, button.localPosition + ThumbstickRight, ControllerHand.Right);
                    break;
                case "b_trigger_front":
                    _controllerButtonsRight.trigger_front = button;
                    SetUpToolTip(button, button.localPosition + TriggerFrontRight, ControllerHand.Right);
                    break;
                case "b_trigger_grip":
                    _controllerButtonsRight.trigger_grip = button;
                    SetUpToolTip(button, button.localPosition + TriggerGripRight, ControllerHand.Right);
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
    private void SetUpToolTip(Transform button, Vector3 toolTipPos, ControllerHand controllerSide)
    {        
        // create a tooltip and make it child of the button model
        GameObject toolTip = GameObject.Instantiate(ToolTipPrefab, Vector3.zero, button.rotation, button.transform);
        toolTip.transform.localScale /= 500;
        toolTip.transform.localPosition = toolTipPos;
        toolTip.GetComponent<HandControllerToolTip>().OpenPosition = toolTipPos; 
        toolTip.GetComponent<HandControllerToolTip>().HandSide = controllerSide;

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

        toolTip.SetActive(false);

        // attach the line's ends at the provided button and one of the tooltip's edges
        BezierCurve curve = toolTip.AddComponent<BezierCurve>();
        curve.SetPoints(button, CalculateTooltipAnchor(toolTip.transform));
        curve.SetCurveFactor(.1f, .1f);
        curve.SetSegmentCount(8);
        toolTip.SetActive(true);
        toolTip.GetComponent<HandControllerToolTip>().Close();
    }

    // decides which edge of tooltip the drawn line/curve should attach to
    private Transform CalculateTooltipAnchor(Transform tooltip)
    {        
        float distX = tooltip.localPosition.x;
        float distY = tooltip.localPosition.y;

        // select left or right edge if x-axis distance from button is larger than than y-axis distance, otherwise select bottom or top edge
        float maxDist = Mathf.Max(Mathf.Abs(distX), Mathf.Abs(distY));
        if (maxDist == Mathf.Abs(distX))
            return distX >= 0 ? tooltip.Find("LeftAnchor") : tooltip.Find("RightAnchor"); // return left edge if positioned to the right, right otherwise
        else
            return distY >= 0 ? tooltip.Find("BottomAnchor") : tooltip.Find("TopAnchor"); // return bottom edge if positioned above, top otherwise
    }

    /// <summary>
    /// This is called when a tooltip has finished moving "out of" the controller.
    /// </summary>
    /// <param name="controllerHand"></param>
    public void OnTooltipOpened(ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.None)
            Debug.LogError("Tooltip must have HandSide set to either left or right, but was " + controllerHand.ToString() + "!");
    }


    /// <summary>
    /// This is called when a tooltip has moved "back into" the controller, signalling that it may be time to hide the Quest controller
    /// and show the player's hand again (if the player's hand is not intersecting with any ControllerTooltipActivators).
    /// </summary>
    /// <param name="controllerHand"></param>
    public void OnTooltipClosed(ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.None)
            Debug.LogError("Tooltip must have HandSide set to either left or right, but was " + controllerHand.ToString() + "!");
        if (controllerHand == ControllerHand.Left && _interractableObjectsLeft.Count == 0)
            SetDefaultHandModel(ControllerHand.Left);
        if (controllerHand == ControllerHand.Right && _interractableObjectsRight.Count == 0)
            SetDefaultHandModel(ControllerHand.Right);
    }

}
