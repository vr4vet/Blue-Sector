using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeMonitor : MonoBehaviour
{
    private float ScrollSpeed = 0.01f;
    private float ScrollSpeedConstant = 0.01f;

    [SerializeField] private Texture DefaultTexture;
    private RawImage RawImage;
    private int SpeedModifier = 1;
    
    public List<int> MagnificationLevels = new List<int> { 2, 4, 8, 16 };
    private Vector2 CurrentXY = new Vector2(0.5f, 0.5f);
    private int CurrentMagnificationStep, CurrentSeparateMagnificationStep = 0;
    private int CurrentImageIndex = 0;

    private TextMeshProUGUI MagnificationLevelOverlay;
    private TextMeshProUGUI SpeedOverlay;

    private MicroscopeSlide CurrentSlide;
    [SerializeField] private RevolvingNosePiece RevolvingNosePiece;



    // Start is called before the first frame update
    void Start()
    {
        RawImage = GetComponentInChildren<RawImage>();
        RawImage.texture = DefaultTexture;

        MagnificationLevelOverlay = transform.Find("Canvas/MagnificationText/Factor").GetComponent<TextMeshProUGUI>();
        SpeedOverlay = transform.Find("Canvas/SpeedText/Factor").GetComponent<TextMeshProUGUI>();

        // set initial magnification level
        SetMagnification(1.0f / MagnificationLevels[CurrentMagnificationStep]);
        SetMagnificationLevelOverlay();
    }

    private void Update()
    {
        // Zooming
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

        // rotate nose piece
        RevolvingNosePiece.transform.RotateAround(RevolvingNosePiece.GetComponent<BoxCollider>().bounds.center, RevolvingNosePiece.transform.up, -90f);

        SetMagnification(1.0f / MagnificationLevels[CurrentMagnificationStep]);
        PreventOutOfBoundsCoordinates();
    }

    public void Minimize()
    {
        CurrentMagnificationStep = (CurrentMagnificationStep - 1) % MagnificationLevels.Count;
        CurrentImageIndex = (CurrentImageIndex - 1) % MagnificationLevels.Count;

        if (CurrentMagnificationStep < 0)
            CurrentMagnificationStep = MagnificationLevels.Count - 1;

        if (CurrentImageIndex < 0)
            CurrentImageIndex = MagnificationLevels.Count - 1;

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
                    CurrentImageSlot--;

                SetTexture(CurrentSlide.textures[CurrentImageSlot]);
                CurrentMagnificationStep = CurrentImageIndex - CurrentImageSlot;
            }

            SetMagnificationLevelOverlay(MagnificationLevels[CurrentImageIndex]);
        }
        else
            SetMagnificationLevelOverlay();

        // rotate nose piece
        RevolvingNosePiece.transform.RotateAround(RevolvingNosePiece.GetComponent<BoxCollider>().bounds.center, RevolvingNosePiece.transform.up, 90f);

        SetMagnification(1.0f / MagnificationLevels[CurrentMagnificationStep]);
        PreventOutOfBoundsCoordinates();
    }

    public void SetMagnification(float magnification)
    {
        // UV x and y must be offset to keep looking at the same point of image when zooming 
        RawImage.uvRect = new Rect(CurrentXY.x - (magnification * 0.5f), CurrentXY.y - (magnification * 0.5f), magnification, magnification);
    }

    public void ScrollRight()
    {
        if (RawImage.uvRect.x < 1 - RawImage.uvRect.width - ScrollSpeed)
        {
            CurrentXY.x += ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x + ScrollSpeed, RawImage.uvRect.y, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    public void ScrollLeft()
    {
        if (RawImage.uvRect.x > ScrollSpeed)
        {
            CurrentXY.x -= ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x - ScrollSpeed, RawImage.uvRect.y, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    public void ScrollUp()
    {
        if (RawImage.uvRect.y < 1 - RawImage.uvRect.height - ScrollSpeed)
        {
            CurrentXY.y += ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x, RawImage.uvRect.y + ScrollSpeed, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    public void ScrollDown()
    {
        if (RawImage.uvRect.y > ScrollSpeed)
        {
            CurrentXY.y -= ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x, RawImage.uvRect.y - ScrollSpeed, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    public void SetTexture(Texture texture)
    {
        RawImage.texture = texture;
    }

    public void SetDefaultTexture()
    {
        RawImage.texture = DefaultTexture;
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
    }

    private void PreventOutOfBoundsCoordinates()
    {
        while (RawImage.uvRect.x < ScrollSpeed)       
            ScrollRight();
        while (RawImage.uvRect.x >= 1 - RawImage.uvRect.width)
            ScrollLeft();
        while (RawImage.uvRect.y < ScrollSpeed)
            ScrollUp();
        while (RawImage.uvRect.y >= 1 - RawImage.uvRect.height)
            ScrollDown();
    }

    public void SetScrollSpeed()
    {
        this.ScrollSpeed = ScrollSpeedConstant * SpeedModifier;
        SpeedOverlay.SetText(SpeedModifier + "x");
    }

    public void SetScrollSpeedConstant(float ScrollSpeedConstant)
    {
        this.ScrollSpeedConstant = ScrollSpeedConstant;
    }

    public void IncreaseScrollSpeed()
    {
        if (SpeedModifier < 3)
        {
            SpeedModifier++;
            SetScrollSpeed();
        }
    }

    public void DecreaseScrollSpeed()
    {
        if (SpeedModifier > 1)
        {
            SpeedModifier--;
            SetScrollSpeed();
        }
    }
}
