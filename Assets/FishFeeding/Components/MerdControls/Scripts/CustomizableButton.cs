using System.Collections.Generic;
using BNG;
using UnityEngine;
using UnityEngine.Events;

public class CustomizableButton : MonoBehaviour
{
    // These have been hard coded for hand speed
    private const float ButtonSpeed = 15f;

    private const float SpringForce = 1500f;

    [Tooltip("The Local Y position of the button when it is pushed all the way down. Local Y position will never be less than this.")]
    public float MinLocalY = 0.25f;

    [Tooltip("The Local Y position of the button when it is not being pushed. Local Y position will never be greater than this.")]
    public float MaxLocalY = 0.55f;

    [Tooltip("How far away from MinLocalY / MaxLocalY to be considered a click")]
    public float ClickTolerance = 0.01f;

    [Tooltip("If true the button can be pressed by physical object by utiizing a Spring Joint. Set to false if you don't need / want physics interactions, or are using this on a moving platform.")]
    public bool AllowPhysicsForces = true;

    public AudioClip ButtonClick;
    public AudioClip ButtonClickUp;

    public UnityEvent onButtonDown;
    public UnityEvent onButtonUp;

    private readonly List<Grabber> grabbers = new List<Grabber>(); // Grabbers in our trigger
    private readonly List<UITrigger> uiTriggers = new List<UITrigger>(); // UITriggers in our trigger
    private SpringJoint joint;
    private AudioSource audioSource;
    private Rigidbody rigid;
    private Vector3 buttonDownPosition;
    private Vector3 buttonUpPosition;

    public virtual bool IsButtonDown { get; private set; }

    public virtual Vector3 GetButtonUpPosition()
    {
        return new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z);
    }

    public virtual Vector3 GetButtonDownPosition()
    {
        return new Vector3(transform.localPosition.x, MinLocalY, transform.localPosition.z);
    }

    // Callback for ButtonDown
    protected virtual void OnButtonDown()
    {
        IsButtonDown = true;

        // Play sound
        if (audioSource && ButtonClick)
        {
            audioSource.clip = ButtonClick;
            audioSource.Play();
        }

        // Call event
        if (onButtonDown != null)
        {
            onButtonDown.Invoke();
        }
    }

    // Callback for ButtonDown
    protected virtual void OnButtonUp()
    {
        IsButtonDown = false;

        // Play sound
        if (audioSource && ButtonClickUp)
        {
            audioSource.clip = ButtonClickUp;
            audioSource.Play();
        }

        // Call event
        if (onButtonUp != null)
        {
            onButtonUp.Invoke();
        }
    }

    protected virtual bool CanReleaseButton() => !IsGrabberInButton() || uiTriggers.Count == 0;

    protected virtual void Start()
    {
        joint = GetComponent<SpringJoint>();
        rigid = GetComponent<Rigidbody>();

        // Set to kinematic so we are not affected by outside forces
        if (!AllowPhysicsForces)
        {
            rigid.isKinematic = true;
        }

        // Start with button up top / popped up
        transform.localPosition = new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z);

        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        buttonDownPosition = GetButtonDownPosition();
        buttonUpPosition = GetButtonUpPosition();

        // push button down
        float force = 0;
        if (!CanReleaseButton())
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, buttonDownPosition, ButtonSpeed * Time.deltaTime);
        }
        else
        {
            // Let the spring push the button up if physics forces are enabled
            if (AllowPhysicsForces)
            {
                force = SpringForce;
            }
            // Need to lerp back into position if spring won't do it for us
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, buttonUpPosition, ButtonSpeed * Time.deltaTime);
            }
        }

        if (joint)
        {
            joint.spring = force;
        }

        // Cap values
        if (transform.localPosition.y < MinLocalY)
        {
            transform.localPosition = buttonDownPosition;
        }
        else if (transform.localPosition.y > MaxLocalY + .1f)
        {
            transform.localPosition = buttonUpPosition;
        }

        // Click Down?
        float buttonDownDistance = transform.localPosition.y - buttonDownPosition.y;
        if (buttonDownDistance <= ClickTolerance && !IsButtonDown)
        {
            OnButtonDown();
        }
        // Click Up?
        float buttonUpDistance = buttonUpPosition.y - transform.localPosition.y;
        if (buttonUpDistance <= ClickTolerance && IsButtonDown)
        {
            OnButtonUp();
        }
    }

    private bool IsGrabberInButton()
    {
        // Find a valid grabber to push down
        for (int i = 0; i < grabbers.Count; i++)
        {
            if (!grabbers[i].HoldingItem)
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check Grabber
        Grabber grab = other.GetComponent<Grabber>();
        if (grab != null)
        {
            if (!grabbers.Contains(grab))
            {
                grabbers.Add(grab);
            }
        }

        // Check UITrigger, which is another type of activator
        UITrigger trigger = other.GetComponent<UITrigger>();
        if (trigger != null)
        {
            if (!uiTriggers.Contains(trigger))
            {
                uiTriggers.Add(trigger);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Grabber grab = other.GetComponent<Grabber>();
        if (grab != null)
        {
            if (grabbers.Contains(grab))
            {
                grabbers.Remove(grab);
            }
        }

        UITrigger trigger = other.GetComponent<UITrigger>();
        if (trigger != null)
        {
            if (uiTriggers.Contains(trigger))
            {
                uiTriggers.Remove(trigger);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Show Grip Point
        Gizmos.color = Color.blue;

        Vector3 upPosition = transform.TransformPoint(new Vector3(transform.localPosition.x, MaxLocalY, transform.localPosition.z));
        Vector3 downPosition = transform.TransformPoint(new Vector3(transform.localPosition.x, MinLocalY, transform.localPosition.z));

        Vector3 size = new Vector3(0.005f, 0.005f, 0.005f);

        Gizmos.DrawCube(upPosition, size);

        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(downPosition, size);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(upPosition, downPosition);
    }
}