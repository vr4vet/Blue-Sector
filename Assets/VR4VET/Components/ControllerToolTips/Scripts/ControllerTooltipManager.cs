using BNG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VRTemplate;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;


public class ControllerTooltipManager : MonoBehaviour
{
    [Tooltip("The material used for the lines between buttons and tooltips")]
    [SerializeField] private Material LineMaterial;
    [Tooltip("The tooltip prefab that will hover above buttons")]
    [SerializeField] private GameObject ToolTipPrefab;
    [Tooltip("3D models of the Oculus hand controllers")]
    [SerializeField] private List<GameObject> OculusControllerModels;
    [Tooltip("Place main menu/pause menu here to allow the player to toggle this accessibility feature on or off")]
    [SerializeField] private NewMenuManger MainMenu;
    [Tooltip("Select the Controller Tooltips Localization Table")]
    [SerializeField] private LocalizedStringTable LocalizedTooltipsStringTable;

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


    // prevent tooltips from being opened/closed before ready
    private int _tooltipsReady = 0;
    private int _tooltipsActiveLeft = 0, _tooltipsActiveRight = 0;

    // the player rig
    private GameObject _player;

    // keep track of hand controller models and their button models
    private ControllerButtonsTransforms _controllerButtonsLeft = new(), _controllerButtonsRight = new();
    private GameObject _controllerModelLeft, _controllerModelRight;
    private GameObject _handModelLeft, _handModelRight;
    private Grabber _grabberLeft, _grabberRight;

    // pre-allocated array for storing surrounding colliders
    private Collider[] _surroundingColliders = new Collider[10]; 

    // controls whether close objects will be scanned, controller models enabled/disabled etc., and is set in main menu/pause menu.
    private bool _accessibilityEnabled = true, _alwaysLabelTeleport = true;

    private List<ButtonActionMapping> _defaultButtonMappingsLeft = new()
    {
        new (ControllerButtons.OculusLeft, ButtonActions.None),
        new (ControllerButtons.ThumbstickLeft, ButtonActions.Teleport),
        new (ControllerButtons.X, ButtonActions.None),
        new (ControllerButtons.Y, ButtonActions.None),
        new (ControllerButtons.TriggerFrontLeft, ButtonActions.None),
        new (ControllerButtons.TriggerGripLeft, ButtonActions.None),
    };

    // Start is called before the first frame update
    void Start()
    {
        // wait until the GameManager instance is set up correctly before accessing the player and controller models
        GameManager.Instance.PlayerHandModelsLoaded.AddListener(OnHandModelsReady);

        if (MainMenu != null)
        {
            MainMenu.m_ControllerTooltipsToggled.AddListener(OnPauseMenuToggledTooltips);
            MainMenu.m_AlwaysLabelTeleportToggled.AddListener(OnPauseMenuToggledLabelTeleport);
        }

        if (LocalizedTooltipsStringTable.IsEmpty)
            Debug.LogWarning("Localized string table not set. Localization of controller tooltips will not work!");
    }

    /// <summary>
    /// Looks for tooltip activators within a 0.05 radius of the player's grabber
    /// and decides which one is closest. It then activates that object's relevant tooltips.
    /// Runs several times a second.
    /// </summary>
    /// <param name="controllerHand"></param>
    private IEnumerator FindClosestObject(ControllerHand controllerHand)
    {
        // don't try to manipulate/change anything before everything is ready
        if (_controllerModelLeft == null ||  _controllerModelRight == null || _tooltipsReady < 12) 
            yield return null;

        if (controllerHand == ControllerHand.None)
        {
            Debug.LogError("The provided controller hand must be either ControllerHand.Left or ControllerHand.Right!");
            yield return null;
        }

        Grabber grabber = controllerHand == ControllerHand.Left ? _grabberLeft : _grabberRight; // figure out which hand

        // once hand is figured out the following code (targeting that hand) runs several times a second
        while (true)
        {
            if (_accessibilityEnabled && !grabber.HeldGrabbable) // if tooltips are disabled or this hand holds something -> don't look for surrounding objects, close all tooltips
            {
                bool deactivated = false;
                int numOverlaps = Physics.OverlapSphereNonAlloc(grabber.transform.position, .05f, _surroundingColliders); // finding surrounding colliders (radius .05 is the same as grabber), and storing the amount to not iterate to "invalid" indicies

                List<ControllerTooltipActivator> activators = new(); 
                for (int i = 0; i < numOverlaps; i++)
                {
                    // hand has entered a deactivator and should not display any tooltips
                    if (_surroundingColliders[i].CompareTag("ControllerTooltipDeactivator")) 
                    {
                        deactivated = true;
                        break;
                    }

                    // find controller tooltip activators among colliders
                    ControllerTooltipActivator activator;
                    if (activator = _surroundingColliders[i].GetComponentInChildren<ControllerTooltipActivator>())
                        activators.Add(activator);
                }

                if (activators.Count > 0) // there are activators in player hand's vicinity
                {
                    float shortestDistance = Mathf.Infinity;
                    ControllerTooltipActivator closestActivator = activators[0];
                    foreach (ControllerTooltipActivator activator in activators) // find closest activator
                    {
                        float distance = Vector3.Distance(grabber.transform.position, activator.transform.position);
                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            closestActivator = activator;
                        }
                    }

                    if (TooltippedButtonsDown(closestActivator, controllerHand)) // close tooltips if player is pressing the correct buttons
                        CloseAllTooltips(controllerHand);
                    else
                    {
                        if (controllerHand == ControllerHand.Left)
                            SetUpTooltips(closestActivator.GetButtonMappingsLeft(), ControllerHand.Left);
                        else
                            SetUpTooltips(closestActivator.GetButtonMappingsRight(), ControllerHand.Right);
                    }
                }
                else
                {
                    // the left hand is special, since it has the option of always labelling the teleport stick
                    if (controllerHand == ControllerHand.Left)
                    {
                        if (!deactivated && _alwaysLabelTeleport && (InputBridge.Instance.LeftThumbstickAxis.magnitude <= .5f) && !(_grabberRight.HeldGrabbable && _grabberRight.HeldGrabbable.CompareTag("Tablet")))
                            SetUpTooltips(_defaultButtonMappingsLeft, controllerHand);
                        else
                            CloseAllTooltips(controllerHand);
                    }
                    else if (controllerHand == ControllerHand.Right)
                        CloseAllTooltips(controllerHand);
                }
            }
            else
                CloseAllTooltips(controllerHand);

            yield return new WaitForSecondsRealtime(.1f); // check 10 times a second
        }
    }

    /// <summary>
    /// Checks if one of the tooltipped buttons (buttons that have some action mapped by a nearby ControllerTooltipActivator) is currently held down. 
    /// This is used to hide tooltips and Quest hand controller models when the player presses one of the related buttons.
    /// Note: the oculus system buttons (the recessed buttons used to navigate the operating system or device) will not trigger this behaviour!
    /// </summary>
    /// <param name="activator"></param>
    /// <param name="controllerHand"></param>
    /// <returns></returns>
    private bool TooltippedButtonsDown(ControllerTooltipActivator activator, ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.None)
            return false;

        foreach (ButtonActionMapping mapping in controllerHand == ControllerHand.Left ? activator.GetButtonMappingsLeft() : activator.GetButtonMappingsRight())
        {
            if (mapping.Action != ButtonActions.None)
            {
                return mapping.Button switch
                {
                    ControllerButtons.ThumbstickLeft => InputBridge.Instance.LeftThumbstickAxis.magnitude > 0,// may need to be tweaked
                    ControllerButtons.A => InputBridge.Instance.AButtonDown,
                    ControllerButtons.B => InputBridge.Instance.BButtonDown,
                    ControllerButtons.TriggerFrontLeft => InputBridge.Instance.LeftTrigger > 0,// may need to be tweaked
                    ControllerButtons.TriggerGripLeft => InputBridge.Instance.LeftGrip > 0,// may need to be tweaked
                    ControllerButtons.ThumbstickRight => InputBridge.Instance.RightThumbstickAxis.magnitude > 0,// may need to be tweaked
                    ControllerButtons.X => InputBridge.Instance.XButtonDown,
                    ControllerButtons.Y => InputBridge.Instance.YButtonDown,
                    ControllerButtons.TriggerFrontRight => InputBridge.Instance.RightTrigger > 0,// may need to be tweaked
                    ControllerButtons.TriggerGripRight => InputBridge.Instance.RightGrip > 0,// may need to be tweaked
                    _ => false,
                };
            }
        }
        return false;
    }

    /// <summary>
    /// Takes a list of mappings between Quest controller buttons and their actions.
    /// Activates tooltips that hover above each button showing their actions for an object.
    /// </summary>
    /// <param name="buttonActionMappings"></param>
    /// <param name="controllerHand"></param>
    private void SetUpTooltips(List<ButtonActionMapping> buttonActionMappings, ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.None)
        {
            Debug.LogError("The provided controller hand must be either ControllerHand.Left or ControllerHand.Right!");
            return;
        }

        foreach (ButtonActionMapping mapping in buttonActionMappings)
        {
            // find the correct button model (on the provided hand) before configuring its tooltip
            Transform button = controllerHand == ControllerHand.Left
            ?
            mapping.Button switch
            {
                ControllerButtons.OculusLeft => _controllerButtonsLeft.Oculus,
                ControllerButtons.ThumbstickLeft => _controllerButtonsLeft.Thumbstick,
                ControllerButtons.X => _controllerButtonsLeft.Primary,
                ControllerButtons.Y => _controllerButtonsLeft.Secondary,
                ControllerButtons.TriggerFrontLeft => _controllerButtonsLeft.TriggerFront,
                ControllerButtons.TriggerGripLeft => _controllerButtonsLeft.TriggerGrip,
                _ => null
            }
            :
            mapping.Button switch
            {
                ControllerButtons.OculusRight => _controllerButtonsRight.Oculus,
                ControllerButtons.ThumbstickRight => _controllerButtonsRight.Thumbstick,
                ControllerButtons.A => _controllerButtonsRight.Primary,
                ControllerButtons.B => _controllerButtonsRight.Secondary,
                ControllerButtons.TriggerFrontRight => _controllerButtonsRight.TriggerFront,
                ControllerButtons.TriggerGripRight => _controllerButtonsRight.TriggerGrip,
                _ => null
            };

            if (button != null)
            {
                HandControllerToolTip toolTip = button.transform.GetChild(1).GetComponent<HandControllerToolTip>();
                if (mapping.Action == ButtonActions.None) // deactivate tooltip if button has no action
                    toolTip.Close();
                else
                {
                    toolTip.Open();

                    // get localized string value (translation) if string table is provided, otherwise use default (english) label
                    StringTable stringTable = !LocalizedTooltipsStringTable.IsEmpty ? LocalizedTooltipsStringTable.GetTable() : null;
                    toolTip.SetLabel(stringTable ? stringTable[mapping.Action.ToString()].LocalizedValue : mapping.Action.ToString());
                }
            }
            else // something wrong happened as button could not be mapped
                Debug.LogError("Could not map all left hand controller buttons");
        }
    }


    /// <summary>
    /// Closes all the provided hand's tooltips
    /// </summary>
    /// <param name="controllerHand"></param>
    private void CloseAllTooltips(ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.None)
        {
            Debug.LogError("The provided controller hand must be either ControllerHand.Left or ControllerHand.Right, but was ControllerHand.None!");
            return;
        }

        // kinda gross, but it is what it is...at least it doesn't run frequently.
        ControllerButtonsTransforms buttons = controllerHand == ControllerHand.Left ? _controllerButtonsLeft : _controllerButtonsRight;
        buttons.Oculus.GetComponentInChildren<HandControllerToolTip>().Close();
        buttons.Primary.GetComponentInChildren<HandControllerToolTip>().Close();
        buttons.Secondary.GetComponentInChildren<HandControllerToolTip>().Close();
        buttons.Thumbstick.GetComponentInChildren<HandControllerToolTip>().Close();
        buttons.TriggerFront.GetComponentInChildren<HandControllerToolTip>().Close();
        buttons.TriggerGrip.GetComponentInChildren<HandControllerToolTip>().Close();
    }

    /// <summary>
    /// Closes all the provided hand's tooltips immediately
    /// </summary>
    /// <param name="controllerHand"></param>
    private void CloseAllTooltipsImmediately(ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.None)
        {
            Debug.LogError("The provided controller hand must be either ControllerHand.Left or ControllerHand.Right, but was ControllerHand.None!");
            return;
        }

        // kinda gross, but it is what it is...at least it doesn't run frequently.
        ControllerButtonsTransforms buttons = controllerHand == ControllerHand.Left ? _controllerButtonsLeft : _controllerButtonsRight;
        buttons.Oculus.GetComponentInChildren<HandControllerToolTip>().CloseImmediately();
        buttons.Primary.GetComponentInChildren<HandControllerToolTip>().CloseImmediately();
        buttons.Secondary.GetComponentInChildren<HandControllerToolTip>().CloseImmediately();
        buttons.Thumbstick.GetComponentInChildren<HandControllerToolTip>().CloseImmediately();
        buttons.TriggerFront.GetComponentInChildren<HandControllerToolTip>().CloseImmediately();
        buttons.TriggerGrip.GetComponentInChildren<HandControllerToolTip>().CloseImmediately();
    }

    /// <summary>
    /// Activates Quest hand controller models
    /// <param name="controllerHand"/>
    /// </summary>
    private void SetQuestControllerModels(ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.Left)
        {
            _handModelLeft.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            _controllerModelLeft.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
        else if (controllerHand == ControllerHand.Right)
        {
            _handModelRight.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
            _controllerModelRight.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        }
        else
            Debug.LogError("The provided controller hand must be either ControllerHand.Left or ControllerHand.Right!");
    }

    /// <summary>
    /// Deactivates Quest hand controller
    /// </summary>
    /// <param name="controllerHand"></param>
    public void SetDefaultHandModel(ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.Left)
        {
            _handModelLeft.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
            _controllerModelLeft.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        }
        else if (controllerHand == ControllerHand.Right)
        {
            _handModelRight.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
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
        _handModelLeft = GameManager.Instance.LeftHandGameObj;
        _handModelRight = GameManager.Instance.RightHandGameObj;

        // the hand grabbers are used to set the Physics Sphere's location in the function FindClosestObject (in other words, the center point from which interactible objects are detected)
        List<Grabber> grabbers = _player.GetComponentsInChildren<Grabber>().ToList();
        _grabberLeft = grabbers.Find(x => x.transform.parent.name == "LeftController");
        _grabberRight = grabbers.Find(x => x.transform.parent.name == "RightController");

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
                    _controllerButtonsLeft.Oculus = button;
                    CreateToolTip(button, button.localPosition + OculusLeft, ControllerHand.Left);
                    break;
                case "b_button_x":
                    _controllerButtonsLeft.Primary = button;
                    CreateToolTip(button, button.localPosition + X, ControllerHand.Left);
                    break;
                case "b_button_y":
                    _controllerButtonsLeft.Secondary = button;
                    CreateToolTip(button, button.localPosition + Y, ControllerHand.Left);
                    break;
                case "b_thumbstick":
                    _controllerButtonsLeft.Thumbstick = button;
                    CreateToolTip(button, button.localPosition + ThumbstickLeft, ControllerHand.Left);
                    break;
                case "b_trigger_front":
                    _controllerButtonsLeft.TriggerFront = button;
                    CreateToolTip(button, button.localPosition + TriggerFrontLeft, ControllerHand.Left);
                    break;
                case "b_trigger_grip":
                    _controllerButtonsLeft.TriggerGrip = button;
                    CreateToolTip(button, button.localPosition + TriggerGripLeft, ControllerHand.Left);
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
                    _controllerButtonsRight.Oculus = button;
                    CreateToolTip(button, button.localPosition + OculusRight, ControllerHand.Right);
                    break;
                case "b_button_a":
                    _controllerButtonsRight.Primary = button;
                    CreateToolTip(button, button.localPosition + A, ControllerHand.Right);
                    break;
                case "b_button_b":
                    _controllerButtonsRight.Secondary = button;
                    CreateToolTip(button, button.localPosition + B, ControllerHand.Right);
                    break;
                case "b_thumbstick":
                    _controllerButtonsRight.Thumbstick = button;
                    CreateToolTip(button, button.localPosition + ThumbstickRight, ControllerHand.Right);
                    break;
                case "b_trigger_front":
                    _controllerButtonsRight.TriggerFront = button;
                    CreateToolTip(button, button.localPosition + TriggerFrontRight, ControllerHand.Right);
                    break;
                case "b_trigger_grip":
                    _controllerButtonsRight.TriggerGrip = button;
                    CreateToolTip(button, button.localPosition + TriggerGripRight, ControllerHand.Right);
                    break;
                default:
                    Debug.LogError("Unknown object found among buttons");
                    break;
            }
        }

        StartCoroutine(FindClosestObject(ControllerHand.Left));
        StartCoroutine(FindClosestObject(ControllerHand.Right));
    }

    /// <summary>
    /// Sets up a tooltip above the provided button at the provided position and creates a line/curve between button and tooltip
    /// </summary>
    /// <param name="button"></param>
    /// <param name="toolTipPos"></param>
    /// <param name="controllerSide"></param>
    private void CreateToolTip(Transform button, Vector3 toolTipPos, ControllerHand controllerSide)
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
        alphaKeys[0].time = .15f;
        alphaKeys[1].alpha = 1f;
        alphaKeys[1].time = .19f;
        alphaKeys[2].alpha = 1f;
        alphaKeys[2].time = .96f;
        alphaKeys[3].alpha = 0f;
        alphaKeys[3].time = 1f;

        Gradient colorGradient = new();
        colorGradient.SetKeys(colorKeys, alphaKeys);
        line.colorGradient = colorGradient;


        // attach the line's ends at the provided button and one of the tooltip's edges
        toolTip.SetActive(false); // doing this to prevent BezierCurve from running before its fields are set
        BezierCurve curve = toolTip.AddComponent<BezierCurve>();
        curve.SetPoints(button, CalculateTooltipAnchor(toolTip.transform));
        curve.SetCurveFactor(.1f, .1f);
        curve.SetSegmentCount(8);
        curve.SetUpdateTrackingType(BezierCurve.UpdateType.UpdateAndBeforeRender); // ensuring line doesn't lag behind when controller is moving
        toolTip.SetActive(true); // fields are set, so it's good to go
        toolTip.GetComponent<HandControllerToolTip>().CloseImmediately(); // prevent tooltip from being visible the first few frames
    }

    // decides which edge of tooltip the drawn line/curve should attach to
    private Transform CalculateTooltipAnchor(Transform tooltip)
    {        
        float distX = tooltip.localPosition.x;
        float distY = tooltip.localPosition.y;

        var tooltipScript = tooltip.GetComponent<HandControllerToolTip>();

        // select left or right edge if x-axis distance from button is larger than than y-axis distance, otherwise select bottom or top edge
        float maxDist = Mathf.Max(Mathf.Abs(distX), Mathf.Abs(distY));
        if (maxDist == Mathf.Abs(distX))
            return distX >= 0 ? tooltipScript.AnchorLeft() : tooltipScript.AnchorRight(); // return left edge if positioned to the right, right otherwise
        else
            return distY >= 0 ? tooltipScript.AnchorBottom() : tooltipScript.AnchorTop(); // return bottom edge if positioned above, top otherwise
    }

    /// <summary>
    /// Keeps track of how many tooltips are currently open
    /// and enable Quest controller hand model
    /// </summary>
    /// <param name="controllerHand"></param>
    public void OnTooltipStartOpening(ControllerHand controllerHand)
    {
        if (controllerHand == ControllerHand.None)
            Debug.LogError("Tooltip must have HandSide set to either left or right, but was " + controllerHand.ToString() + "!");

        if (controllerHand == ControllerHand.Left)
            _tooltipsActiveLeft++;
        if (controllerHand == ControllerHand.Right)
            _tooltipsActiveRight++;

        SetQuestControllerModels(controllerHand);
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

        // decrease active tooltips by one, but make sure it is not negative
        if (controllerHand == ControllerHand.Left)
            _tooltipsActiveLeft = Mathf.Max(_tooltipsActiveLeft - 1, 0);
        if (controllerHand == ControllerHand.Right)
            _tooltipsActiveRight = Mathf.Max(_tooltipsActiveRight - 1, 0);

        // show standard hand model if no tooltips
        if (_tooltipsActiveLeft <= 0)
            SetDefaultHandModel(ControllerHand.Left);
        if (_tooltipsActiveRight <= 0)
            SetDefaultHandModel(ControllerHand.Right);
    }

    /// <summary>
    /// This is called by each tooltip when it is ready, 
    /// which allows manager to wait until all tooltips are ready before activating or deactivating tooltips.
    /// </summary>
    public void OnTooltipReady() => _tooltipsReady++;

    /// <summary>
    /// Is triggered when this accessibility feature is toggled in pause menu. Controls whether tooltips appear or not.
    /// </summary>
    /// <param name="isOn"></param>
    public void OnPauseMenuToggledTooltips(bool isOn)
    {
        _accessibilityEnabled = isOn;

        // don't do things unless everything is set up and ready
        if (_tooltipsReady < 12)
            return;

        if (!isOn)
        {
            StopAllCoroutines();
            CloseAllTooltipsImmediately(ControllerHand.Left);
            CloseAllTooltipsImmediately(ControllerHand.Right);
            SetDefaultHandModel(ControllerHand.Left);
            SetDefaultHandModel(ControllerHand.Right);
        }
        else
        {
            StartCoroutine(FindClosestObject(ControllerHand.Left));
            StartCoroutine(FindClosestObject(ControllerHand.Right));
        }  
    }

    /// <summary>
    /// This is called when setting is changed or loaded from Pause Menu.
    /// </summary>
    /// <param name="isOn"></param>
    public void OnPauseMenuToggledLabelTeleport(bool isOn)
    {
        _alwaysLabelTeleport = isOn;
    }
}
