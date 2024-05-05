using UnityEngine;

public class ConveyorController : MonoBehaviour
{    
    /// <summary>
    /// Enum <c>Direction</c> is used to select the movement direction of the conveyor belt.
    /// The colors used by the Unity Editor to indicate movement axis.
    /// D and I refers to whether it is an increase or decrease in value along the given axis.
    /// </summary>
    private enum DirectionState
    {
        Forward_BlueI,
        Backward_BlueD,
        Right_RedI,
        Left_RedD,
        Up_GreenI,
        Down_GreenD
    }

    // ----------------- Editor Variables -----------------
    [Header("Conveyor Belt Settings")]
    [Tooltip("Links the conveyor to the secondary task if true")]
    [SerializeField]
    private bool _useSecondaryTask;
    public bool UseSecondaryTask
    {
        get { return _useSecondaryTask; }
    }

    [Tooltip("Toggle the conveyor belt on or off.")]
    [SerializeField]
    private bool _isActive = true;
    public bool IsActive
    {
        get { return _isActive;}
    }

    [Header("Speed Settings")]
    [Tooltip("The acceleration of the object on the conveyor belt.")]
    [SerializeField]
    private float _acceleration = 30f;

    [Tooltip("The maximum speed the object can reach on the conveyor belt.")]
    [SerializeField]
    [Range(0, 50)]
    private float _speed = 1f;
    public float Speed
    {
        get { return _speed; }
    }

    [SerializeField]
    [Tooltip(
        "For selecting movement direction. Using the direction of the belt object. The colors used by the Unity Editor to indicate movement axis. D and I refers to whether it is an increase or decrease in value along the given axis."
    )]
    private DirectionState _direction = DirectionState.Forward_BlueI;

    // ----------------- Private Variables -----------------

    private Vector3 _directionVector;

    // ----------------- Unity Functions -----------------

    void Start()
    {
        switch (_direction) // Transforming enum to vector3
        {
            case DirectionState.Forward_BlueI:
            {
                _directionVector = gameObject.transform.forward;
                break;
            }
            case DirectionState.Right_RedI:
            {
                _directionVector = gameObject.transform.right;
                break;
            }
            case DirectionState.Backward_BlueD:
            {
                _directionVector = -gameObject.transform.forward;
                break;
            }
            case DirectionState.Left_RedD:
            {
                _directionVector = -gameObject.transform.right;
                break;
            }
            case DirectionState.Up_GreenI:
            {
                _directionVector = gameObject.transform.up;
                break;
            }
            case DirectionState.Down_GreenD:
            {
                _directionVector = -gameObject.transform.up;
                break;
            }
        }
    }

    void Update()
    {
        _isActive = UseSecondaryTask ? GameManager.Instance.IsSecondaryTaskOn : GameManager.Instance.IsTaskOn;
    }

    // ----------------- Private Functions -----------------

    /// <summary>
    /// Method <c>OnCollisionStay</c> applies force to objects that are on the conveyor belt.
    /// </summary>
    /// <param name="collision"> The collision object that is on the conveyor belt. </param>
    private void OnCollisionStay(Collision collision)
    {
        if (!_isActive)
            return;

        Rigidbody obj = collision.gameObject.GetComponent<Rigidbody>();
        if (obj)
        {
            if (obj.velocity.magnitude > _speed)
                return;

            obj.AddForce(_directionVector * _acceleration, ForceMode.Acceleration);
        }
    }
}
