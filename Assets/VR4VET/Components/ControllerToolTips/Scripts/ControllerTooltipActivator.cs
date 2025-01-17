using BNG;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;


public class ControllerTooltipActivator : MonoBehaviour
{
    [Tooltip("The trigger area will be this many times larger than the bounds of the object")]
    [SerializeField] private float TriggerSizeFactor = 1.5f;
    [Tooltip("Place an object containing a collider component here to use that to trigger tooltips instead of an automatically generated one. Will not be scaled using TriggerSizeFactor")]
    [SerializeField] private Collider CustomColliderTrigger;

    private ControllerTooltipManager _controllerTooltipManager;

    [Header("Left controller buttons")]
    [SerializeField] private ButtonActions OculusLeft;
    [SerializeField] private ButtonActions ThumbstickLeft;
    [SerializeField] private ButtonActions X;
    [SerializeField] private ButtonActions Y;
    [SerializeField] private ButtonActions TriggerFrontLeft;
    [SerializeField] private ButtonActions TriggerGripLeft;

    [Header("Right controller buttons")]
    [SerializeField] private ButtonActions OculusRight;
    [SerializeField] private ButtonActions ThumbstickRight;
    [SerializeField] private ButtonActions A;
    [SerializeField] private ButtonActions B;
    [SerializeField] private ButtonActions TriggerFrontRight;
    [SerializeField] private ButtonActions TriggerGripRight;

    // lists of button mappings for both Oculus hand controllers
    private List<ButtonActionMapping> _buttonMappingsLeft, _buttonMappingsRight;

    // collider that will be sent to ControllerTooltipManager to determine distance from hand to object
    private Collider _colliderForDistanceComparison;

    // Start is called before the first frame update
    void Start()
    {
        _controllerTooltipManager = GameObject.Find("ControllerToolTipManager").GetComponent<ControllerTooltipManager>();

        // A duplicate collider is created.
        // If a custom collider is provided, that will be duplicated.
        // Otherwise, the paren't collider is duplicated and scaled by TriggerSizeFactor
        Collider parentCollider = transform.parent.GetComponent<Collider>();
        var _colliderType = CustomColliderTrigger != null ? CustomColliderTrigger.GetType() : parentCollider.GetType(); // get the collider type, either from the provided custom collider or the parent's collider

        if (_colliderType == typeof(MeshCollider))
        {
            Debug.LogError("Object with name '" + parentCollider.transform.name + "' has mesh collider! This will not work! Please attach a custom box, sphere, or capsule collider, perhaps directly onto this ControllerTooltipActivator!");
            return;
        }
        else 
        {
            var tooltipTriggerCollider = (Collider)gameObject.AddComponent(_colliderType); // create a duplicate attached to this activator object
            var properties = _colliderType.GetDeclaredProperties(); // get properties of the collider type
                        
            foreach (var property in properties)
            {
                if (CustomColliderTrigger == null)
                {
                    if (_colliderForDistanceComparison == null)
                        _colliderForDistanceComparison = parentCollider; // parent collider (presumably tight around object) will be used for distance evaluation. necessary because the trigger collider is too large

                    if (property.Name.Equals("center"))
                        property.SetValue(tooltipTriggerCollider, (Vector3)property.GetValue(parentCollider) / TriggerSizeFactor); // adjust center of collider to compensate for TriggerSizeFactor. casting is neccesary, but we know for sure that is is a collider, so it should be safe
                    else
                        property.SetValue(tooltipTriggerCollider, property.GetValue(parentCollider)); // otherwise duplicate properties from the collider we are copying
                }
                else
                {
                    if (_colliderForDistanceComparison == null)
                        _colliderForDistanceComparison = CustomColliderTrigger; // custom collider will be used for distance evaluation. is fine because the custom collider has no concept of "being larger than the object itself"

                    property.SetValue(tooltipTriggerCollider, property.GetValue(CustomColliderTrigger)); // duplicate properties from the collider we are copying
                }
            }

            ((Collider)tooltipTriggerCollider).isTrigger = true; // make trigger trigger. casting is neccesary, but we know for sure that is is a collider, so it should be safe
            transform.localScale *= CustomColliderTrigger == null ? TriggerSizeFactor : 1; // scale object (in practice the collider) by TriggerSizeFactor
        }


        // create a list of mappings between buttons and actions
        _buttonMappingsLeft = new List<ButtonActionMapping>
        {
            new(ControllerButtons.OculusLeft, OculusLeft),
            new(ControllerButtons.ThumbstickLeft, ThumbstickLeft),
            new(ControllerButtons.X, X),
            new(ControllerButtons.Y, Y),
            new(ControllerButtons.TriggerFrontLeft, TriggerFrontLeft),
            new(ControllerButtons.TriggerGripLeft, TriggerGripLeft),
        };

        _buttonMappingsRight = new List<ButtonActionMapping>
        {
            new(ControllerButtons.OculusRight, OculusRight),
            new(ControllerButtons.ThumbstickRight, ThumbstickRight),
            new(ControllerButtons.A, A),
            new(ControllerButtons.B, B),
            new(ControllerButtons.TriggerFrontRight, TriggerFrontRight),
            new(ControllerButtons.TriggerGripRight, TriggerGripRight)
        };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Grabber"))
        {
            // send the necessary information about this object to ControllerTooltipManager
            if (other.GetComponent<Grabber>().HandSide == ControllerHand.Left)
                _controllerTooltipManager.OnHandEntered(new InterractableObject(_colliderForDistanceComparison, _buttonMappingsLeft), ControllerHand.Left);
            else if (other.GetComponent<Grabber>().HandSide == ControllerHand.Right)
                _controllerTooltipManager.OnHandEntered(new InterractableObject(_colliderForDistanceComparison, _buttonMappingsRight), ControllerHand.Right);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Grabber"))
        {
            // send the necessary information about this object to ControllerTooltipManager
            if (other.GetComponent<Grabber>().HandSide == ControllerHand.Left)
                _controllerTooltipManager.OnHandExited(new InterractableObject(_colliderForDistanceComparison, _buttonMappingsLeft), ControllerHand.Left);
            else if (other.GetComponent<Grabber>().HandSide == ControllerHand.Right)
                _controllerTooltipManager.OnHandExited(new InterractableObject(_colliderForDistanceComparison, _buttonMappingsRight), ControllerHand.Right);
        }
    }
}
