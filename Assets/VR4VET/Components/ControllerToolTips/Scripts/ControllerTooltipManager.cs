using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
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

    private ControllerButtonsLeft _controllerButtonsLeft = new();
    private ControllerButtonsRight _controllerButtonsRight = new();
    private Transform _controllerModelLeft, _controllerModelRight;

    private int _questControllerModelIndex = 0;

    private Vector3 _averageLeftButtonPos = Vector3.zero;
    private Vector3 _averageRightButtonPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.PlayerHandModelsLoaded.AddListener(OnHandModelsReady);
    }

    private bool _oculusModelsActive = false;
    public void SetOculusHandModels()
    {
        _player.transform.GetChild(0).GetComponent<HandModelSelector>().ChangeHandsModel(_questControllerModelIndex);
        _oculusModelsActive = true;
    }

    public void SetDefaultHandModels()
    {
        _player.transform.GetChild(0).GetComponent<HandModelSelector>().ChangeHandsModel(0);
        _oculusModelsActive = false;
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
        i = 0;
        foreach (Transform model in _player.transform.GetComponentInChildren<HandModelSelector>().RightHandGFXHolder)
        {
            if (model.name.Equals("ControllerReferences_Right"))
            {
                _controllerModelRight = model;

                // making sure both models have the same index/position in hierarchy
                if (_questControllerModelIndex != i)
                    Debug.LogError("Oculus models do not have the same index/position in hierarhcy");
            }
            i++;
        }

        _averageLeftButtonPos = CalculateAverageButtonPosition(false);
        _averageRightButtonPos = CalculateAverageButtonPosition(true);

        // finding and setting up left controller buttons
        foreach (Transform button in _controllerModelLeft.Find("OculusTouchForQuest2_Left/left_oculus_controller_world"))
        {
            switch (button.name)
            {
                case "b_button_oculus":
                    _controllerButtonsLeft.oculus = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, false));
                    break;
                case "b_button_x":
                    _controllerButtonsLeft.x = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, false));
                    break;
                case "b_button_y":
                    _controllerButtonsLeft.y = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, false));
                    break;
                case "b_thumbstick":
                    _controllerButtonsLeft.thumbstick = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, false));
                    break;
                case "b_trigger_front":
                    _controllerButtonsLeft.trigger_front = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, false));
                    break;
                case "b_trigger_grip":
                    _controllerButtonsLeft.trigger_grip = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, false));
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
                    _controllerButtonsRight.b = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, true));
                    break;
                case "b_button_a":
                    _controllerButtonsRight.a = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, true));
                    break;
                case "b_button_b":
                    _controllerButtonsRight.b = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, true));
                    break;
                case "b_thumbstick":
                    _controllerButtonsRight.thumbstick = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, true));
                    break;
                case "b_trigger_front":
                    _controllerButtonsRight.trigger_front = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, true));
                    break;
                case "b_trigger_grip":
                    _controllerButtonsRight.trigger_grip = button;
                    SetUpToolTip(button, CalculateFinalTooltipPosition(button, true));
                    break;
                default:
                    Debug.LogError("Could not find all Oculus controller buttons");
                    break;
            }
        }
    }

    private void SetUpToolTip(Transform button, Vector3 toolTipPos)
    {
        BezierCurve curve = button.AddComponent<BezierCurve>();
        LineRenderer line = button.AddComponent<LineRenderer>();

        line.material = LineMaterial;
        line.startWidth = .004f;
        line.endWidth = .004f;
        line.startColor = new Color32(0x00, 0x8C, 0xFA, 0xFF);
        curve.SetPoints(button, _player.transform.GetChild(0).transform);
        curve.SetCurveFactor(.5f, .5f);
        curve.SetSegmentCount(32);

        //GameObject toolTip = GameObject.Instantiate(ToolTipPrefab, )
        
    }

    /// <summary>
    /// This calculates the average position between all the buttons of a given controller.
    /// It assists in the process of placing the tooltips relative to each button.
    /// It is assumed that a controller has 6 buttons.
    /// The average position of all buttons on the right controller is returned if "right" is true.
    /// Otherwise the average position of buttons on the left controller is returned.
    /// </summary>
    /// <param name="right"></param>
    /// <returns></returns>
    private Vector3 CalculateAverageButtonPosition(bool right)
    {
        string path;        
        if (!right)
            path = "OculusTouchForQuest2_Left/left_oculus_controller_world";
        else
            path = "OculusTouchForQuest2_Right/right_oculus_controller_world";

        Vector3 average = Vector3.zero;
        foreach (Transform button in !right ? _controllerModelLeft.Find(path) : _controllerModelRight.Find(path))
        {
            average += button.localPosition;
        }
        average /= 6;

        return average;
    }

    private Vector3 CalculateFinalTooltipPosition(Transform button, bool right)
    {        
        float distX = (!right ? _averageLeftButtonPos.x : _averageRightButtonPos.x) - button.localPosition.x;
        float distY = (!right ? _averageLeftButtonPos.y : _averageRightButtonPos.y) - button.localPosition.y;
        float distZ = (!right ? _averageLeftButtonPos.z : _averageRightButtonPos.z) - button.localPosition.z;

        Debug.Log(button.name + " " + new Vector3(distX, distY, distZ));
        return new Vector3(distX, distY, distZ);
    }

}
