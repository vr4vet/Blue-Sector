using System.Collections.Generic;
using System.Text.RegularExpressions;
using Meta.WitAi.TTS.Integrations;
using Meta.WitAi.TTS.Utilities;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetCharacterModel : MonoBehaviour
{
    [SerializeField] private GameObject _parentObject; // Which Gameobject should the instense of the model be a child of (probably the root of the NPC)
    [HideInInspector] private Vector3 _spawnLocation;
    [SerializeField] private GameObject _characterModelPrefab; // what should the NPC look like
    [SerializeField] private Avatar _avatar; // The animation avatar belonging to the model
    [SerializeField] private int _voicePresetId; // what should the NPC sound like
    [SerializeField] private RuntimeAnimatorController _runtimeAnimatorController;
    // The model with rig
    [HideInInspector] private GameObject _bonesAndSkin;
    [HideInInspector] private GameObject _oldbonesAndSkin;
    // scripts references
    [HideInInspector] private FollowThePlayerController _followThePlayerController;
    [HideInInspector] private DialogueBoxController _dialogueBoxController;
    [HideInInspector] private ConversationController _conversationController;
    // Animator and its parameters
    [HideInInspector] private Animator _animator;
    [HideInInspector] private int _isTalkingHash;
    [HideInInspector] private int _hasNewDialogueOptionsHash;
    [HideInInspector] private int _velocityYHash;
    [HideInInspector] private int _velocityXHash;
    [HideInInspector] private bool _isTalking;
    [HideInInspector] private bool _hasNewDialogueOptions;
    [HideInInspector] private float _velocityY;
    [HideInInspector] private float _velocityX;
    private List<GameObject> _fingers;

    void Awake()
    {
        // set variables
        PrepareAnimationValues();
        _followThePlayerController = _parentObject.GetComponent<FollowThePlayerController>();
        _dialogueBoxController = _parentObject.GetComponent<DialogueBoxController>();
        _conversationController = _parentObject.GetComponentInChildren<ConversationController>();
        _spawnLocation = new Vector3(0, 0, 0);
        CheckMissingComponents();
        NewCharacter();
    }

    private void PrepareAnimationValues() {
        _isTalkingHash = Animator.StringToHash("isTalking");
        _hasNewDialogueOptionsHash = Animator.StringToHash("hasNewDialogueOptions");
        _velocityYHash = Animator.StringToHash("VelocityY");
        _velocityXHash = Animator.StringToHash("VelocityX");
        _isTalking = false;
        _hasNewDialogueOptions = false;
        _velocityY = 0;
        _velocityX = 0;
    }

    private void CheckMissingComponents()
    {
        if (_followThePlayerController == null)
        {
            Debug.LogError("You are missing the script called followThePlayerController");
        }
        if (_dialogueBoxController == null)
        {
            Debug.LogError("You are missing the script called dialogueBoxController");
        }
        if (_conversationController == null)
        {
            Debug.LogError("You are missing the script called conversationController");
        }
    }

    private void NewCharacter()
    {
        // Instansiate the model (consisting of skin and bones (rig) as a child of the ParentObject
        // at the parentObjects location and with the same rotation
        _bonesAndSkin = Instantiate(_characterModelPrefab, _spawnLocation, Quaternion.identity);
        _bonesAndSkin.transform.SetParent(_parentObject.transform);
        Vector3 bonesAndSkinLocation = new Vector3(_spawnLocation.x, _spawnLocation.y, _spawnLocation.z);
        _bonesAndSkin.transform.localPosition = bonesAndSkinLocation;
        _bonesAndSkin.transform.rotation = Quaternion.Euler(new Vector3(_parentObject.transform.eulerAngles.x, _parentObject.transform.eulerAngles.y, _parentObject.transform.eulerAngles.z)); 

        // get or create a new animator and transfer over the old animation paramet values to this new one
        _animator = _bonesAndSkin.GetComponent<Animator>();
        if (_animator == null)
        {
            _bonesAndSkin.AddComponent<Animator>();
            _animator = _bonesAndSkin.GetComponent<Animator>();
        }
        _animator.runtimeAnimatorController = _runtimeAnimatorController;
        _animator.avatar = _avatar;
        // Resetting the old values
        SetNewAnimationValues();
        // We need to tell some other scripts that we changed the animator, so they can continue to refernce it correctly
        UpdateOtherScripts(_animator);

        // change the voice
        TTSWit ttsWitService = _parentObject.GetComponentInChildren<TTSWit>();
        TTSSpeaker ttsSpeaker = _parentObject.GetComponentInChildren<TTSSpeaker>();
        if (ttsWitService != null && ttsSpeaker != null)
        {
            // Change the voice of the NPC
            ttsSpeaker.ClearVoiceOverride();
            ttsSpeaker.GetComponentInChildren<TTSSpeaker>().SetVoiceOverride(ttsWitService.GetAllPresetVoiceSettings()[_voicePresetId]);
        }

        // Attach rigbuilder to the head of the NPC to enable looking at player
        // Start by attaching rigbuilder to character (bonesAndSkin)
        if (_bonesAndSkin.GetComponent<RigBuilder>() == null) {
            RigBuilder rigBuilder = _bonesAndSkin.AddComponent<RigBuilder>();
            // Create new rig object and add it to layers of rigbuilder
            GameObject rigObject = new GameObject("TargetTracking");
            rigObject.transform.parent = _bonesAndSkin.transform;
            Rig rig = rigObject.AddComponent<Rig>();
            rigBuilder.layers.Add(new RigLayer(rig));
            // Add last object to containt multi-aim constraints
            GameObject aimObjectHead = new GameObject("AimObjectHead");
            GameObject aimObjectSpine = new GameObject("AimObjectSpine");
            GameObject aimObjectRightArm = new GameObject("AimObjectRightArm");
            GameObject aimObjectRightForeArm = new GameObject("AimObjectRightForeArm");
            GameObject rIndex = new GameObject("RightIndex");
            GameObject rMiddle = new GameObject("RightMiddle");
            GameObject rRing = new GameObject("RightRing");
            GameObject rPinky = new GameObject("RightPinky");
            GameObject rThumb = new GameObject("RightThumb");
            
            aimObjectHead.transform.parent = rigObject.transform;
            aimObjectSpine.transform.parent = rigObject.transform;
            aimObjectRightArm.transform.parent = rigObject.transform;
            aimObjectRightForeArm.transform.parent = rigObject.transform;
            rIndex.transform.parent = rigObject.transform;
            rMiddle.transform.parent = rigObject.transform;
            rRing.transform.parent = rigObject.transform;
            rPinky.transform.parent = rigObject.transform;
            rThumb.transform.parent = rigObject.transform;

            // Add multi-aim constraints components
            MultiAimConstraint constraintsHead = aimObjectHead.AddComponent<MultiAimConstraint>();
            MultiAimConstraint constraintsSpine = aimObjectSpine.AddComponent<MultiAimConstraint>();
            MultiAimConstraint constraintsRightArm = aimObjectRightArm.AddComponent<MultiAimConstraint>();
            MultiAimConstraint constraintsRightForeArm = aimObjectRightForeArm.AddComponent<MultiAimConstraint>();
            ChainIKConstraint constraintsRIndex = rIndex.AddComponent<ChainIKConstraint>();
            ChainIKConstraint constraintsRMiddle = rMiddle.AddComponent<ChainIKConstraint>();
            ChainIKConstraint constraintsRRing = rRing.AddComponent<ChainIKConstraint>();
            ChainIKConstraint constraintsRPinky = rPinky.AddComponent<ChainIKConstraint>();
            ChainIKConstraint constraintsRThumb = rThumb.AddComponent<ChainIKConstraint>();
            
            // Find the NPC head
            Component[] bodyParts = _bonesAndSkin.GetComponentsInChildren<Component>(true);
            GameObject NPCHead = null;
            GameObject NPCSpine = null;
            GameObject NPCRightArm = null;
            GameObject NPCRightForeArm = null;
            GameObject NPCRightIndexFinger = null;
            GameObject NPCRightMiddleFinger = null;
            GameObject NPCRightRingFinger = null;
            GameObject NPCRightPinkyFinger = null;
            GameObject NPCRightThumbFinger = null;
            // Create list of fingers
            _fingers = new List<GameObject>();
            foreach (Component comp in bodyParts) {
                // Return component if name contains "Head"
                if (comp.gameObject.name.Contains(":Head") && NPCHead == null) { NPCHead = comp.gameObject; }
                // Return component if name contains "Spine"
                if (comp.gameObject.name.Contains(":Spine2") && NPCSpine == null) {NPCSpine = comp.gameObject; }
                // Return component if name contains "RightArm"
                if (comp.gameObject.name.Contains(":RightArm") && NPCRightArm == null) {NPCRightArm = comp.gameObject; }
                // Return component if name contains "RightForeArm"
                if (comp.gameObject.name.Contains(":RightForeArm") && NPCRightForeArm == null) {NPCRightForeArm = comp.gameObject; }
                
                if (comp.gameObject.name.Contains(":RightHandThumb1")) { NPCRightThumbFinger = comp.gameObject; }
                if (comp.gameObject.name.Contains(":RightHandIndex1")) { NPCRightIndexFinger = comp.gameObject; }
                if (comp.gameObject.name.Contains(":RightHandMiddle1")) { NPCRightMiddleFinger = comp.gameObject; }
                if (comp.gameObject.name.Contains(":RightHandRing1")) { NPCRightRingFinger = comp.gameObject; }
                if (comp.gameObject.name.Contains(":RightHandPinky1")) { NPCRightPinkyFinger = comp.gameObject; }
                

               /* if (Regex.IsMatch(comp.gameObject.name, @"\bmixamorig11:RightHand(?:Thumb|Pinky|Index|Middle|Ring)1\b"))
                {
                    GameObject fingerConstraintObject = new GameObject(comp.gameObject.name + "Constraint");
                    
                    MultiRotationConstraint fingerConstraint = fingerConstraintObject.AddComponent<MultiRotationConstraint>();
                    fingerConstraint.data.constrainedObject = comp.gameObject.transform;
                    
                    _fingers.Add(fingerConstraintObject);
                    
                    Debug.Log(comp.gameObject.name);
                } */
                
                // Stop checking body parts if both are found
                if (NPCHead != null && NPCSpine != null && NPCRightArm != null && NPCRightForeArm != null && NPCRightIndexFinger != null && NPCRightMiddleFinger && NPCRightPinkyFinger != null && NPCRightRingFinger != null && NPCRightThumbFinger) { break; }
            }
            if (NPCHead == null) {
                Debug.LogError("Could not find NPC head");
            } else {
                // Set constrained object to NPC head
                constraintsHead.data.constrainedObject = NPCHead.transform;
            }
            if (NPCSpine == null) {
                Debug.LogError("Could not find NPC spine");
            } else {
                // Set constrained object to NPC spine
                constraintsSpine.data.constrainedObject = NPCSpine.transform;
            }
            if (NPCRightArm == null) {
                Debug.LogError("Could not find NPC right arm");
            } else {
                // Set constrained object to NPC right arm
                constraintsRightArm.data.constrainedObject = NPCRightArm.transform;
            }
            
            if (NPCRightForeArm == null) {
                Debug.LogError("Could not find NPC right forearm");
            } else {
                // Set constrained object to NPC right forearm
                constraintsRightForeArm.data.constrainedObject = NPCRightForeArm.transform;
            }
            
            if (NPCRightIndexFinger == null) {
                Debug.LogError("Could not find index finger");
            }
            else
            {
                constraintsRIndex.data.root = NPCRightIndexFinger.transform;
                constraintsRIndex.data.tip = GetFingerTip(NPCRightIndexFinger.transform);
            }
            
            if (NPCRightMiddleFinger == null) {
                Debug.LogError("Could not find middle finger");
            }
            else
            {
                constraintsRMiddle.data.root = NPCRightMiddleFinger.transform;
                constraintsRMiddle.data.tip = GetFingerTip(NPCRightMiddleFinger.transform);
            }
            
            if (NPCRightRingFinger == null) {
                Debug.LogError("Could not find ring finger");
            }
            else
            {
                constraintsRRing.data.root = NPCRightRingFinger.transform;
                constraintsRRing.data.tip = GetFingerTip(NPCRightRingFinger.transform);
            }
            
            if (NPCRightPinkyFinger == null) {
                Debug.LogError("Could not find pinky finger");
            }
            else
            {
                constraintsRPinky.data.root = NPCRightPinkyFinger.transform;
                constraintsRPinky.data.tip = GetFingerTip(NPCRightPinkyFinger.transform);
            }
            
            if (NPCRightThumbFinger == null) {
                Debug.LogError("Could not find thumb finger");
            }
            else
            {
                constraintsRThumb.data.root = NPCRightThumbFinger.transform;
                constraintsRThumb.data.tip = GetFingerTip(NPCRightThumbFinger.transform);
            }
            
            

            // If there is already a animation constraints controller (f.ex from previous model) remove it
            if (_bonesAndSkin.GetComponent<AnimationConstraintsController>() != null) {
                Destroy(_bonesAndSkin.GetComponent<AnimationConstraintsController>());
            }

            // Add the constraints lastly at runtime so the animation constraints fit to the NEW model
            // Makes the rig "rebuild", could be perfomance heavy
            _bonesAndSkin.AddComponent<AnimationConstraintsController>();
            
            Debug.Log("Rigbuilder instantiated and configurated for NPC");
            
           /*GameObject missingChild = GameObject.Find("mixamorig11:RightHandIndex1Constraint");
            if (missingChild != null) {
                
                Debug.Log("Found missing child:" + missingChild.name);
                missingChild.transform.parent = rightFingers.transform;
                
            } else {
                
                Debug.Log("Could not find missing child");
            } */

        } else {
            Debug.Log("RigBuilder already instantiated on NPC");
        }
    }

    public void ChangeCharacter(GameObject characterModelPrefab, Avatar avatar, RuntimeAnimatorController runtimeAnimatorController, int voicePresetId)
    {
        // keep a reference to the old stuff, so it safely can be destroyed later
        _oldbonesAndSkin = _bonesAndSkin;
        // Save all the old animator parameter values
        this._characterModelPrefab = characterModelPrefab;
        this._avatar = avatar;
        this._runtimeAnimatorController = runtimeAnimatorController;
        SaveOldAnimationValues();
        this._voicePresetId = voicePresetId;
        // destory the old stuff
        Destroy(_oldbonesAndSkin);
        // Add the new model and set the saved values to the new animator
        NewCharacter();
    }

    private void UpdateOtherScripts(Animator animator)
    {
        _dialogueBoxController.updateAnimator(animator);
        _followThePlayerController.UpdateAnimator(animator);
        _conversationController.updateAnimator(animator);
    }

    private void SaveOldAnimationValues() {
        if (_runtimeAnimatorController.name.Contains("NPCHumanoidAnimationController")) {
            _isTalking = _animator.GetBool(_isTalkingHash);
            _hasNewDialogueOptions = _animator.GetBool(_hasNewDialogueOptionsHash);
            _velocityX = _animator.GetFloat(_velocityXHash);
            _velocityY = _animator.GetFloat(_velocityYHash);
        } else {
            Debug.LogError("You are not settubg the animation values");
        }
    }

    private void SetNewAnimationValues() {
        if (_runtimeAnimatorController.name.Contains("NPCHumanoidAnimationController")) {
            _animator.SetBool(_isTalkingHash, _isTalking);
            _animator.SetBool(_hasNewDialogueOptionsHash, _hasNewDialogueOptions);
            _animator.SetFloat(_velocityXHash, _velocityX);
            _animator.SetFloat(_velocityYHash, _velocityY);
        } else {
            Debug.LogError("You are not settubg the animation values");
        }
    }

    public Transform GetFingerTip(Transform parent)
    {
        Transform child = parent.GetChild(0);
        Transform grandChild = child.GetChild(0);

        return grandChild.GetChild(0);
    }

}
