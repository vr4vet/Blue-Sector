using UnityEngine;

public class GameManager : MonoBehaviour
{
    /* ------------------ README ------------------
    
    This script is the main Game Manager for the simulation. It keeps track of the player's progress and the state of the simulation.
    The GameManager is a Singleton, meaning that only one instance of the GameManager can exist at any given time.
    The GameManager is also a MonoBehaviour, meaning that it should be attached to a GameObject in the Unity Editor. This exists as a prefab.
    The GameManager should be present in every scene, and should not be destroyed when loading a new scene to ensure persistence of the player's progress.
    The GameManager is connected to the AudioManager object in every scene to play sound effects. The AudioManager prefab should therefore be present in every scene.
    The GameManager will dynamically find the AudioManager object in the scene, as well as the game objects for the player's hands, to ensure that the player's gloves are set correctly.

    The GameManager is responsible for:
        - Keeping track of the player's progress
        - Playing sound effects
        - Setting the player's gloves based on the player's hand state
        - Setting the HSE room as completed when the player has put on ear protection, gloves, and boots
        - Toggling the room tasks to on and off

    */

    // ----------------- GameManager Instance -----------------

    // The main Game Manager follows the Singleton pattern to ensure that only one instance of the GameManager exists at any given time
    // The instance can be accessed from any script by calling GameManager.Instance
    public static GameManager Instance;

    // ---------------- Enumerator ----------------

    [Tooltip("The possible state of the player's hands")]
    public enum PlayerHandState
    {
        Unsanitized,
        Sanitized,
        BlueGlove,
        SteelGlove,
    }

    // ----------------- Editor Variables -----------------

    [Header("Sound Settings")]
    [SerializeField]
    [Tooltip("The AudioManager object in the scene that will play sound effects.")]
    private GameObject _audioManager;
    public GameObject AudioManager
    {
        set { _audioManager = value; }
        get { return _audioManager; }
    }

    [SerializeField]
    [Tooltip(
        "The sound effects that will be played when the player completes a task. The order should be 'correct', 'wrong', 'taskComplete', 'door', in that order."
    )]
    private AudioClip[] _soundEffects;
    public AudioClip[] SoundEffects
    {
        set { _soundEffects = value; }
        get { return _soundEffects; }
    }

    [Header("Glove Settings")]
    [SerializeField]
    [Tooltip(
        "The game object that contains the material for the left hand. Should be 'fully_gloved' in BNG."
    )]
    private GameObject _leftHandGameObj;
    public GameObject LeftHandGameObj
    {
        get { return _leftHandGameObj; }
        set { _leftHandGameObj = value; }
    }

    [SerializeField]
    [Tooltip(
        "The game object that contains the material for the right hand. Should be 'fully_gloved' in BNG."
    )]
    private GameObject _rightHandGameObj;
    public GameObject RightHandGameObj
    {
        get { return _rightHandGameObj; }
        set { _rightHandGameObj = value; }
    }

    [SerializeField]
    private Material _blueGlove;

    [SerializeField]
    private Material _steelGlove;

    [SerializeField]
    [Tooltip(
        "Whether the HSE room has been completed. The player needs to put on ear protection, gloves, and boots."
    )]
    private bool _hseRoomCompleted = false;
    public bool HSERoomCompleted
    {
        get { return _hseRoomCompleted; }
        set { _hseRoomCompleted = value; }
    }

    [SerializeField]
    [Tooltip("The player's left hand")]
    private PlayerHandState _leftHand = PlayerHandState.Unsanitized;
    public PlayerHandState LeftHand
    {
        get { return _leftHand; }
        set
        {
            _leftHand = value;
            SetPlayerGloves();
            SetHSECompleted();
        }
    }

    [SerializeField]
    [Tooltip("The player's right hand")]
    private PlayerHandState _rightHand = PlayerHandState.Unsanitized;
    public PlayerHandState RightHand
    {
        get { return _rightHand; }
        set
        {
            _rightHand = value;
            SetPlayerGloves();
            SetHSECompleted();
        }
    }

    [Header("Task Settings")]
    [SerializeField]
    [Tooltip("Whether the task is currently active. Task is off when entering a new location.")]
    private bool _isTaskOn = true;
    public bool IsTaskOn
    {
        get { return _isTaskOn; }
        set { _isTaskOn = value; }
    }

    [SerializeField]
    [Tooltip(
        "Whether the secondary task is currently active. Is false when entering a new location."
    )]
    private bool _isSecondaryTaskOn = false;
    public bool IsSecondaryTaskOn
    {
        get { return _isSecondaryTaskOn; }
        set { _isSecondaryTaskOn = value; }
    }

    // ------------- Public Variables -------------

    // Whether the player has put on the boots
    private bool _bootsOn = false;
    public bool BootsOn
    {
        get { return _bootsOn; }
        set
        {
            _bootsOn = value;
            SetHSECompleted();
        }
    }

    // Whether the player has put on ear protection
    private bool _earProtectionOn = false;
    public bool EarProtectionOn
    {
        get { return _earProtectionOn; }
        set
        {
            _earProtectionOn = value;
            SetHSECompleted();
        }
    }

    // The player's position in the next scene
    private Vector3 _nextScenePlayerPosition;
    public Vector3 NextScenePlayerPosition
    {
        get { return _nextScenePlayerPosition; }
        set { _nextScenePlayerPosition = value; }
    }

    // Euler angles for the player's rotation in the next scene
    private Vector3 _nextScenePlayerRotation;
    public Vector3 NextScenePlayerRotation
    {
        get { return _nextScenePlayerRotation; }
        set { _nextScenePlayerRotation = value; }
    }

    // ----------------- Unity Functions -----------------

    void Awake()
    {
        // Sets the instance of the GameManager to this object if it does not already exist
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        // Makes the GameManager object persist between scenes
        DontDestroyOnLoad(gameObject);

        IsTaskOn = false;
    }

    void Update()
    {
        // If the game objects are not set, find them in the current scene
        //TODO: The objects are not searchable at the at the loading of the scene, so this is a workaround
        // This is a limitation of the current implementation of the game, and should be fixed at a later stage
        if (_leftHandGameObj == null || _rightHandGameObj == null || _audioManager == null)
        {
            _audioManager = GameObject.Find("AudioManager");
            _rightHandGameObj = GameObject
                .Find("Green Gloves Right")
                .transform.GetChild(0)
                .gameObject;
            _leftHandGameObj = GameObject
                .Find("Green Gloves Left")
                .transform.GetChild(0)
                .gameObject;
            SetPlayerGloves();
        }
    }

    // ----------------- Public Functions -----------------

    /// <summary>
    /// Plays a sound effect based on the sound name. Connected to the AudioManager object in every scene.
    /// </summary>
    /// <param name="soundName">The name of the sound effect to be played. Either "correct", "incorrect", "taskComplete", or "door"</param>
    public void PlaySound(string soundName)
    {
        AudioSource audioSource = _audioManager.GetComponent<AudioSource>();
        switch (soundName)
        {
            case "correct":
                audioSource.clip = _soundEffects[0];
                break;
            case "incorrect":
                audioSource.clip = _soundEffects[1];
                break;
            case "taskComplete":
                audioSource.clip = _soundEffects[2];
                break;
            case "door":
                audioSource.clip = _soundEffects[3];
                break;
        }
        audioSource.Play();
    }

    /// <summary>
    /// Toggles the main task on and off
    /// </summary>
    public void ToggleTaskOn()
    {
        _isTaskOn = !_isTaskOn;
    }

    /// <summary>
    /// Toggles the secondary task on and off
    /// </summary>
    public void ToggleSecondaryTaskOn()
    {
        _isSecondaryTaskOn = !_isSecondaryTaskOn;
    }

    // ----------------- Private Functions -----------------

    /// <summary>
    /// Sets the HSE room as completed when the player has put on ear protection, gloves, and boots
    /// </summary>
    private void SetHSECompleted()
    {
        bool correctGloveCombo =
            (_leftHand == PlayerHandState.SteelGlove && _rightHand == PlayerHandState.BlueGlove)
            || (_leftHand == PlayerHandState.BlueGlove && _rightHand == PlayerHandState.SteelGlove);

        if (_earProtectionOn && correctGloveCombo && _bootsOn)
        {
            HSERoomCompleted = true;

            PlaySound("taskComplete");
        }
        else
        {
            HSERoomCompleted = false;
        }
    }

    /// <summary>
    /// Sets the player's gloves to the correct material based on the player's hand state
    /// </summary>
    private void SetPlayerGloves()
    {
        if (_leftHand == PlayerHandState.BlueGlove)
        {
            _leftHandGameObj.GetComponent<Renderer>().material = _blueGlove;
        }
        else if (_leftHand == PlayerHandState.SteelGlove)
        {
            _leftHandGameObj.GetComponent<Renderer>().material = _steelGlove;
        }

        if (_rightHand == PlayerHandState.BlueGlove)
        {
            _rightHandGameObj.GetComponent<Renderer>().material = _blueGlove;
        }
        else if (_rightHand == PlayerHandState.SteelGlove)
        {
            _rightHandGameObj.GetComponent<Renderer>().material = _steelGlove;
        }
    }
}
