using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;

public class Game : MonoBehaviour
{
    private bool hud;
    public bool startGame = false;
    private GameObject[] merds;

    [SerializeField] private GameObject MerdCameraHost;
    private MerdCameraController merdCameraController;

    [SerializeField] private int time = 60;

    private float gameTimeLeft;
    private TextMeshProUGUI endScoreText;
    private TextMeshProUGUI timeLeft;
    private TextMeshProUGUI currentScore;
    private TextMeshProUGUI deadFishText;
    private TextMeshProUGUI foodWasteText;
    private TextMeshProUGUI currentMerdText;

    private UnityEngine.UI.Slider foodWasteSlider;
    private UnityEngine.UI.Image foodWasteFillImage;

    [SerializeField] private List<TextMeshProUGUI> staticText;

    private Color32 redColor = new Color32(202, 18, 7, 242);
    private Color32 yellowColor = new Color32(205, 157, 0, 255);
    private Color32 greenColor = new Color32(1, 159, 28, 255);


    [HideInInspector] public Scoring scoring;
    [HideInInspector] public ScoreTablet scoreTablet;
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
        merdCameraController = MerdCameraHost.GetComponent<MerdCameraController>();
        scoring = FindObjectOfType<Scoring>();
        scoreTablet = FindObjectOfType<ScoreTablet>();
        endScoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        GameObject canvas = GameObject.FindGameObjectWithTag("MonitorMerdCanvas");
        timeLeft = canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        currentScore = canvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        deadFishText = canvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteText = canvas.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteSlider = canvas.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Slider>();
        currentMerdText = canvas.transform.GetChild(5).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteFillImage = foodWasteSlider.GetComponentsInChildren<UnityEngine.UI.Image>()[1];

        modesClass = FindObjectOfType<Modes>();
        modesList = modesClass.modesList; // reassign

        tutorials = new(FindObjectsOfType<MonoBehaviour>().OfType<Tutorial>().ToList());
        modesClass.OnModeChanged += ModesClass_OnModeChanged;
        modesClass.OnFinishedLoading += InitializeMode;
    }

    /* Update is called once per frame. If the key 'g' is pressed or the A button on the controller is pressed and the
     * game hasn't started, start the game and the coroutine Timer and start scoring. */
    private void Update()
    {
        UpdateCurrentMerdText();

        if (startGame) // only check for pre game things if not started
        {
            UpdateScreenStats();
            return; // skip rest of update
        }

        if (modesList == null || modesList.Count == 0)
        {
            modesList = modesClass.modesList;
            return; // wait until modes are loaded
        }

        if ((Input.GetKeyDown(KeyCode.G) || InputBridge.Instance.AButtonUp) && !startGame && CanStartGame)
        {
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
        scoreTablet.GiveFinalTabletScore();
        endScoreText.text = scoring.Score.ToString();
    }

    /* Updates the timer, score, food waste and the amount of dead fish on the merd screen. */
    public void UpdateScreenStats()
    {
        if (time != -1)
        {
            timeLeft.text = Mathf.FloorToInt(gameTimeLeft / 60) + ":" +
                Mathf.FloorToInt(gameTimeLeft % 60).ToString("00");
        }
        else
        {
            timeLeft.text = "";
        }
        if (!hud) return; // Don't show hud if in mode defined as such

        currentScore.text = scoring.Score.ToString();
        foodWasteText.text = scoring.FoodWasted.ToString();
        foodWasteSlider.value = scoring.FoodWastedPercentage;

        if (foodWasteSlider.value == 1.0f)
        {
            foodWasteFillImage.enabled = true;
            foodWasteFillImage.color = redColor;
        }
        else if (foodWasteSlider.value == 0.6f)
        {
            foodWasteFillImage.enabled = true;
            foodWasteFillImage.color = yellowColor;
        }
        else if (foodWasteSlider.value == 0.2f)
        {
            foodWasteFillImage.enabled = true;
            foodWasteFillImage.color = greenColor;
        }
        else if (foodWasteSlider.value == 0.0f)
        {
            foodWasteFillImage.enabled = false;
        }

        deadFishText.text = scoring.DeadFish.ToString();
    }

    private void UpdateCurrentMerdText()
    {
        if (merdCameraController.SelectedFishSystem != null)
        {
            currentMerdText.text = merdCameraController.SelectedFishSystem.merdNr.ToString();
        }
    }

    // Update visibility of HUD based on mode
    private void UpdateHUDModeVisibility()
    {
        currentScore.enabled = hud;
        foodWasteText.enabled = hud;
        foodWasteSlider.enabled = hud;
        foreach (var image in foodWasteSlider.GetComponentsInChildren<UnityEngine.UI.Image>())
        {
            image.enabled = hud;
        }
        deadFishText.enabled = hud;
        foreach (TextMeshProUGUI text in staticText)
        {
            text.enabled = hud;
        }
    }

    private void InitializeMode(object sender, Mode e)
    {
        modesList = modesClass.modesList; 
        currentMode = e;

        // Set mode values
        time = currentMode.timeLimit;
        hud = currentMode.hud;

        UpdateHUDModeVisibility();
    }

    private void ModesClass_OnModeChanged(object sender, Mode e)
    {
        currentMode = e;

        // Set mode values
        time = currentMode.timeLimit;
        hud = currentMode.hud;

        UpdateHUDModeVisibility();

        // Only disable tutorials if defined in mode
        tutorials.ForEach(tutorial =>
        {
            if (currentMode.tutorial.Equals(Tut.NO))
            {
                tutorial.Dismiss();
            }
            else
            {
                tutorial.Dismiss();
                tutorial.MoveNext();
                IsTutorialCompleted = false;
            }
        });
    }
}