using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class MicroscopeMonitor : MonoBehaviour
{
    [SerializeField] private float ScrollSpeed = 0.01f;
    [SerializeField] private List<int> MagnificationLevels = new List<int> { 1, 2, 5, 8 };
    private RawImage RawImage;
    private Vector2 CurrentXY = new Vector2(0.5f, 0.5f);

    private int CurrentMagnificationStep = 0;

    // Start is called before the first frame update
    void Start()
    {
        RawImage = GetComponentInChildren<RawImage>();
    }

    private void Update()
    {
        // Zooming
        if (Input.GetKeyDown(KeyCode.Keypad1))
            Magnify();
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
        float Magnification = 1.0f / MagnificationLevels[CurrentMagnificationStep];

        // UV x and y must be offset to keep looking at the same point of image when zooming
        if (CurrentMagnificationStep == 0)
        {
            CurrentXY = new Vector2(0.5f, 0.5f);
            RawImage.uvRect = new Rect(0f, 0f, Magnification, Magnification);
        }
        else
            RawImage.uvRect = new Rect(CurrentXY.x - (Magnification * 0.5f), CurrentXY.y - (Magnification * 0.5f), Magnification, Magnification);
    }

    private void ScrollRight()
    {
        if (RawImage.uvRect.x < 1 - RawImage.uvRect.width)
        {
            CurrentXY.x += ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x + ScrollSpeed, RawImage.uvRect.y, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    private void ScrollLeft()
    {
        if (RawImage.uvRect.x > 0.01f)
        {
            CurrentXY.x -= ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x - ScrollSpeed, RawImage.uvRect.y, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    private void ScrollUp()
    {
        if (RawImage.uvRect.y < 1 - RawImage.uvRect.height)
        {
            CurrentXY.y += ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x, RawImage.uvRect.y + ScrollSpeed, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }

    private void ScrollDown()
    {
        if (RawImage.uvRect.y > 0.01f)
        {
            CurrentXY.y -= ScrollSpeed;
            RawImage.uvRect = new Rect(RawImage.uvRect.x, RawImage.uvRect.y - ScrollSpeed, RawImage.uvRect.width, RawImage.uvRect.height);
        }
    }
}
