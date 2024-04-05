using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ----------------- Public Variables -----------------

    // The main Game Manager is static so that it can be accessed from other scripts
    public static GameManager instance;

    // ----------------- Private Variables -----------------

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
}
