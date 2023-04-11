using BNG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool startGame = false;
    public bool inActivatedArea = false;
    private GameObject[] merds;
    [SerializeField]
    private int time = 60;
    private float gameTimeLeft;
    private TextMeshProUGUI endScoreText;
    private TextMeshProUGUI timeLeft;
    private TextMeshProUGUI currentScore;
    private TextMeshProUGUI deadFishText;
    private TextMeshProUGUI foodWasteText;
    private UnityEngine.UI.Slider foodWasteSlider;
    Scoring scoring;


    // Start is called before the first frame update
    void Start()
    {
        merds = GameObject.FindGameObjectsWithTag("Fish System");
        scoring = FindObjectOfType<Scoring>();
        endScoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        GameObject canvas = GameObject.FindGameObjectWithTag("MonitorMerdCanvas");
        timeLeft = canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        currentScore = canvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        deadFishText = canvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteText = canvas.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteSlider = canvas.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Slider>();
    }

    /* Update is called once per frame. If the key 'g' is pressed or the A button on the controller is pressed and the game hasn't started, start the game and
     * the coroutine Timer and start scoring. */
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.G) || InputBridge.Instance.AButtonUp) && !startGame && inActivatedArea)
        {
            startGame = true;
            foreach (GameObject merd in merds)
            {
                FishSystemScript merdScript = merd.GetComponent<FishSystemScript>();
                merdScript.ReleaseIdle();   // change all fish systems' states from Idle to Full
            }
            Debug.Log("Started the game");
            scoring.StartScoring();
            StartCoroutine(Timer());
        }
        if (startGame)
        {
            UpdateScreenStats();
        }
    }

    /* Starts a timer and gives the score after a certain amount of time. */
    IEnumerator Timer()
    {
        for (gameTimeLeft = time; gameTimeLeft > 0; gameTimeLeft -= Time.deltaTime)
        {
            yield return null;
        }
        scoring.StopScoring();
        startGame = false;
        foreach (GameObject merd in merds)
        {
            FishSystemScript merdScript = merd.GetComponent<FishSystemScript>();
            merdScript.SetIdle();
        }
        endScoreText.text = "YOUR SCORE:\n" + scoring.Score;
        Debug.Log("End of game");
        Debug.Log("Score: " + scoring.Score);
    }

    /* Updates the timer, score, food waste and the amount of dead fish on the merd screen. */
    public void UpdateScreenStats()
    {
        timeLeft.text = "Time left: " + Mathf.FloorToInt(gameTimeLeft / 60) + ":" +
            Mathf.FloorToInt(gameTimeLeft % 60).ToString("00");
        currentScore.text = "Score: " + scoring.Score;
        foodWasteText.text = "Food wastage: " + scoring.FoodWasted + " / Sec.";
        foodWasteSlider.value = scoring.FoodWastedPercentage;
        deadFishText.text = "Dead fish: " + scoring.DeadFish;
    }

}
