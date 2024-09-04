using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeMonitor : MonoBehaviour
{
    private float ScrollSpeed = 2f;
    private float SpeedModifier = 1;

    [SerializeField] private Sprite DefaultTexture;
    private Image Image;
    
    public List<int> MagnificationLevels = new List<int> { 2, 4, 8, 16 };
    private Vector2 CurrentXY = new Vector2(0f, 0f);
    private int CurrentMagnificationStep, CurrentSeparateMagnificationStep, CurrentCustomMagnificationLevel = 0;
    private int CurrentImageIndex = 0;

    private TextMeshProUGUI MagnificationLevelOverlay;
    private TextMeshProUGUI SpeedOverlay;

    private MicroscopeSlide CurrentSlide;
    private bool HasSlide = false;
    [SerializeField] public RevolvingNosePiece RevolvingNosePiece;

    [SerializeField] private bool UseCustomMagnificationLevels = false;
    [SerializeField] private List<int> CustomMagnificationLevels = new List<int>();

    private float AspectRatio;


    // Start is called before the first frame update
    void Start()
    {
        Image = transform.Find("Canvas/Panel/Image").GetComponent<Image>();
        Image.sprite = DefaultTexture;

        MagnificationLevelOverlay = transform.Find("Canvas/Panel/MagnificationText/Factor").GetComponent<TextMeshProUGUI>();
        SpeedOverlay = transform.Find("Canvas/Panel/SpeedText/Factor").GetComponent<TextMeshProUGUI>();

        AspectRatio = GetComponentInChildren<RectTransform>().sizeDelta.x / GetComponentInChildren<RectTransform>().sizeDelta.y;

        // set initial magnification level
        SetMagnification(1.0f / MagnificationLevels[CurrentMagnificationStep]);

        if (UseCustomMagnificationLevels)
            SetMagnificationLevelOverlay(CustomMagnificationLevels[0]);
        else
            SetMagnificationLevelOverlay();

        // scaling image to fit canvas
        float ratio = GetComponentInChildren<RectTransform>().sizeDelta.x / Image.GetComponent<RectTransform>().sizeDelta.x;
        Image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Image.GetComponent<RectTransform>().sizeDelta.x * ratio);
        Image.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Image.GetComponent<RectTransform>().sizeDelta.y * ratio);
    }

    private void Update()
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
    }

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
    /// The Magnify() and Minimize() methods work in three different ways:
    /// - When a slide containts only one image (must be the first slot).
    /// - When some of a slide's image slots contain an image (again, the first slot must contain an image).
    /// - When all of a slide's image slots contains an image.
    /// 
    /// The code interated through the image slots. A new image is always magnified by a factor of 2, unless found while minimizing with a distance larger than 1 from the original index.
    /// If a slot is empty, it simply goes to the next magnification level in the MagnificationLevels list.
    /// Therefore, this can function perfectly as a reading aid or similair, simply a monitor that magnifies some image, or 
    /// it could swap images every time, or a hybrid.
    /// </summary>
    public void Magnify()
    {
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
        SetMagnification(1.0f / MagnificationLevels[CurrentMagnificationStep]);
        PreventOutOfBoundsCoordinates();

        if (UseCustomMagnificationLevels)
            SetMagnificationLevelOverlay(CustomMagnificationLevels[CurrentCustomMagnificationLevel]);

        if (!HasSlide)
            SetDefaultTexture();
    }

    public void Minimize()
    {
        CurrentMagnificationStep = (CurrentMagnificationStep - 1) % MagnificationLevels.Count;
        CurrentImageIndex = (CurrentImageIndex - 1) % MagnificationLevels.Count;
        CurrentCustomMagnificationLevel = (CurrentCustomMagnificationLevel - 1) % MagnificationLevels.Count;

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
        SetMagnification(1.0f / MagnificationLevels[CurrentMagnificationStep]);
        PreventOutOfBoundsCoordinates();

        if (UseCustomMagnificationLevels)
            SetMagnificationLevelOverlay(CustomMagnificationLevels[CurrentCustomMagnificationLevel]);

        if (!HasSlide)
            SetDefaultTexture();

        
    }

    public void SetMagnification(float magnification)
    {        
        int Scale = GetMagnificationLevel();
        Image.GetComponent<RectTransform>().localScale = new Vector3(Scale, Scale, Scale);
        Image.GetComponent<RectTransform>().localPosition = new Vector3(CurrentXY.x * Scale, CurrentXY.y * Scale, Image.GetComponent<RectTransform>().position.z);
    }

    public void ScrollRight()
    {
        float monitorWidth = GetComponentInChildren<RectTransform>().sizeDelta.x;
        float ratio = (Image.GetComponentInChildren<RectTransform>().sizeDelta.x * GetMagnificationLevel()) - monitorWidth;
        if (Image.GetComponent<RectTransform>().localPosition.x > -(ratio / 2) + (ScrollSpeed * GetMagnificationLevel()))
        {
            CurrentXY.x -= ScrollSpeed;
            Image.GetComponent<RectTransform>().localPosition += new Vector3(-ScrollSpeed * GetMagnificationLevel(), 0f, 0f);
        }
    }

    public void ScrollLeft()
    {
        float monitorWidth = GetComponentInChildren<RectTransform>().sizeDelta.x;
        float ratio = (Image.GetComponentInChildren<RectTransform>().sizeDelta.x * GetMagnificationLevel()) - monitorWidth;
        if (Image.GetComponent<RectTransform>().localPosition.x < (ratio / 2) - (ScrollSpeed * GetMagnificationLevel()))
        {
            CurrentXY.x += ScrollSpeed;
            Image.GetComponent<RectTransform>().localPosition += new Vector3(ScrollSpeed * GetMagnificationLevel(), 0f, 0f);
        }
    }

    public void ScrollUp()
    {
        float monitorHeight = GetComponentInChildren<RectTransform>().sizeDelta.y;
        float ratio = (Image.GetComponentInChildren<RectTransform>().sizeDelta.y * GetMagnificationLevel()) - monitorHeight;
        if (Image.GetComponent<RectTransform>().localPosition.y > -(ratio / 2) + (ScrollSpeed * GetMagnificationLevel()))    
        {
            CurrentXY.y -= ScrollSpeed;
            Image.GetComponent<RectTransform>().localPosition += new Vector3(0f, -ScrollSpeed * GetMagnificationLevel(), 0f);
        }
    }

    public void ScrollDown()
    {
        float monitorHeight = GetComponentInChildren<RectTransform>().sizeDelta.y;
        float ratio = (Image.GetComponentInChildren<RectTransform>().sizeDelta.y * GetMagnificationLevel()) - monitorHeight;
        if (Image.GetComponent<RectTransform>().localPosition.y < (ratio / 2) - (ScrollSpeed * GetMagnificationLevel()))
        {
            CurrentXY.y += ScrollSpeed;
            Image.GetComponent<RectTransform>().localPosition += new Vector3(0f, ScrollSpeed * GetMagnificationLevel(), 0f);
        }
    }

    public void SetTexture(Sprite texture)
    {
        Image.sprite = texture;
    }

    public void SetDefaultTexture()
    {
        Image.sprite= DefaultTexture;
        HasSlide = false;
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
        int imageIndex = CurrentImageIndex;
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

    private void PreventOutOfBoundsCoordinates()
    {
        float ratioWidth = (Image.GetComponentInChildren<RectTransform>().sizeDelta.x * GetMagnificationLevel()) - GetComponentInChildren<RectTransform>().sizeDelta.x;
        float ratioHeight = (Image.GetComponentInChildren<RectTransform>().sizeDelta.y * GetMagnificationLevel()) - GetComponentInChildren<RectTransform>().sizeDelta.y;
        while (Image.GetComponent<RectTransform>().localPosition.x >= (ratioWidth / 2))
            ScrollRight();
        while (Image.GetComponent<RectTransform>().localPosition.x <= -(ratioWidth / 2))
            ScrollLeft();
        while (Image.GetComponent<RectTransform>().localPosition.y >= (ratioHeight / 2))
            ScrollUp();
        while (Image.GetComponent<RectTransform>().localPosition.y <= -(ratioHeight / 2))
            ScrollDown();
    }

    public void SetScrollSpeed()
    {
        this.ScrollSpeed = 2f * SpeedModifier;
        SpeedOverlay.SetText(SpeedModifier + "x");
    }

    public void IncreaseScrollSpeed()
    {
        if (SpeedModifier < 3)
        {
            SpeedModifier += 0.5f;
            SetScrollSpeed();
        }
    }

    public void DecreaseScrollSpeed()
    {
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

    public RectTransform GetImageRectTransform()
    {
        return Image.GetComponent<RectTransform>();
    }

    public Sprite GetImage()
    {
        return Image.sprite;
    }
}
