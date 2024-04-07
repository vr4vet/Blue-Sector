using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ----------------- Public Variables -----------------

    // The main Game Manager is static so that it can be accessed from other scripts
    public static GameManager instance;

    // ---------------- Enumerators ----------------

    public enum PlayerLeftHand
    {
        Unsanitized,
        Sanitized,
        BlueGlove,
        SteelGlove,
    }

    public enum PlayerRightHand
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

    private bool earProtectionOn = false;
    public bool EarProtectionOn
    {
        get { return earProtectionOn; }
        set { earProtectionOn = value; }
    }
    private PlayerLeftHand leftHand = PlayerLeftHand.Unsanitized;
    public PlayerLeftHand LeftHand
    {
        get { return leftHand; }
        set
        {
            leftHand = value;
            SetPlayerGloves();
        }
    }

    private PlayerRightHand rightHand = PlayerRightHand.Unsanitized;
    public PlayerRightHand RightHand
    {
        get { return rightHand; }
        set
        {
            rightHand = value;
            SetPlayerGloves();
        }
    }

    private bool bootsOn = false;
    public bool BootsOn
    {
        get { return bootsOn; }
        set { bootsOn = value; }
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

    void Update()
    {
        Debug.Log("score " + score);
        Debug.Log("hand" + leftHand + rightHand);
    }

    void Awake()
    {
        // Sets the instance of the GameManager to this object if it does not already exist
        if (instance == null)
            instance = this;

        // Makes the GameManager object persist between scenes
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "correct":
                audioManager.GetComponent<AudioSource>().clip = soundEffects[0];
                audioManager.GetComponent<AudioSource>().Play();
                break;
            case "incorrect":
                audioManager.GetComponent<AudioSource>().clip = soundEffects[1];
                audioManager.GetComponent<AudioSource>().Play();
                break;
            case "taskComplete":
                audioManager.GetComponent<AudioSource>().clip = soundEffects[2];
                audioManager.GetComponent<AudioSource>().Play();
                break;
            case "door":
                audioManager.GetComponent<AudioSource>().clip = soundEffects[3];
                audioManager.GetComponent<AudioSource>().Play();
                break;
        }
    }

    public void ToggleTaskOn()
    {
        isTaskOn = !isTaskOn;
    }

    private void SetPlayerGloves()
    {
        if (leftHand == PlayerLeftHand.BlueGlove)
        {
            leftHandGameObj.GetComponent<Renderer>().material = blueGlove;
        }
        else if (leftHand == PlayerLeftHand.SteelGlove)
        {
            leftHandGameObj.GetComponent<Renderer>().material = steelGlove;
        }

        if (rightHand == PlayerRightHand.BlueGlove)
        {
            rightHandGameObj.GetComponent<Renderer>().material = blueGlove;
        }
        else if (rightHand == PlayerRightHand.SteelGlove)
        {
            rightHandGameObj.GetComponent<Renderer>().material = steelGlove;
        }
    }
}
