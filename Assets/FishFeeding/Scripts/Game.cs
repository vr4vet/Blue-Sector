using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BNG;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    private bool hud;
    public bool startGame = false;
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
    public List<Tutorial> tutorials;
    public Mode currentMode;
    public Modes modesClass;
    public List<Mode> modesList;
    public bool InActivatedArea { get; set; }
    public bool IsTutorialCompleted { get; set; }
    public bool CanStartGame => InActivatedArea && IsTutorialCompleted;

    // Start is called before the first frame update
    private void Start()
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

        modesClass = FindObjectOfType<Modes>();
        modesList = modesClass.modesList; // reassign

        tutorials = new(FindObjectsOfType<MonoBehaviour>().OfType<Tutorial>().ToList());
        modesClass.OnModeChanged += ModesClass_OnModeChanged;
    }

    /* Update is called once per frame. If the key 'g' is pressed or the A button on the controller is pressed and the
     * game hasn't started, start the game and the coroutine Timer and start scoring. */

    private void Update()
    {
        currentMode = modesClass.mode;

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

        if ((Input.GetKeyDown(KeyCode.G) || InputBridge.Instance.AButtonUp) && !startGame && CanStartGame)
        {
            // Set mode values
            time = currentMode.timeLimit;
            hud = currentMode.hud;

            currentScore.enabled = hud;
            foodWasteText.enabled = hud;
            foodWasteSlider.enabled = hud;
            foreach (var image in foodWasteSlider.GetComponentsInChildren<UnityEngine.UI.Image>())
            {
                image.enabled = hud;
            }
            deadFishText.enabled = hud;

            startGame = true;

            foreach (GameObject merd in merds)
            {
                FishSystemScript merdScript = merd.GetComponent<FishSystemScript>();
                merdScript.modifier = currentMode.modifier; // Set modifier on timetohungry etc based on mode difficulty
                merdScript.ReleaseIdle();   // change all fish systems' states from Idle to Full
            }

            scoring.StartScoring();
            StartCoroutine(Timer());
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
        scoring.StopScoring();
        startGame = false;
        foreach (GameObject merd in merds)
        {
            FishSystemScript merdScript = merd.GetComponent<FishSystemScript>();
            merdScript.SetIdle();
        }
        endScoreText.text = "YOUR SCORE:\n" + scoring.Score;
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

    private void ModesClass_OnModeChanged(object sender, Mode e)
    {
        currentMode = e;

        // Only disable tutorials if defined in mode
        tutorials.ForEach(tutorial =>
        {
            if (currentMode.tutorial.Equals(Tut.NO))
            {
                tutorial.enabled = false;
                tutorial.Dismiss();
            }
            else
            {
                tutorial.enabled = true;
                tutorial.ResetTutorial();
            }
        });
    }
}