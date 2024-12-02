using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class MicroscopeMonitor : MonoBehaviour
{
    private float ScrollSpeed;
    private float SpeedModifier = 1;
    
    private DialogueBoxController dialogueBoxController;

    [SerializeField] private Sprite DefaultTexture;
    private Image Image;
    
    public List<int> MagnificationLevels = new List<int> { 2, 4, 8, 16 };
    private Vector2 CurrentXY = new Vector2(0f, 0f);
    private int CurrentMagnificationStep, CurrentSeparateMagnificationStep, CurrentCustomMagnificationLevel = 0;
    private int CurrentImageIndex = 0;

    private TextMeshProUGUI MagnificationLevelOverlay;
    private TextMeshProUGUI SpeedOverlay;

    private MicroscopeSlide CurrentSlide;
    
    private MicroscopeSlideWithGrid CurrentSlideWithGrid;
    private GameObject Grid;
    private bool SlideWithGrid = false;

    private bool HasSlide = false;

    [SerializeField] public RevolvingNosePiece RevolvingNosePiece;

    [SerializeField] private bool UseCustomMagnificationLevels = false;
    [SerializeField] private List<int> CustomMagnificationLevels = new List<int>();

    private float AspectRatio;

    private List<int> GridCellRotations = new List<int>();

    private bool PlanktonHighlights = false;


    // Start is called before the first frame update
    void Start()
    {
        dialogueBoxController = FindObjectOfType<DialogueBoxController>();
        Image = transform.Find("Canvas/Panel/Image").GetComponent<Image>();
        Image.sprite = DefaultTexture;

        MagnificationLevelOverlay = transform.Find("Canvas/Panel/MagnificationText/Factor").GetComponent<TextMeshProUGUI>();
        SpeedOverlay = transform.Find("Canvas/Panel/SpeedText/Factor").GetComponent<TextMeshProUGUI>();

        AspectRatio = GetComponentInChildren<RectTransform>().sizeDelta.x / GetComponentInChildren<RectTransform>().sizeDelta.y;

        // set initial magnification level
        SetMagnification();

        // set initial scrol speed
        SetScrollSpeed();

        if (UseCustomMagnificationLevels)
            SetMagnificationLevelOverlay(CustomMagnificationLevels[0]);
        else
            SetMagnificationLevelOverlay();

        // scaling image to fit canvas
        float ratio = GetComponentInChildren<RectTransform>().sizeDelta.x / Image.GetComponent<RectTransform>().sizeDelta.x;
        Image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Image.GetComponent<RectTransform>().sizeDelta.x * ratio);
        Image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Image.GetComponent<RectTransform>().sizeDelta.y * ratio);
    }

/*    private void Update()
    {
        // Zooming
        if (RevolvingNosePiece.IsRotating())
            return;

        if (Input.GetKeyDown(KeyCode.Keypad1))
            Magnify();
        if (Input.GetKeyDown(KeyCode.Keypad2))
            Minimize();
    }

    private void FixedUpdate()
    {
        // Scrolling
        if (Input.GetKey(KeyCode.RightArrow))
            ScrollRight();
        if (Input.GetKey(KeyCode.LeftArrow))
            ScrollLeft();
        if (Input.GetKey(KeyCode.UpArrow))
            ScrollUp();
        if (Input.GetKey(KeyCode.DownArrow))
            ScrollDown();
    }*/

    public void ScrollX(bool right)
    {
        if (right)
            ScrollRight();
        else
            ScrollLeft();
    }

    public void ScrollY(bool up)
    {   
        if (up)
            ScrollUp();
        else
            ScrollDown();
    }

    /// <summary>
    /// The Magnify() and Minimize() methods work in four different ways:
    /// - When a slide containts only one image (must be the first slot).
    /// - When some of a slide's image slots contain an image (again, the first slot must contain an image).
    /// - When all of a slide's image slots contains an image.
    /// - When a slide with a grid layout is used.
    /// 
    /// If a slot is empty, it simply goes to the next magnification level in the MagnificationLevels list.
    /// Therefore, this can function as a reading aid or similair (simply a monitor that magnifies some image), or 
    /// it could swap images every time, or a hybrid, or lastly, displaying the entire grid layout manipulating it like a sigle image.
    /// </summary>
    public void Magnify()
    {
        // skip NPC dialogue forwards when player successfully changes the magnification
        if (dialogueBoxController != null && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[3])
            dialogueBoxController.SkipLine();

        // keep track of the different indexes for magnification
        CurrentMagnificationStep = (CurrentMagnificationStep + 1) % MagnificationLevels.Count;
        CurrentImageIndex = (CurrentImageIndex + 1) % MagnificationLevels.Count;
        CurrentCustomMagnificationLevel = (CurrentCustomMagnificationLevel + 1) % MagnificationLevels.Count;

        if (CurrentSlide != null && CurrentSlide.UseSeparateMagnificationTextures)
        {
            if (CurrentSlide.textures[CurrentImageIndex] != null)
            {
                SetTexture(CurrentSlide.textures[CurrentImageIndex]);
                CurrentMagnificationStep = 0;
            }
            SetMagnificationLevelOverlay(MagnificationLevels[CurrentImageIndex]);
        }
        else
            SetMagnificationLevelOverlay();

        RotateRevolvingNosePiece(false);
        SetMagnification();
        PreventOutOfBoundsCoordinates();

        if (UseCustomMagnificationLevels)
            SetMagnificationLevelOverlay(CustomMagnificationLevels[CurrentCustomMagnificationLevel]);

        if (!HasSlide)
            SetDefaultTexture();
    }

    public void Minimize()
    {
        // skip NPC dialogue forwards when player successfully changes the magnification
        if (dialogueBoxController != null && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[3])
            dialogueBoxController.SkipLine();

        // keep track of the different indexes for magnification
        CurrentMagnificationStep = (CurrentMagnificationStep - 1) % MagnificationLevels.Count;
        CurrentImageIndex = (CurrentImageIndex - 1) % MagnificationLevels.Count;
        CurrentCustomMagnificationLevel = (CurrentCustomMagnificationLevel - 1) % MagnificationLevels.Count;

        // 'wrap around' to the highest magnification level if minimizing from the lowest magnification level
        if (CurrentMagnificationStep < 0)
            CurrentMagnificationStep = MagnificationLevels.Count - 1;

        if (CurrentImageIndex < 0)
            CurrentImageIndex = MagnificationLevels.Count - 1;

        if (CurrentCustomMagnificationLevel < 0)
            CurrentCustomMagnificationLevel = MagnificationLevels.Count - 1;

        if (CurrentSlide != null && CurrentSlide.UseSeparateMagnificationTextures)
        {
            if (CurrentSlide.textures[CurrentImageIndex] != null)
            {
                SetTexture(CurrentSlide.textures[CurrentImageIndex]);
                CurrentMagnificationStep = 0;
            }
            else
            {
                // iterate backwards until an image is found, and set magnification accordingly
                int CurrentImageSlot = CurrentImageIndex == 0 ? MagnificationLevels.Count - 1 : CurrentImageIndex - 1;

                while (CurrentSlide.textures[CurrentImageSlot] == null)
                {
                    CurrentImageSlot--;
                    if (CurrentImageSlot < 0)
                        Debug.LogError("First image slot of microscope slide is empty!");
                }
                    

                SetTexture(CurrentSlide.textures[CurrentImageSlot]);
                CurrentMagnificationStep = CurrentImageIndex - CurrentImageSlot;
            }
            SetMagnificationLevelOverlay(MagnificationLevels[CurrentImageIndex]);
        }
        else
            SetMagnificationLevelOverlay();

        RotateRevolvingNosePiece(true);
        SetMagnification();
        PreventOutOfBoundsCoordinates();

        if (UseCustomMagnificationLevels)
            SetMagnificationLevelOverlay(CustomMagnificationLevels[CurrentCustomMagnificationLevel]);

        if (!HasSlide)
            SetDefaultTexture();
    }

    public void SetMagnification()
    {
        // decide if the water sample is a grid or not before setting magnification
        GameObject sample = GetSample();

        // perform the magnification by scaling the sample canvas
        int Scale = GetMagnificationLevel();
        sample.GetComponent<RectTransform>().localScale = new Vector3(Scale, Scale, Scale);
        sample.GetComponent<RectTransform>().localPosition = new Vector3(CurrentXY.x * Scale, CurrentXY.y * Scale, Image.GetComponent<RectTransform>().position.z);
    }

    public void ScrollRight()
    {
        // skip NPC dialogue forwards when player successfully scrolls
        if (dialogueBoxController != null && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[1])
            dialogueBoxController.SkipLine();

        // decide if the water sample is a grid or not before scrolling
        GameObject sample = GetSample();

        // perform the scrolling while ensuring it doesn't go beyond edges of monitor
        float monitorWidth = GetComponentInChildren<RectTransform>().sizeDelta.x;
        float ratio = (sample.GetComponentInChildren<RectTransform>().sizeDelta.x * GetMagnificationLevel()) - monitorWidth;
        if (sample.GetComponent<RectTransform>().localPosition.x > -(ratio / 2) + (ScrollSpeed * GetMagnificationLevel()))
        {
            CurrentXY.x -= ScrollSpeed;
            sample.GetComponent<RectTransform>().localPosition += new Vector3(-ScrollSpeed * GetMagnificationLevel(), 0f, 0f);
        }
    }

    public void ScrollLeft()
    {
        // skip NPC dialogue forwards when player successfully scrolls
        if (dialogueBoxController != null && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[1])
            dialogueBoxController.SkipLine();

        // decide if the water sample is a grid or not before scrolling
        GameObject sample = GetSample();

        // perform the scrolling while ensuring it doesn't go beyond edges of monitor
        float monitorWidth = GetComponentInChildren<RectTransform>().sizeDelta.x;
        float ratio = (sample.GetComponentInChildren<RectTransform>().sizeDelta.x * GetMagnificationLevel()) - monitorWidth;
        if (sample.GetComponent<RectTransform>().localPosition.x < (ratio / 2) - (ScrollSpeed * GetMagnificationLevel()))
        {
            CurrentXY.x += ScrollSpeed;
            sample.GetComponent<RectTransform>().localPosition += new Vector3(ScrollSpeed * GetMagnificationLevel(), 0f, 0f);
        }
    }

    public void ScrollUp()
    {
        // skip NPC dialogue forwards when player successfully scrolls
        if (dialogueBoxController != null && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[1])
            dialogueBoxController.SkipLine();

        // decide if the water sample is a grid or not before scrolling
        GameObject sample = GetSample();

        // perform the scrolling while ensuring it doesn't go beyond edges of monitor
        float monitorHeight = GetComponentInChildren<RectTransform>().sizeDelta.y;
        float ratio = (sample.GetComponentInChildren<RectTransform>().sizeDelta.y * GetMagnificationLevel()) - monitorHeight;
        if (sample.GetComponent<RectTransform>().localPosition.y > -(ratio / 2) + (ScrollSpeed * GetMagnificationLevel()))
        {
            CurrentXY.y -= ScrollSpeed;
            sample.GetComponent<RectTransform>().localPosition += new Vector3(0f, -ScrollSpeed * GetMagnificationLevel(), 0f);
        }
    }

    public void ScrollDown()
    {
        // skip NPC dialogue forwards when player successfully scrolls
        if (dialogueBoxController != null && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[1])
            dialogueBoxController.SkipLine();

        // decide if the water sample is a grid or not before scrolling
        GameObject sample = GetSample();

        // perform the scrolling while ensuring it doesn't go beyond edges of monitor
        float monitorHeight = GetComponentInChildren<RectTransform>().sizeDelta.y;
        float ratio = (sample.GetComponentInChildren<RectTransform>().sizeDelta.y * GetMagnificationLevel()) - monitorHeight;
        if (sample.GetComponent<RectTransform>().localPosition.y < (ratio / 2) - (ScrollSpeed * GetMagnificationLevel()))
        {
            CurrentXY.y += ScrollSpeed;
            sample.GetComponent<RectTransform>().localPosition += new Vector3(0f, ScrollSpeed * GetMagnificationLevel(), 0f);
        }
    }

    public void SetTexture(Sprite texture)
    {
        Image.sprite = texture;
    }

    public void SetDefaultTexture()
    {
        Image.sprite= DefaultTexture;
        Image.enabled = true;
        HasSlide = false;

        if (Grid != null)
            Destroy(Grid);
    }

    private void SetMagnificationLevelOverlay()
    {
        MagnificationLevelOverlay.SetText(MagnificationLevels[CurrentMagnificationStep].ToString() + "x");
    }

    private void SetMagnificationLevelOverlay(int level)
    {
        MagnificationLevelOverlay.SetText(level + "x");
    }

    public void SetCurrentSlide(MicroscopeSlide slide)
    {
        CurrentSlide = slide;
        HasSlide = true;
        SlideWithGrid = false;
        int imageIndex = CurrentImageIndex;

        // skip NPC dialogue forwards when player successfully places sample under the scope
        if (dialogueBoxController != null && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[0])
            dialogueBoxController.SkipLine();

        if (CurrentSlide.UseSeparateMagnificationTextures)
        {
            while (CurrentSlide.textures[imageIndex] == null)
            {
                imageIndex--;
                if (imageIndex < 0)
                    Debug.LogError("First image slot of microscope slide is empty!");
            }
            SetTexture(CurrentSlide.textures[imageIndex]);
        }
    }

    public void SetCurrentSlideWithGrid(MicroscopeSlideWithGrid slide)
    {
        CurrentSlideWithGrid = slide;
        SlideWithGrid = true;
        HasSlide = true;

        // skip NPC dialogue forwards when player successfully places sample under the scope
        if (dialogueBoxController != null && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[0])
            dialogueBoxController.SkipLine();
    }

    public void SetGrid(GameObject grid)
    {
        Image.enabled = false;
        Grid = GameObject.Instantiate(grid);

        // make grid fetch its amount of plankton. neccessary to let NPC verify the player's answer
        Grid.GetComponent<MicroscopeGrid>().FetchPlanktonCount(); 

        // randomly rotate grid cells and store in list the first time the sample is placed under scope. otherwise read from list and restore rotation from the first time
        if (GridCellRotations.Count == 0)
            GridCellRotations = Grid.GetComponent<MicroscopeGrid>().RandomlyRotateCells();
        else
            Grid.GetComponent<MicroscopeGrid>().RotateCells(GridCellRotations);

        // give highest position among siblings to ensure UI elements are drawn on top
        Grid.transform.SetParent(transform.Find("Canvas/Panel/"));
        Grid.transform.SetAsFirstSibling();  
        
        SetMagnification();
        
        // placing grid canvas flat onto monitor
        Grid.GetComponent<RectTransform>().localEulerAngles = Vector3.zero;

        // rescaling canvas to fit screen
        float biggestCanvas = Mathf.Max(GetComponentInChildren<RectTransform>().sizeDelta.x, Grid.GetComponent<RectTransform>().sizeDelta.x);
        float smallestCanvas = Mathf.Min(GetComponentInChildren<RectTransform>().sizeDelta.x, Grid.GetComponent<RectTransform>().sizeDelta.x);
        float ratio = biggestCanvas / smallestCanvas;

        if (biggestCanvas == GetComponentInChildren<RectTransform>().sizeDelta.x)
            Grid.GetComponent<RectTransform>().localScale *= ratio;
        else
            Grid.GetComponent<RectTransform>().localScale /= ratio;
    }

    private void PreventOutOfBoundsCoordinates()
    {
        // decide if the water sample is a grid or not before scrolling
        GameObject sample = GetSample();

        // decide if sample has scrolled too far, and scroll back if necessary
        float ratioWidth = (sample.GetComponentInChildren<RectTransform>().sizeDelta.x * GetMagnificationLevel()) - GetComponentInChildren<RectTransform>().sizeDelta.x;
        float ratioHeight = (sample.GetComponentInChildren<RectTransform>().sizeDelta.y * GetMagnificationLevel()) - GetComponentInChildren<RectTransform>().sizeDelta.y;
        while (sample.GetComponent<RectTransform>().localPosition.x >= (ratioWidth / 2))
            ScrollRight();
        while (sample.GetComponent<RectTransform>().localPosition.x <= -(ratioWidth / 2))
            ScrollLeft();
        while (sample.GetComponent<RectTransform>().localPosition.y >= (ratioHeight / 2))
            ScrollUp();
        while (sample.GetComponent<RectTransform>().localPosition.y <= -(ratioHeight / 2))
            ScrollDown();
    }

    public void SetScrollSpeed()
    {
        this.ScrollSpeed = .25f * SpeedModifier;
        SpeedOverlay.SetText(SpeedModifier + "x");
    }

    public void IncreaseScrollSpeed()
    {
        // skip NPC dialogue forwards when player successfully adjusts scrolling speed
        if (dialogueBoxController != null && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[2])
            dialogueBoxController.SkipLine();

        if (SpeedModifier < 3)
        {
            SpeedModifier += 0.5f;
            SetScrollSpeed();
        }
    }

    public void DecreaseScrollSpeed()
    {
        // skip NPC dialogue forwards when player successfully adjusts scrolling speed
        if (dialogueBoxController != null && dialogueBoxController._dialogueText.text == dialogueBoxController.dialogueTreeRestart.sections[13].dialogue[2])
            dialogueBoxController.SkipLine();

        if (SpeedModifier > 0.5f)
        {
            SpeedModifier -= 0.5f;
            SetScrollSpeed();
        }
    }

    private void RotateRevolvingNosePiece(bool Right)
    {
        RevolvingNosePiece.RotateNosePiece(Right);
    }

    public int GetMagnificationLevel()
    {
        return MagnificationLevels[CurrentMagnificationStep];
    }

    public float GetMagnification()
    {
        return 1 / MagnificationLevels[CurrentMagnificationStep];
    }

    public Vector2 GetCurrentXY()
    {
        return CurrentXY;
    }

    public Vector3 GetImagePosition()
    {
        return Image.GetComponent<RectTransform>().localPosition;
    }

    public Vector3 GetGridPosition()
    {
        return Grid.GetComponent<RectTransform>().localPosition;
    }

    public List<int> GetGridCellRotations()
    {
        return GridCellRotations; 
    }

    public RectTransform GetImageRectTransform()
    {
        return Image.GetComponent<RectTransform>();
    }

    public Sprite GetImage()
    {
        return Image.sprite;
    }

    public GameObject GetGrid()
    {
        return Grid;
    }

    public bool IsDisplayingGrid()
    {
        return Grid != null;
    }

    private GameObject GetSample()
    {
        if (SlideWithGrid)
            return Grid;
        else
            return Image.gameObject;
    }

    public void OnEnablePlanktonHighlights()
    {
        PlanktonHighlights = true;
        foreach (MicroscopeSlideCell cell in Grid.GetComponentsInChildren<MicroscopeSlideCell>())
        {
            cell.EnablePlanktonHighlights();
        }

    }

    public void OnDisablePlanktonHighlights()
    {
        PlanktonHighlights = false;
        foreach (MicroscopeSlideCell cell in Grid.GetComponentsInChildren<MicroscopeSlideCell>())
        {
            cell.DisablePlanktonHighlights();
        }
    }

    public bool PlankonHighlightsEnabled()
    {
        return PlanktonHighlights;
    }
}
