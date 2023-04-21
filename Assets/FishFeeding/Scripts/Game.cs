using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BNG;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour
{
    private bool hud;
    public bool startGame = false;
    public bool inActivatedArea = false;
    private GameObject[] merds;

    [field: SerializeField]
    private int time = 60;

    private float gameTimeLeft;
    private TextMeshProUGUI endScoreText;
    private TextMeshProUGUI timeLeft;
    private TextMeshProUGUI currentScore;
    private TextMeshProUGUI deadFishText;
    private TextMeshProUGUI foodWasteText;

    [field: SerializeField]
    private UnityEngine.UI.Slider foodWasteSlider;

    public Scoring scoring;
    public List<MonoBehaviour> tutorials;
    public Mode currentMode;
    public Modes modesClass;
    public List<Mode> modesList;

    // Start is called before the first frame update
    private void Start()
    {
        merds = GameObject.FindGameObjectsWithTag("Fish System");
        scoring = FindObjectOfType<Scoring>();
       // endScoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        GameObject canvas = GameObject.FindGameObjectWithTag("MonitorMerdCanvas");
        timeLeft = canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        currentScore = canvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        deadFishText = canvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteText = canvas.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteSlider = canvas.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Slider>();
        
        modesClass = FindObjectOfType<Modes>();
        modesList = modesClass.modesList; // reassign

        tutorials = new(FindObjectsOfType<MonoBehaviour>().OfType<ITutorial>().Cast<MonoBehaviour>().ToList());
    }

    /* Update is called once per frame. If the key 'g' is pressed or the A button on the controller is pressed and the
     * game hasn't started, start the game and the coroutine Timer and start scoring. */

    private void Update()
    {
        if (startGame) // only check for pre game things if not started
        {
            UpdateScreenStats();
            return; // skip rest of update
        }

        if (modesList.Count == 0)
        {
            modesList = modesClass.modesList;
            return; // wait until modes are loaded
        }

        if (/*(Input.GetKeyDown(KeyCode.G) || InputBridge.Instance.AButtonUp) &&*/ !startGame && inActivatedArea)
        {
            // Set mode values
            currentMode = modesClass.mode;
            time = currentMode.timeLimit;
            hud = currentMode.hud;

            if (!hud)
            {
                currentScore.enabled = false;
                foodWasteText.enabled = false;
                foodWasteSlider.enabled = false;
                deadFishText.enabled = false;
            }

            startGame = true;

            foreach (GameObject merd in merds)
            {
                FishSystemScript merdScript = merd.GetComponent<FishSystemScript>();
                merdScript.modifier = currentMode.modifier; // Set modifier on timetohungry etc based on mode difficulty
                merdScript.ReleaseIdle();   // change all fish systems' states from Idle to Full
            }

            scoring.StartScoring();
            StartCoroutine(Timer());

            if (!currentMode.tutorial.Equals(Tut.NO)) return; // Only disable tutorials if defined in mode

            tutorials.ForEach(tutorial => { tutorial.enabled = false; });
        }
    }

    /* Starts a timer and gives the score after a certain amount of time. */

    private IEnumerator Timer()
    {
        if (time == -1) yield return null; // return if game mode has endless time limit
        for (gameTimeLeft = time; gameTimeLeft > 0; gameTimeLeft -= Time.deltaTime)
        {
            yield return null;
        }
        // scoring.StopScoring();
        // startGame = false;
        foreach (GameObject merd in merds)
        {
            FishSystemScript merdScript = merd.GetComponent<FishSystemScript>();
            merdScript.SetIdle();
        }
        // endScoreText.text = "YOUR SCORE:\n" + scoring.Score;
    }

    /* Updates the timer, score, food waste and the amount of dead fish on the merd screen. */

    public void UpdateScreenStats()
    {
        if (time != -1)
        {
            timeLeft.text = "Time left: " + Mathf.FloorToInt(gameTimeLeft / 60) + ":" +
                Mathf.FloorToInt(gameTimeLeft % 60).ToString("00");
        }
        else
        {
            timeLeft.text = "";
        }
        if (!hud) return; // Don't show hud if in mode defined as such

        currentScore.text = "Score: " + scoring.Score;
        foodWasteText.text = "Food wastage: " + scoring.FoodWasted + " / Sec.";
        foodWasteSlider.value = scoring.FoodWastedPercentage;
        deadFishText.text = "Dead fish: " + scoring.DeadFish;
    }
}