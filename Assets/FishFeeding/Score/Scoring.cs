using System.Collections;
using System.Collections.Generic;
using TMPro;
using BNG;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    private int score = 0;
    private GameObject[] merds;
    public bool startGame = false;
    public bool inActivatedArea = false;
    [SerializeField]
    private int time = 60;
    private float gameTimeLeft;
    private int deadFish;
    private float foodWasted;
    private float foodWastedPercentage;
    private TextMeshProUGUI endScoreText;
    private TextMeshProUGUI timeLeft;
    private TextMeshProUGUI currentScore;
    private TextMeshProUGUI deadFishText;
    private TextMeshProUGUI foodWasteText;
    private UnityEngine.UI.Slider foodWasteSlider;
    private MerdCameraController merdCameraController;
    [SerializeField]
    private GameObject MerdCameraHost;


    // Start is called before the first frame update
    void Start()
    {
        merds = GameObject.FindGameObjectsWithTag("Fish System");
        endScoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        GameObject canvas = GameObject.FindGameObjectWithTag("MonitorMerdCanvas");
        timeLeft = canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        currentScore = canvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        deadFishText = canvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteText = canvas.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteSlider = canvas.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Slider>();
        merdCameraController = MerdCameraHost.GetComponent<MerdCameraController>();
    }

    /* Update is called once per frame. If the key 'g' is pressed and the game hasn't started, start the game and
     * the coroutine GiveScore and update the score every second. */
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.G) || InputBridge.Instance.AButtonUp) && !startGame && inActivatedArea)
        {
            /*Debug.Log()*/
            score = 0;
            startGame = true;
            foreach (GameObject merd in merds) {
                FishSystemScript merdScript = merd.GetComponent<FishSystemScript>();
                merdScript.ReleaseIdle();   // change all fish systems' states from Idle to Full
            }
            Debug.Log("Started the game");
            Debug.Log("Score: " + score);
            InvokeRepeating(nameof(UpdateScore), 1.0f, 1.0f);
            StartCoroutine(GiveScore());
        }
        if (startGame)
        {
            UpdateScreenStats();
        }
    }

     /* Gives the score after a certain amount of time. */
    IEnumerator GiveScore()
    {
        for (gameTimeLeft = time; gameTimeLeft > 0; gameTimeLeft -= Time.deltaTime)
        {
            yield return null;
        }
        CancelInvoke("UpdateScore");
        startGame = false;
        foreach (GameObject merd in merds)
        {
            FishSystemScript merdScript = merd.GetComponent<FishSystemScript>();
            merdScript.SetIdle();
        }
        endScoreText.text = "YOUR SCORE:\n" + score;
        Debug.Log("End of game");
        Debug.Log("Score: " + score);
    }

    /* Goes through every merd and checks if it is full, hungry or dead. Based on the status to the merd 
     * add or subtract from the total score of the game. */
    void UpdateScore()
    {
        foreach (GameObject i in merds)
        {
            FishSystemScript script = i.GetComponent<FishSystemScript>();
            if (script.state == FishSystemScript.FishState.Full)
            {
                score += 1 * script.amountOfFish;
            }
            else if (script.state == FishSystemScript.FishState.Hungry)
            {
                score -= (int)(0.5 * script.amountOfFish);
            }
            else
            {
                score -= 1 * script.fishKilled;
            }

            float wastedFoodPoints = script.foodWasted / 10;
            score -= (int)(wastedFoodPoints + 0.5f); // Rounds wastedFoodPoints to the nearest int.
            Debug.Log("Score after food waste: " + score);

            UpdateFoodWasteAndDeadFish();
        }
        Debug.Log("Time left: " + gameTimeLeft);
    }

    void UpdateFoodWasteAndDeadFish()
    {
        foreach (GameObject i in merds)
        {
            FishSystemScript script = i.GetComponent<FishSystemScript>();
            Debug.Log("if setning" + (merdCameraController.SelectedFishSystem == script));
            if (merdCameraController.SelectedFishSystem == script)
            {
                foodWasted = script.foodWasted;
                foodWastedPercentage = script.foodWasted / (script.foodBase * 5 / 3);
                deadFish = script.fishKilled;
            }
        }
    }

    /* Updates the timer, score, food waste and the amount of dead fish on the merd screen. */
    public void UpdateScreenStats()
    {
        timeLeft.text = "Time left: " + Mathf.FloorToInt(gameTimeLeft / 60) + ":" +
            Mathf.FloorToInt(gameTimeLeft % 60).ToString("00");
        currentScore.text = "Score: " + score;
        foodWasteText.text = "Food wastage: " + foodWasted + " / Sec.";
        foodWasteSlider.value = foodWastedPercentage;
        deadFishText.text = "Dead fish: " + deadFish;
    }

}
