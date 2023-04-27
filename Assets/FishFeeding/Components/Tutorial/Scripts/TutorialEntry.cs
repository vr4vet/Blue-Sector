using BNG;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TutorialEntry : MonoBehaviour
{
    [Tooltip("The text that is shown to the user.")]
    [TextArea]
    public string Text = string.Empty;

    private bool isActive;
    private GameObject currentHint;
    private Popup currentPopup;
    private RectTransform rectTransform;

    /// <summary>
    /// Gets a value indicating whether the tutorial entry is currently visible.
    /// </summary>
    public bool IsActive
    {
        get => isActive;
        internal set
        {
            if (isActive != value)
            {
                isActive = value;
                if (value)
                {
                    currentHint = Instantiate(Tutorial.PopupHint);
                    var tmp = currentHint.GetComponent<TextMeshPro>();
                    tmp.text = Text;
                    currentPopup = currentHint.GetComponent<Popup>();
                    UpdatePopupTransform();
                    currentPopup.Show();
                }
                else
                {
                    currentPopup.Hide();
                    currentHint = null;
                }
            }
        }
    }

    internal Tutorial Tutorial { get; set; }

    public void SetCompleted()
    {
        if (IsActive
            && Tutorial != null
            && Tutorial.Current == this)
        {
            Tutorial.MoveNext();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPopup != null)
        {
            UpdatePopupTransform();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }

        if (rectTransform == null)
        {
            return;
        }

        var rect = rectTransform.rect;
        var center = rectTransform.position + Vector3.Scale((Vector3)rect.center, transform.lossyScale);
        Gizmos.DrawCube(center, Vector3.Scale(rect.size, transform.lossyScale));
    }

    /// <summary>
    /// Updates the current popup to use the rect transform
    /// for this game object.
    /// </summary>
    private void UpdatePopupTransform()
    {
        currentPopup.transform.localScale = transform.localScale;
        currentPopup.transform.position = transform.position;
        var popupRectTransform = (RectTransform)currentPopup.transform;
        popupRectTransform.pivot = rectTransform.pivot;
        popupRectTransform.anchorMin = rectTransform.anchorMin;
        popupRectTransform.anchorMax = rectTransform.anchorMax;
        popupRectTransform.sizeDelta = rectTransform.sizeDelta;
    }
}
