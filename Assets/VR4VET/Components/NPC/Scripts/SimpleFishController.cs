using UnityEngine;
using System.Collections;

/// <summary>
/// Simple character model controller for non-humanoid NPCs like fish
/// Replaces SetCharacterModel for characters that don't need complex humanoid rigging
/// </summary>
public class SimpleFishController : MonoBehaviour
{
    [Header("Model Settings")]
    [SerializeField] private GameObject fishModelPrefab;
    [Tooltip("If true, uses the model already in the hierarchy instead of spawning from prefab")]
    [SerializeField] private bool useExistingModel = true;
    [SerializeField] private string modelName = "FishModel";
    
    [Header("Animation Settings")]
    [SerializeField] private float swimSpeed = 0.5f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float bobAmplitude = 0.1f;
    [SerializeField] private float bobFrequency = 1f;
    [Tooltip("How much to scale movement when 'talking'")]
    [SerializeField] private float talkingAnimationMultiplier = 1.5f;
    
    [Header("Look At Settings")]
    [SerializeField] private bool lookAtPlayer = true;
    [SerializeField] private float lookSpeed = 5f;
    [SerializeField] private Transform headBone;
    
    [Header("Flying Fish Settings")]
    [Tooltip("If true, fish will actively follow the player")]
    [SerializeField] private bool followPlayer = true;
    [Tooltip("Preferred distance from player")]
    [SerializeField] private float preferredPlayerDistance = 1.5f;
    [Tooltip("How quickly the fish moves toward target position")]
    [SerializeField] private float flySpeed = 2.0f;
    [Tooltip("Set to true for flying fish that ignores terrain")]
    [SerializeField] private bool canFly = true;
    [Tooltip("How high or low relative to player eye level")]
    [SerializeField] private float heightOffset = -0.3f;
    [Tooltip("How often to update target position (seconds)")]
    [SerializeField] private float positionUpdateInterval = 1.0f;
    [Tooltip("Visual effects when fish is moving rapidly")]
    [SerializeField] private GameObject movementEffectPrefab;
    
    private GameObject fishModel;
    private Vector3 startPosition;
    private float timeOffset;
    private bool isTalking = false;
    private float defaultBobAmplitude;
    private float defaultRotationSpeed;
    private Transform playerTransform;
    private float intensityMultiplier = 1.0f;
    private Vector3 currentTargetPosition;
    private float timeSincePositionUpdate = 0f;
    private ParticleSystem movementEffect;
    private bool isMovingToPlayer = false;

    /// <summary>
    /// Public access to check if this fish can fly
    /// </summary>
    public bool CanFly
    {
        get { return canFly; }
    }
    
    private void Awake()
    {
        // Find the player camera
        if (Camera.main != null)
        {
            playerTransform = Camera.main.transform;
        }

        // Set default values
        defaultBobAmplitude = bobAmplitude;
        defaultRotationSpeed = rotationSpeed;
        
        // Random time offset for varied animation
        timeOffset = Random.Range(0f, 100f);
        
        // Setup model
        SetupFishModel();
        
        // Setup movement effects
        if (movementEffectPrefab != null)
        {
            GameObject effectObj = Instantiate(movementEffectPrefab, transform);
            effectObj.transform.localPosition = Vector3.zero;
            movementEffect = effectObj.GetComponent<ParticleSystem>();
            if (movementEffect != null)
            {
                // Start with effect disabled
                movementEffect.Stop();
            }
        }
    }

    private void SetupFishModel()
    {
        // Use existing model if specified
        if (useExistingModel)
        {
            // Try to find a child GameObject with the model name
            Transform childModel = transform.Find(modelName);
            
            if (childModel != null)
            {
                fishModel = childModel.gameObject;
                Debug.Log("Using existing fish model: " + fishModel.name);
            }
            else
            {
                // Search for name in children (instead of tag which might not be defined)
                foreach (Transform child in transform)
                {
                    if (child.name.Contains(modelName) || child.name.Contains("Model") || child.name.Contains("Fish"))
                    {
                        fishModel = child.gameObject;
                        Debug.Log("Found fish model by name: " + fishModel.name);
                        break;
                    }
                }

                // If still null, use the first child that might be a model
                if (fishModel == null && transform.childCount > 0)
                {
                    // Look for children that might be models (have MeshRenderer or SkinnedMeshRenderer)
                    foreach (Transform child in transform)
                    {
                        if (child.GetComponentInChildren<MeshRenderer>() != null || 
                            child.GetComponentInChildren<SkinnedMeshRenderer>() != null)
                        {
                            fishModel = child.gameObject;
                            Debug.Log("Using child with mesh as fish model: " + fishModel.name);
                            break;
                        }
                    }
                    
                    // If still not found, use first child
                    if (fishModel == null && transform.childCount > 0)
                    {
                        fishModel = transform.GetChild(0).gameObject;
                        Debug.Log("Using first child as fish model: " + fishModel.name);
                    }
                }
            }
        }
        // Create from prefab
        else if (fishModelPrefab != null)
        {
            fishModel = Instantiate(fishModelPrefab, transform);
            fishModel.transform.localPosition = Vector3.zero;
            fishModel.transform.localRotation = Quaternion.identity;
            Debug.Log("Instantiated fish model from prefab");
        }
        
        if (fishModel == null)
        {
            // If we still don't have a model, create a simple primitive to represent the fish
            fishModel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fishModel.transform.SetParent(transform, false);
            fishModel.transform.localPosition = Vector3.zero;
            fishModel.transform.localScale = new Vector3(0.3f, 0.2f, 0.5f); // Fish-like shape
            fishModel.name = "DefaultFishModel";
            Debug.LogWarning("Created default fish model because no model was found or assigned");
        }
        
        // Try to find head bone if not manually set
        if (headBone == null)
        {
            // Look for common names used for head bones
            string[] possibleHeadNames = { "head", "Head", "fish_head", "Face", "face" };
            
            foreach (string headName in possibleHeadNames)
            {
                Transform foundHead = fishModel.transform.Find(headName);
                if (foundHead != null)
                {
                    headBone = foundHead;
                    Debug.Log("Found head bone: " + headBone.name);
                    break;
                }
            }
            
            // If still not found, use the model transform itself
            if (headBone == null)
            {
                headBone = fishModel.transform;
            }
        }
        
        // Record starting position for bob effect
        startPosition = transform.localPosition;
    }
    
    private void Update()
    {
        if (fishModel == null) return;
        
        // Update player reference if lost
        if (playerTransform == null && Camera.main != null)
        {
            playerTransform = Camera.main.transform;
        }
        
        // Handle fish animation (bobbing and head rotation)
        AnimateFish();
        
        // Only follow player if explicitly enabled and not talking
        // For Chatbot fish, we typically don't want it to move when not talking
        if (followPlayer && playerTransform != null && !isTalking)
        {
            // If we're very far from player (e.g., player teleported), teleport to catch up
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer > preferredPlayerDistance * 3f)
            {
                TeleportToPlayer();
            }
            // Otherwise only update position occasionally
            else if (timeSincePositionUpdate >= positionUpdateInterval)
            {
                timeSincePositionUpdate = 0f;
                UpdateTargetPositionNearPlayer();
                FlyTowardTargetPosition();
            }
            else
            {
                timeSincePositionUpdate += Time.deltaTime;
            }
        }
        else
        {
            // When talking, always face the player but don't change position
            LookAtPlayerHorizontal();
        }
    }
    
    /// <summary>
    /// Handles fish model animation (bobbing, rotation, etc.)
    /// </summary>
    private void AnimateFish()
    {
        // Simple bobbing motion for fish-like movement
        float bobY = Mathf.Sin((Time.time + timeOffset) * bobFrequency) * bobAmplitude * intensityMultiplier;
        fishModel.transform.localPosition = new Vector3(0, bobY, 0);
        
        // Gentle rotation for swimming appearance
        fishModel.transform.Rotate(0, Time.deltaTime * rotationSpeed * intensityMultiplier, 0, Space.Self);
        
        // Look at player if enabled and player exists
        if (lookAtPlayer && playerTransform != null && headBone != null)
        {
            // Smoothly rotate head to look at player
            Vector3 directionToPlayer = playerTransform.position - headBone.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            headBone.rotation = Quaternion.Slerp(headBone.rotation, targetRotation, Time.deltaTime * lookSpeed);
        }
    }
    
    /// <summary>
    /// Calculates a good position near the player for the fish to swim to
    /// </summary>
    private void UpdateTargetPositionNearPlayer()
    {
        if (playerTransform == null) return;
        
        Vector3 playerPosition = playerTransform.position;
        Vector3 playerForward = playerTransform.forward;
        playerForward.y = 0; // Keep it horizontal
        playerForward.Normalize();
        
        // Calculate position directly in front of player with fixed distance
        // No randomness to prevent the fish from wandering too much
        float distance = preferredPlayerDistance;
        
        // If we're far away, move closer more quickly
        float currentDistance = Vector3.Distance(transform.position, playerPosition);
        if (currentDistance > preferredPlayerDistance * 2f)
        {
            distance = preferredPlayerDistance * 0.75f; // Move closer than usual
            isMovingToPlayer = true;
            
            // Activate movement effect if available
            if (movementEffect != null && !movementEffect.isPlaying)
            {
                movementEffect.Play();
            }
        }
        else if (isMovingToPlayer && currentDistance <= preferredPlayerDistance * 1.2f)
        {
            // We've reached the player
            isMovingToPlayer = false;
            
            // Deactivate movement effect
            if (movementEffect != null && movementEffect.isPlaying)
            {
                movementEffect.Stop();
            }
        }
        
        // Calculate position directly in front of player - very limited randomness
        // This keeps the fish more consistently in front of the player
        float smallRandomAngle = Random.Range(-15f, 15f); // Much smaller random angle
        Vector3 randomizedDirection = Quaternion.Euler(0, smallRandomAngle, 0) * playerForward;
        
        // Position fish in front of player
        Vector3 targetPos = playerPosition + randomizedDirection * distance;
        
        // Add slight side offset so fish isn't directly blocking view (smaller offset)
        targetPos += playerTransform.right * Random.Range(-0.3f, 0.3f);
        
        // Set proper height
        float eyeHeight = playerPosition.y;
        targetPos.y = eyeHeight + heightOffset; // No random height variation to keep it stable
        
        // If flying is enabled and the position is inside terrain, adjust height
        if (canFly && Physics.CheckSphere(targetPos, 0.2f, LayerMask.GetMask("Terrain", "Default")))
        {
            // Try different heights
            for (float heightCheck = 0.3f; heightCheck <= 2.0f; heightCheck += 0.3f)
            {
                Vector3 elevatedPos = targetPos;
                elevatedPos.y = eyeHeight + heightCheck;
                
                if (!Physics.CheckSphere(elevatedPos, 0.2f, LayerMask.GetMask("Terrain", "Default")))
                {
                    targetPos = elevatedPos;
                    break;
                }
            }
        }
        
        currentTargetPosition = targetPos;
    }
    
    /// <summary>
    /// Moves the fish toward the current target position
    /// </summary>
    private void FlyTowardTargetPosition()
    {
        if (currentTargetPosition == Vector3.zero || playerTransform == null) return;
        
        // Calculate distance to target
        float distanceToTarget = Vector3.Distance(transform.position, currentTargetPosition);
        
        if (distanceToTarget > 0.1f)
        {
            // Calculate movement speed based on distance
            float speed = flySpeed;
            
            // Move faster if far away
            if (distanceToTarget > preferredPlayerDistance * 2f)
            {
                speed = flySpeed * 3f;
            }
            
            // Move toward target position
            transform.position = Vector3.Lerp(transform.position, currentTargetPosition, speed * Time.deltaTime);
            
            // Always face player rather than movement direction
            LookAtPlayerHorizontal();
        }
        else
        {
            // Even when we're at the target position, keep facing the player
            LookAtPlayerHorizontal();
        }
    }
    
    private void LateUpdate()
    {
        // If talking, at rest, or no current target, always face player
        if (isTalking || playerTransform == null)
        {
            LookAtPlayerHorizontal();
        }
    }
    
    /// <summary>
    /// Makes the fish face the player on the horizontal plane (no tilt)
    /// </summary>
    private void LookAtPlayerHorizontal()
    {
        if (playerTransform == null) return;
        
        // Calculate direction to player on horizontal plane
        Vector3 targetPosition = playerTransform.position;
        Vector3 directionToPlayer = targetPosition - transform.position;
        directionToPlayer.y = 0; // Keep fish level horizontally
        
        if (directionToPlayer != Vector3.zero)
        {
            // Create rotation that looks at player without tilting
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            
            // Apply rotation directly for talking fish to ensure it always faces player
            if (isTalking) 
            {
                transform.rotation = targetRotation;
            }
            // Otherwise smooth rotation for natural movement
            else
            {
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, 
                    targetRotation, 
                    lookSpeed * Time.deltaTime);
            }
        }
    }
    
    /// <summary>
    /// Teleports the fish immediately to a good position near the player
    /// Call this when you need to reposition the fish instantly
    /// </summary>
    public void TeleportToPlayer()
    {
        if (playerTransform == null) return;
        
        Debug.Log("Teleporting fish to player");
        UpdateTargetPositionNearPlayer();
        transform.position = currentTargetPosition;
        
        // Face the player
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));
        
        // Trigger a brief effect to show teleportation
        StartCoroutine(TeleportEffect());
    }
    
    /// <summary>
    /// Visual effect when teleporting
    /// </summary>
    private IEnumerator TeleportEffect()
    {
        // Increase intensity briefly for a "pop-in" effect
        float originalIntensity = intensityMultiplier;
        SetIntensity(3.0f);
        
        // Play movement effect if available
        if (movementEffect != null)
        {
            movementEffect.Play();
        }
        
        yield return new WaitForSeconds(0.5f);
        
        // Return to normal intensity
        SetIntensity(originalIntensity);
        
        // Stop effect
        if (movementEffect != null)
        {
            movementEffect.Stop();
        }
    }
    
    /// <summary>
    /// Called by DialogueBoxController or other scripts when the NPC starts or stops talking
    /// </summary>
    public void OnTalkingStateChanged(bool talking)
    {
        SetTalking(talking);
    }
    
    /// <summary>
    /// Sets the fish to "talking" mode with more animated movements
    /// </summary>
    /// <param name="talking">Whether the fish is currently talking</param>
    public void SetTalking(bool talking)
    {
        isTalking = talking;
        
        if (talking)
        {
            // Increase bobbing and rotation when "talking"
            bobAmplitude = defaultBobAmplitude * talkingAnimationMultiplier;
            rotationSpeed = defaultRotationSpeed * talkingAnimationMultiplier;
            
            // Make sure fish is near player when talking
            if (followPlayer && playerTransform != null && 
                Vector3.Distance(transform.position, playerTransform.position) > preferredPlayerDistance * 1.5f)
            {
                TeleportToPlayer();
            }
        }
        else
        {
            // Return to normal movement
            bobAmplitude = defaultBobAmplitude;
            rotationSpeed = defaultRotationSpeed;
        }
    }
    
    /// <summary>
    /// Sets the overall intensity of the fish animation
    /// </summary>
    /// <param name="intensity">The multiplier for animation intensity (1.0 = normal)</param>
    public void SetIntensity(float intensity)
    {
        intensityMultiplier = Mathf.Max(0.1f, intensity); // Ensure it's not too low
        
        // Apply immediately to the animation parameters
        if (isTalking)
        {
            // If already talking, adjust from the talking baseline
            bobAmplitude = defaultBobAmplitude * talkingAnimationMultiplier * intensityMultiplier;
            rotationSpeed = defaultRotationSpeed * talkingAnimationMultiplier * intensityMultiplier;
        }
        else
        {
            // If not talking, adjust from the default values
            bobAmplitude = defaultBobAmplitude * intensityMultiplier;
            rotationSpeed = defaultRotationSpeed * intensityMultiplier;
        }
    }
    
    /// <summary>
    /// Swim toward a target position
    /// </summary>
    /// <param name="targetPosition">The position to swim toward</param>
    /// <param name="duration">How long to take to reach the position</param>
    public void SwimTo(Vector3 targetPosition, float duration)
    {
        StartCoroutine(SwimToCoroutine(targetPosition, duration));
    }
    
    /// <summary>
    /// Coroutine to handle swimming movement
    /// Made public so it can be used by the FishAnimatorController
    /// </summary>
    public IEnumerator SwimToCoroutine(Vector3 targetPosition, float duration)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0;
        
        // Look toward the target position
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
        
        while (elapsed < duration)
        {
            // Move toward target
            transform.position = Vector3.Lerp(startPos, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Ensure we're exactly at target position
        transform.position = targetPosition;
    }
    
    /// <summary>
    /// Swims to the player's position
    /// </summary>
    public void SwimToPlayer()
    {
        if (playerTransform != null)
        {
            UpdateTargetPositionNearPlayer();
            SwimTo(currentTargetPosition, 1.0f);
        }
    }
    
    /// <summary>
    /// Sets whether the fish is allowed to fly/swim through the air
    /// </summary>
    public void SetFlyingEnabled(bool enabled)
    {
        canFly = enabled;
    }
    
    /// <summary>
    /// Sets whether the fish should follow the player 
    /// </summary>
    public void SetFollowPlayerEnabled(bool enabled)
    {
        followPlayer = enabled;
        
        // If enabling following and we're not near the player, teleport close
        if (enabled && playerTransform != null && 
            Vector3.Distance(transform.position, playerTransform.position) > preferredPlayerDistance * 2f)
        {
            TeleportToPlayer();
        }
    }
}