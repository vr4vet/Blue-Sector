using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationConstraintsController : MonoBehaviour
{
    [HideInInspector] private Animator animator;
    [HideInInspector] private int isTalkingHash;
    [HideInInspector] private RigBuilder rigBuilder; // Use RigBuilder instead of Rig
    private GameObject targetRef;
    private MultiAimConstraint headCon;
    private MultiAimConstraint spineCon;
    private MultiAimConstraint rightArmCon;
    private MultiAimConstraint rightForeArmCon;
    private List<ChainIKConstraint> fingerConstraints = new List<ChainIKConstraint>();
    private PointingScript _pointingScript;
    private Transform _previousPointingObject;
    private DialogueBoxController _dialogueBoxController;
    

    /// <summary>
    /// Check that every compontent need exists
    /// Add contraints at run time, the NPC should look at the player (aka. CameraRig)
    /// </summary>
    void Start()
    {
        _pointingScript = GetComponentInParent<PointingScript>();
        animator = GetComponent<Animator>();
        rigBuilder = GetComponent<RigBuilder>();

        // increases performance
        isTalkingHash = Animator.StringToHash("isTalking");
        // Ensure isTalking starts as false
        animator.SetBool(isTalkingHash, false);

        // Find the MultiAimConstraint component within the "TargetTracking" Rig Layer
        if (rigBuilder != null && rigBuilder.layers.Count > 0)
        {
            // Check if a Rig Layer with the name "TargetTracking" exists
            RigLayer rigLayer = rigBuilder.layers.Find(layer => layer.name == "TargetTracking");
            if (rigLayer != null)
            {
                // Access the Rig component within the Rig Layer
                Rig rig = rigLayer.rig;

                if (rig != null)
                {               
                    GameObject playerRef = NPCToPlayerReferenceManager.Instance.PlayerTarget;                                      
                    targetRef = playerRef.transform.Find("TrackingSpace").transform.Find("CenterEyeAnchor").gameObject;
                    
                    
                    if (targetRef != null)
                    {
                        // Adds contraints at runtime
                        MultiAimConstraint[] constraints = rig.GetComponentsInChildren<MultiAimConstraint>();
                        if (constraints.Count() == 0) {
                            Debug.LogError("Could not find any multi aim constraints in the rig (AimObjectHead or AimObjectSpine)");
                        }
                        
                        // Get the chain IK constraints for the fingers
                        ChainIKConstraint[] chainIKConstraints = rig.GetComponentsInChildren<ChainIKConstraint>();
                        if (chainIKConstraints.Count() == 0) {
                            Debug.LogError("Could not find any chain IK constraints in the rig (Finger objects)");
                        }
                        
                        foreach (MultiAimConstraint con in constraints)
                        {
                            var sourceObject = con.data.sourceObjects;
                            
                            // Set the constraints for the right arm and right forearm
                            if (con.name == "AimObjectRightArm" || con.name == "AimObjectRightForeArm")
                            {
                                if (con.name == "AimObjectRightArm") {
                                    
                                    con.data.aimAxis = MultiAimConstraintData.Axis.Y;
                                    con.data.upAxis = MultiAimConstraintData.Axis.Y;
                                    con.data.constrainedXAxis = true;
                                    con.data.constrainedYAxis = false;
                                    con.data.constrainedZAxis = true;
                                    
                                    con.data.limits = new Vector2(-120f, 120f);
                                    con.weight = 0.0f;
                                    rightArmCon = con;
                                    
                                    
                                    
                                } else if (con.name == "AimObjectRightForeArm") {
                                    con.data.aimAxis = MultiAimConstraintData.Axis.Y;
                                    con.data.upAxis = MultiAimConstraintData.Axis.Y;
                                    con.data.constrainedXAxis = true;
                                    con.data.constrainedYAxis = false;
                                    con.data.constrainedZAxis = true;
                                    
                                    con.data.limits = new Vector2(120f, 120f);
                                    con.weight = 0.0f;
                                    rightForeArmCon = con;
                                    
                                }
                            } 
                            else
                            {
                                // Manage settings for the constrained object(s)
                                con.data.aimAxis = MultiAimConstraintData.Axis.Z;
                                con.data.upAxis = MultiAimConstraintData.Axis.Z;
                                con.data.constrainedXAxis = true;
                                con.data.constrainedYAxis = true;
                                con.data.constrainedZAxis = true;
                                // Set the player camera as the source object (what the NPC will look at)
                                var newSource = new WeightedTransform(targetRef.transform, 1.0f);
                                sourceObject.Add(newSource);
                                con.data.sourceObjects = sourceObject;  
                                
                                
                                if (con.gameObject.name == "AimObjectHead") {
                                    //con.data.constrainedXAxis = true;
                                    // The max ranges for how far to the side the NPC will look
                                    con.data.limits = new Vector2(-70f, 70f);
                                    headCon = con;
                                } else if (con.gameObject.name == "AimObjectSpine") {
                                    // Spine should move less than head and not bend backwards (x-axis)
                                    con.data.limits = new Vector2(-40f, 40f);
                                    spineCon = con;
                                }
                            }
                            
                        }

                        // Set the constraints for the fingers
                        foreach (ChainIKConstraint con in chainIKConstraints)
                        {
                            GameObject fingerRetract = null;
                            // The constraints will use an empty child of the hand which they will target to be able to retract the fingers. Each finger will have its own empty object to target, so they don't interfere with each other.
                            if (con.data.root.parent.childCount < 9)
                            {
                                if (con.data.root.name.Contains("Ring1"))
                                {
                                    // Create an empty object for the ring finger to target
                                    fingerRetract = new GameObject("RingFingerRetract");
                                    // Set the hand as the parent so the location is relative to the hand
                                    fingerRetract.transform.SetParent(con.data.root.parent, false);
                                    // Set the position of the empty object to match the position of the finger
                                    fingerRetract.transform.localPosition = new Vector3((float)0.0183000006,(float)-0.00300000003,(float)-0.000600000028);
                                }
                                
                                else if (con.data.root.name.Contains("Pinky1"))
                                {
                                    fingerRetract = new GameObject("PinkyFingerRetract");
                                    fingerRetract.transform.SetParent(con.data.root.parent, false);
                                    fingerRetract.transform.localPosition = new Vector3((float)0.0379999988, (float)9.99999975e-05, (float)-0.00079999998);

                                }
                                
                                // The other fingers use the same target as they are either not retracted or can use the target without interfering with others
                                else
                                {
                                    fingerRetract = GameObject.Find("GenericFingerRetract");
                                    if (fingerRetract == null)
                                    {
                                        fingerRetract = new GameObject("GenericFingerRetract");
                                        fingerRetract.transform.SetParent(con.data.root.parent, false);
                                    }
                                }
                                
                            }
                            
                            con.data.target = fingerRetract.transform;
                            fingerConstraints.Add(con);
                        }
                        rigBuilder.Build();
                    }
                    else
                    {
                        Debug.LogError("Cannot find XR Rig Advanced/PlayerController/CameraRig/TrackingSpace/CenterEyeAnchor in the scene");
                    }
                    
                }
                else
                {
                    Debug.LogError("Rig component not found in the 'TargetTracking' Rig Layer.");
                }
            }
            else
            {
                Debug.LogError("Rig Layer 'TargetTracking' not found.");
            }
        }
        else
        {
            Debug.LogError("RigBuilder component not found.");
        }

    }

    /// <summary>
    /// The NPC should look at the player while talking
    /// </summary>
    void Update()
    {
        if (animator != null) {
            
            // Uses the isTalking parameter of the animator to determine if the NPC should look at the player. This was originally used for the old pointing animation but works fine for this purpose too.
            bool isPointing = animator.GetBool("isPointing");

            if (isPointing)
            {
                // Check if the object to point at has changed as constantly updating the constraints takes a heavy toll on performance
                Transform currentPointingObject = _pointingScript.GetObjectToPointAt().transform;

                if (_previousPointingObject != currentPointingObject)
                {
                    ChangeTarget(rightArmCon, currentPointingObject);
                    ChangeTarget(headCon, currentPointingObject);
                    ChangeTarget(spineCon, currentPointingObject);
                    // Update the previous pointing object to the current one
                    _previousPointingObject = currentPointingObject;
                    
                }
                
                // Enable the right arm constraint
                if (rightArmCon.weight < 1.0f) rightArmCon.weight += 0.01f;
                if (headCon.weight < 0.4f) headCon.weight += 0.001f;
                if (spineCon.weight < 0.3f) spineCon.weight += 0.001f;
                
                // Enable the right forearm constraint at specific weights for each finger
                foreach (ChainIKConstraint finger in fingerConstraints)
                {
                    if (!finger.name.Contains("Index"))
                    {
                        if (finger.name.Contains("Middle"))
                        {
                            finger.data.chainRotationWeight = 0.693f;
                        }
                        else if (finger.name.Contains("Ring"))
                        {
                            finger.data.chainRotationWeight = 0.72f;
                        }
                        else if (finger.name.Contains("Pinky"))
                        {
                            finger.data.chainRotationWeight = 0.633f;
                        }
                        else if (finger.name.Contains("Thumb"))
                        {
                            finger.data.chainRotationWeight = 0.299f;
                        }
                    }
                    
                }

            }
            else
            {
                // disables the constaints when the NPC is not pointing
                if (rightArmCon.weight > 0.0f) rightArmCon.weight -= 0.01f;
                
                foreach (ChainIKConstraint finger in fingerConstraints)
                {
                    if (!finger.name.Contains("Index"))
                    {
                        finger.data.chainRotationWeight = 0.0f;
                    }
                }
            }
            
            bool isTalking = animator.GetBool(isTalkingHash);
            // Add the code to control the multi-aim constraint here
            if (isTalking)
            {
                if (headCon.data.sourceObjects[0].transform.name != targetRef.transform.name)
                {
                    ChangeTarget(headCon, targetRef.transform);
                    ChangeTarget(spineCon, targetRef.transform);
                }
                // Get the direction between player and NPC
                Vector3 playerDirection = targetRef.transform.position - transform.position;
                playerDirection.Normalize();
                // Get forward direction of NPC (this)
                Vector3 forward = transform.forward;
                // Angle between player and NPC
                float angle = Vector3.Angle(forward, playerDirection);
                if (angle <= 90f)  {
                    // Enable the multi-aim constraint when character is talking and player not behind NPC
                    // Add up to different thresholds for spine and head so spine moves less than head
                    if (headCon.weight < 0.7f) { headCon.weight += 0.004f; }
                    if (spineCon.weight < 0.3f) { spineCon.weight += 0.004f; }
                } else {
                    // If behind NPC, stop looking at player
                    headCon.weight -= 0.002f;
                    spineCon.weight -= 0.002f;
                }
            }
            else
            {
                if (!isPointing)
                {
                    // Disable the multi-aim constraint when character is not talking
                    headCon.weight -= 0.002f;
                    spineCon.weight -= 0.002f; 
                }
            }
        } else {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void ChangeTarget(MultiAimConstraint con, Transform target)
    {
        if (con.data.sourceObjects.Count > 0)
        {
            var resetSourceObjects = con.data.sourceObjects;
            resetSourceObjects.Clear();
            con.data.sourceObjects = resetSourceObjects;
        }
        
        var sourceObjects = con.data.sourceObjects;
        var newSource = new WeightedTransform(target, 1.0f);
        sourceObjects.Add(newSource);
        con.data.sourceObjects = sourceObjects;

        rigBuilder.Build();
    }

}