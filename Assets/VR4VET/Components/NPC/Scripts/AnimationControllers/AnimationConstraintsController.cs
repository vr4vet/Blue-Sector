using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimationConstraintsController : MonoBehaviour
{
    [HideInInspector] private Animator animator;
    [HideInInspector] private int isTalkingHash;
    [HideInInspector] private RigBuilder rigBuilder; // Use RigBuilder instead of Rig
    private GameObject targetRef;
    private GameObject pointRef;
    private MultiAimConstraint headCon;
    private MultiAimConstraint spineCon;
    private MultiAimConstraint rightArmCon;
    private MultiAimConstraint rightForeArmCon;
    

    /// <summary>
    /// Check that every compontent need exists
    /// Add contraints at run time, the NPC should look at the player (aka. CameraRig)
    /// </summary>
    void Start()
    {
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
                    pointRef = GameObject.Find("cardboard_box_3 (2)");
                    
                    
                    if (targetRef != null)
                    {
                        // Adds contraints at runtime
                        MultiAimConstraint[] constraints = rig.GetComponentsInChildren<MultiAimConstraint>();
                        if (constraints.Count() == 0) {
                            Debug.LogError("Could not find any multi aim constraints in the rig (AimObjectHead or AimObjectSpine)");
                        }
                        
                        ChainIKConstraint[] chainIKConstraints = rig.GetComponentsInChildren<ChainIKConstraint>();
                        if (chainIKConstraints.Count() == 0) {
                            Debug.LogError("Could not find any chain IK constraints in the rig (Finger objects)");
                        }
                        
                        foreach (MultiAimConstraint con in constraints)
                        {
                            var sourceObject = con.data.sourceObjects;
                            if (con.name == "AimObjectRightArm" || con.name == "AimObjectRightForeArm")
                            {
                                
                                
                                var newSource = new WeightedTransform(pointRef.transform, 1.0f);
                                sourceObject.Add(newSource);
                                con.data.sourceObjects = sourceObject;
                                
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
                                    // Manage settings for the constrained object(s)
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

                        foreach (ChainIKConstraint con in chainIKConstraints)
                        {
                            GameObject fingerRetract = null;
                            if (con.data.root.parent.childCount < 9)
                            {
                                if (con.data.root.name.Contains("Ring1"))
                                {
                                    fingerRetract = new GameObject("RingFingerRetract");
                                    fingerRetract.transform.SetParent(con.data.root.parent, false);
                                    fingerRetract.transform.localPosition = new Vector3((float)0.0183000006,(float)-0.00300000003,(float)-0.000600000028);
                                }
                                
                                else if (con.data.root.name.Contains("Pinky1"))
                                {
                                    fingerRetract = new GameObject("PinkyFingerRetract");
                                    fingerRetract.transform.SetParent(con.data.root.parent, false);
                                    fingerRetract.transform.localPosition = new Vector3((float)0.0379999988, (float)9.99999975e-05, (float)-0.00079999998);

                                }

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
            
            Vector3 direction = pointRef.transform.position - transform.position;
            direction.Normalize();
            Vector3 forward1 = transform.forward;

            //rightArmCon.weight = 0.5f;
            
            bool isTalking = animator.GetBool(isTalkingHash);
            // Add the code to control the multi-aim constraint here
            if (isTalking)
            {
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
                // Disable the multi-aim constraint when character is not talking
                headCon.weight -= 0.002f;
                spineCon.weight -= 0.002f;
            }
        } else {
            animator = GetComponentInChildren<Animator>();
        }
    }
    
    
    void OnDrawGizmos()
    {
        if (pointRef != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointRef.transform.position, 0.1f);
            Gizmos.DrawLine(transform.position, pointRef.transform.position);
        }

        if (targetRef != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(targetRef.transform.position, 0.1f);
            Gizmos.DrawLine(transform.position, targetRef.transform.position);
        }
    }
}
