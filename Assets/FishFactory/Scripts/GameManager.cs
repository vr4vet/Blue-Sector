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
        - Keeping track of the player's score

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

    [SerializeField]
    [Tooltip("The AudioManager object in the scene that will play sound effects.")]
    private GameObject audioManager;

    [SerializeField]
    [Tooltip(
        "The sound effects that will be played when the player completes a task. The order should be 'correct', 'wrong', 'taskComplete', 'door', in that order."
    )]
    private AudioClip[] soundEffects;

    [SerializeField]
    [Tooltip(
        "The game object that contains the material for the left hand. Should be 'fully_gloved' in BNG."
    )]
    private GameObject leftHandGameObj;

    [SerializeField]
    [Tooltip(
        "The game object that contains the material for the right hand. Should be 'fully_gloved' in BNG."
    )]
    private GameObject rightHandGameObj;

    [SerializeField]
    private Material blueGlove;

    [SerializeField]
    private Material steelGlove;

    [SerializeField]
    [Tooltip(
        "Whether the HSE room has been completed. The player needs to put on ear protection, gloves, and boots."
    )]
    private bool hseRoomCompleted = false;
    public bool HSERoomCompleted
    {
        get { return hseRoomCompleted; }
        set { hseRoomCompleted = value; }
    }

    [SerializeField]
    [Tooltip("The player's left hand")]
    private PlayerHandState leftHand = PlayerHandState.Unsanitized;
    public PlayerHandState LeftHand
    {
        get { return leftHand; }
        set
        {
            leftHand = value;
            SetPlayerGloves();
            SetHSECompleted();
        }
    }

    [SerializeField]
    [Tooltip("The player's right hand")]
    private PlayerHandState rightHand = PlayerHandState.Unsanitized;
    public PlayerHandState RightHand
    {
        get { return rightHand; }
        set
        {
            rightHand = value;
            SetPlayerGloves();
            SetHSECompleted();
        }
    }

    [SerializeField]
    [Tooltip("Whether the task is currently active. Task is off when entering a new location.")]
    private bool isTaskOn = true;
    public bool IsTaskOn
    {
        get { return isTaskOn; }
        set { isTaskOn = value; }
    }

    private int score = 0;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    // ------------- Public Variables -------------

    // Whether the player has put on the boots
    private bool bootsOn = false;
    public bool BootsOn
    {
        get { return bootsOn; }
        set
        {
            bootsOn = value;
            SetHSECompleted();
        }
    }

    // Whether the player has put on ear protection
    private bool earProtectionOn = false;
    public bool EarProtectionOn
    {
        get { return earProtectionOn; }
        set
        {
            earProtectionOn = value;
            SetHSECompleted();
        }
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
        //FIXME: Currently the only way to view score, should be removed at a later stage
        Debug.Log("score " + score);

        // If the game objects are not set, find them in the current scene
        //TODO: The objects are not searchable at the at the loading of the scene, so this is a workaround
        // This is a limitation of the current implementation of the game, and should be fixed at a later stage
        if (leftHandGameObj == null || rightHandGameObj == null || audioManager == null)
        {
            audioManager = GameObject.Find("AudioManager");
            rightHandGameObj = GameObject
                .Find("Green Gloves Right")
                .transform.GetChild(0)
                .gameObject;
            leftHandGameObj = GameObject.Find("Green Gloves Left").transform.GetChild(0).gameObject;
            SetPlayerGloves();
        }
    }

    // ----------------- Public Functions -----------------

    // Connected to the AudioManager object in every scene. to play sound effects
    /// <summary>
    /// Plays a sound effect based on the sound name
    /// </summary>
    /// <param name="soundName">The name of the sound effect to be played. Either "correct", "incorrect", "taskComplete", or "door"</param>
    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "correct":
                audioManager.GetComponent<AudioSource>().clip = soundEffects[0];
                break;
            case "incorrect":
                audioManager.GetComponent<AudioSource>().clip = soundEffects[1];
                break;
            case "taskComplete":
                audioManager.GetComponent<AudioSource>().clip = soundEffects[2];
                break;
            case "door":
                audioManager.GetComponent<AudioSource>().clip = soundEffects[3];
                break;
        }
        audioManager.GetComponent<AudioSource>().Play();
    }

    public void ToggleTaskOn()
    {
        isTaskOn = !isTaskOn;
    }

    // ----------------- Private Functions -----------------

    // Should only be called when the player has put on ear protection, gloves, and boots
    private void SetHSECompleted()
    {
        bool correctGloveCombo =
            (leftHand == PlayerHandState.SteelGlove && rightHand == PlayerHandState.BlueGlove)
            || (leftHand == PlayerHandState.BlueGlove && rightHand == PlayerHandState.SteelGlove);

        if (earProtectionOn && correctGloveCombo && bootsOn)
        {
            HSERoomCompleted = true;
            //FIXME: should play task complete sound, but overruled by correct sound
            PlaySound("taskComplete");
        }
        else
        {
            HSERoomCompleted = false;
        }
    }

    // Sets the player's gloves to the correct material based on the player's hand state
    // The player hand state can be manipulated through the EquipGlove script
    private void SetPlayerGloves()
    {
        if (leftHand == PlayerHandState.BlueGlove)
        {
            leftHandGameObj.GetComponent<Renderer>().material = blueGlove;
        }
        else if (leftHand == PlayerHandState.SteelGlove)
        {
            leftHandGameObj.GetComponent<Renderer>().material = steelGlove;
        }

        if (rightHand == PlayerHandState.BlueGlove)
        {
            rightHandGameObj.GetComponent<Renderer>().material = blueGlove;
        }
        else if (rightHand == PlayerHandState.SteelGlove)
        {
            rightHandGameObj.GetComponent<Renderer>().material = steelGlove;
        }
    }
}
