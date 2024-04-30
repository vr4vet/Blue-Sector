using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    // The colors used by the Unity Editor to indicate movement axis. D and I refers to whether it is an increase or decrease in value along the given axis.
    private enum Direction
    {
        Forward_BlueI,
        Backward_BlueD,
        Right_RedI,
        Left_RedD,
        Up_GreenI,
        Down_GreenD
    }

    // ----------------- Editor Variables -----------------
    [SerializeField]
    private bool useSecondaryTask;
    public bool UseSecondaryTask
    {
        get { return useSecondaryTask; }
    }

    [SerializeField]
    private bool isBeltOn = true;
    public bool IsBeltOn
    {
        get { return isBeltOn; }
    }

    [SerializeField]
    private float acceleration = 30f;

    [SerializeField]
    [Range(0, 50)]
    private float maxSpeed = 1f;
    public float MaxSpeed
    {
        get { return maxSpeed; }
    }

    [SerializeField]
    [Tooltip(
        "For selecting movement direction. Using the direction of the belt object. The colors used by the Unity Editor to indicate movement axis. D and I refers to whether it is an increase or decrease in value along the given axis."
    )]
    private Direction direction = Direction.Forward_BlueI;

    // ----------------- Private Variables -----------------

    private Vector3 _direction;

    // ----------------- Unity Functions -----------------

    void Start()
    {
        switch (direction) // Transforming enum to vector3
        {
            case Direction.Forward_BlueI:
            {
                _direction = gameObject.transform.forward;
                break;
            }
            case Direction.Right_RedI:
            {
                _direction = gameObject.transform.right;
                break;
            }
            case Direction.Backward_BlueD:
            {
                _direction = -gameObject.transform.forward;
                break;
            }
            case Direction.Left_RedD:
            {
                _direction = -gameObject.transform.right;
                break;
            }
            case Direction.Up_GreenI:
            {
                _direction = gameObject.transform.up;
                break;
            }
            case Direction.Down_GreenD:
            {
                _direction = -gameObject.transform.up;
                break;
            }
        }
    }

    void Update()
    {
        if (useSecondaryTask)
        {
            isBeltOn = GameManager.Instance.IsSecondaryTaskOn;
            return;
        }
        isBeltOn = GameManager.Instance.IsTaskOn;
    }

    // ----------------- Private Functions -----------------

    /// <summary>
    /// Method <c>OnCollisionStay</c> applies force to objects that are on the conveyor belt.
    /// </summary>
    /// <param name="collision"> The collision object that is on the conveyor belt. </param>
    private void OnCollisionStay(Collision collision)
    {
        if (!IsBeltOn)
            return;

        Rigidbody obj = collision.gameObject.GetComponent<Rigidbody>();
        if (obj)
        {
            if (obj.velocity.magnitude > maxSpeed)
                return;

            obj.AddForce(_direction * acceleration, ForceMode.Acceleration);
        }
    }
}
