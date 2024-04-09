using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ----------------- Public Variables -----------------

    // The main Game Manager is static so that it can be accessed from other scripts
    public static GameManager instance;

    // ---------------- Enumerator ----------------

    [Tooltip("The possible state of the player's hands")]
    public enum PlayerHandState
    {
        Unsanitized,
        Sanitized,
        BlueGlove,
        SteelGlove,
    }

    // ----------------- Private Variables -----------------

    [SerializeField]
    private GameObject audioManager;

    [Tooltip(
        "The sound effects that will be played when the player completes a task. The order should be 'correct', 'wrong', 'taskComplete', 'door', in that order."
    )]
    [SerializeField]
    private AudioClip[] soundEffects;

    [Tooltip(
        "The game object that contains the material for the left hand. Should be 'fully_gloved' in BNG."
    )]
    [SerializeField]
    private GameObject leftHandGameObj;

    [Tooltip(
        "The game object that contains the material for the right hand. Should be 'fully_gloved' in BNG."
    )]
    [SerializeField]
    private GameObject rightHandGameObj;

    [SerializeField]
    private Material blueGlove;

    [SerializeField]
    private Material steelGlove;

    [Tooltip(
        "Whether the HSE room has been completed. The player needs to put on ear protection, gloves, and boots."
    )]
    [SerializeField]
    private bool hseRoomCompleted = false;
    public bool HSERoomCompleted
    {
        get { return hseRoomCompleted; }
        set { hseRoomCompleted = value; }
    }

    [Tooltip("Whether the player has put on ear protection")]
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

    [Tooltip("Whether the player has put on the boots")]
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

    private bool isTaskOn = true;
    public bool IsTaskOn
    {
        get { return isTaskOn; }
    }

    private int score = 0;
    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    void Awake()
    {
        // Sets the instance of the GameManager to this object if it does not already exist
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        // Makes the GameManager object persist between scenes
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        //FIXME: Currently the only way to view score, should be removed at a later stage
        Debug.Log("score " + score);
        Debug.Log(hseRoomCompleted ? "HSE Room Completed" : "HSE Room Incomplete");

        // If the game objects are not set, find them in the current scene
        //TODO: The objects are not searchable at the at the loading of the scene, so this is a workaround
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

    // Connected to the AudioManager object in every scene, to play sound effects
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
