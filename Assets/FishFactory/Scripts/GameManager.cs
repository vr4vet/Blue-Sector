using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ----------------- Public Variables -----------------

    // The main Game Manager is static so that it can be accessed from other scripts
    public static GameManager instance;

    // ---------------- Enumerators ----------------

    public enum LeftGloveEquipped
    {
        False,
        True,
        Steel,
    }

    public enum RightGloveEquipped
    {
        False,
        True,
        Steel,
    }

    // ----------------- Private Variables -----------------

    [SerializeField]
    private GameObject leftHand;

    [SerializeField]
    private GameObject rightHand;

    [SerializeField]
    private Material blueGlove;

    [SerializeField]
    private Material steel;

    private bool earProtectionOn = false;
    public bool EarProtectionOn
    {
        get { return earProtectionOn; }
        set { earProtectionOn = value; }
    }
    private LeftGloveEquipped leftGlove = LeftGloveEquipped.False;
    public LeftGloveEquipped LeftGlove
    {
        get { return leftGlove; }
        set
        {
            leftGlove = value;
            SetPlayerGloves();
        }
    }

    private RightGloveEquipped rightGlove = RightGloveEquipped.False;
    public RightGloveEquipped RightGlove
    {
        get { return rightGlove; }
        set
        {
            rightGlove = value;
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
        Debug.Log("glove" + leftGlove + rightGlove);
    }

    void Awake()
    {
        // Sets the instance of the GameManager to this object if it does not already exist
        if (instance == null)
            instance = this;

        // Makes the GameManager object persist between scenes
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleTaskOn()
    {
        isTaskOn = !isTaskOn;
    }

    void SetPlayerGloves()
    {
        if (leftGlove == LeftGloveEquipped.True)
        {
            leftHand.GetComponent<Renderer>().material = blueGlove;
        }
        else if (leftGlove == LeftGloveEquipped.Steel)
        {
            leftHand.GetComponent<Renderer>().material = steel;
        }

        if (rightGlove == RightGloveEquipped.True)
        {
            rightHand.GetComponent<Renderer>().material = blueGlove;
        }
        else if (rightGlove == RightGloveEquipped.Steel)
        {
            rightHand.GetComponent<Renderer>().material = steel;
        }
    }
}
