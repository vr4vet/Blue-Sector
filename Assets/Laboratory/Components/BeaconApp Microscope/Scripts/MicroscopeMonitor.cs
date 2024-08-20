using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeMonitor : MonoBehaviour
{
    [SerializeField] private float ScrollSpeed = 0.01f;
    [SerializeField] private Texture DefaultTexture;
    public List<int> MagnificationLevels = new List<int> { 2, 4, 8, 16 };
    private RawImage RawImage;
    private Vector2 CurrentXY = new Vector2(0.5f, 0.5f);
    private int CurrentMagnificationStep, CurrentSeparateMagnificationStep = 0;
    private int CurrentImageIndex = 0;
    private TextMeshProUGUI MagnificationLevelOverlay;
    private MicroscopeSlide CurrentSlide;


    // Start is called before the first frame update
    void Start()
    {
        RawImage = GetComponentInChildren<RawImage>();
        RawImage.texture = DefaultTexture;

        MagnificationLevelOverlay = transform.Find("Canvas/Panel/Factor").GetComponent<TextMeshProUGUI>();

        //RawImage.uvRect = new Rect(CurrentXY.x, CurrentXY.y, MagnificationLevels[CurrentMagnificationStep], MagnificationLevels[CurrentMagnificationStep]);

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

        Debug.Log("UVrect x: " + RawImage.uvRect.x);
        Debug.Log("UVrect y: " + RawImage.uvRect.y);
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

    public void Magnify()
    {
        CurrentMagnificationStep = (CurrentMagnificationStep + 1) % MagnificationLevels.Count;
        CurrentImageIndex = (CurrentImageIndex + 1) % MagnificationLevels.Count;

        if (CurrentSlide.UseSeparateMagnificationTextures)
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

        SetMagnification(1.0f / MagnificationLevels[CurrentMagnificationStep]);
        PreventOutOfBoundsCoordinates();
    }

    public void Minimize()
    {
        CurrentMagnificationStep = (CurrentMagnificationStep - 1) % MagnificationLevels.Count;
        CurrentImageIndex = (CurrentImageIndex - 1) % MagnificationLevels.Count;

        if (CurrentMagnificationStep < 0)
            CurrentMagnificationStep = MagnificationLevels.Count - 1;

        if (CurrentSlide.UseSeparateMagnificationTextures)
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

        SetMagnification(1.0f / MagnificationLevels[CurrentMagnificationStep]);
        PreventOutOfBoundsCoordinates();
    }

    public void SetMagnification(float magnification)
    {
        // UV x and y must be offset to keep looking at the same point of image when zooming 
        RawImage.uvRect = new Rect(CurrentXY.x - (magnification * 0.5f), CurrentXY.y - (magnification * 0.5f), magnification, magnification);
    }

    private void ScrollRight()
    {
        if (RawImage.uvRect.x < 1 - RawImage.uvRect.width - ScrollSpeed)
        {
            CurrentXY.x += ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x + ScrollSpeed, RawImage.uvRect.y, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    private void ScrollLeft()
    {
        if (RawImage.uvRect.x > ScrollSpeed)
        {
            CurrentXY.x -= ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x - ScrollSpeed, RawImage.uvRect.y, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    private void ScrollUp()
    {
        if (RawImage.uvRect.y < 1 - RawImage.uvRect.height - ScrollSpeed)
        {
            CurrentXY.y += ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x, RawImage.uvRect.y + ScrollSpeed, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    private void ScrollDown()
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
}
