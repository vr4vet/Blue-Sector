using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

[System.Serializable]
public class LargerTMPTextComponentSettings
{
    [Tooltip("The text component.")]
    public TMP_Text TextComponent;
    [Tooltip("Size of the font when larger text accessibility setting is active.")]
    public float FontSize;
    [Tooltip("The scale of the object when larger text is enabled.")]
    public float Scale;
    [Tooltip("Width of the text object after size is changed. This prevents wrapping/formatting issues.")]
    public float Width;
    [Tooltip("Height of the text object after size is changed. This prevents wrapping/formatting issues.")]
    public float Height;
}

[System.Serializable]
public class LargerUnityUITextComponentSettings
{
    [Tooltip("The text component.")]
    public Text TextComponent;
    [Tooltip("Size of the font when larger text accessibility setting is active.")]
    public float FontSize;
    [Tooltip("Width of the text object after size is changed. This prevents wrapping/formatting issues.")]
    public float Width;
    [Tooltip("Height of the text object after size is changed. This prevents wrapping/formatting issues.")]
    public float Height;
}

[System.Serializable]
public class LargerUnityUIObjectsSettings
{
    public GameObject Object;
    public float Scale;
    public float Width;
    public float Height;
}

public class MenuLargerTextModeApplier : MonoBehaviour
{
    private NewMenuManger _newMenuManger;
    [SerializeField] private List<LargerTMPTextComponentSettings> LargerTMPTextComponentSettings = new();
    [SerializeField] private List<LargerUnityUITextComponentSettings> LargerUnityUITextComponentSettings = new();
    [SerializeField] private List<LargerUnityUIObjectsSettings> LargerUnityUIObjectsSettings = new();
    private List<float> _oldSizes = new(), _oldWidths = new(), _oldHeights = new();
    private List<Vector3> _oldScales = new ();

    // Start is called before the first frame update
    void Start()
    {
        foreach (LargerTMPTextComponentSettings TMPSettings in LargerTMPTextComponentSettings)
            _oldSizes.Add(TMPSettings.TextComponent.fontSize);

        foreach (LargerUnityUIObjectsSettings ObjectSettings in LargerUnityUIObjectsSettings)
            _oldScales.Add(ObjectSettings.Object.transform.localScale);

        _newMenuManger = FindObjectOfType<NewMenuManger>();
        if (_newMenuManger)
            _newMenuManger.m_LargerTextSizeToggled.AddListener(OnLargerTextSizeToggled);
    }


    private void OnLargerTextSizeToggled(bool enabled)
    {
        for (int i = 0; i < LargerTMPTextComponentSettings.Count; i++)
        {
            var setting = LargerTMPTextComponentSettings[i];
            setting.TextComponent.fontSize = enabled ? setting.FontSize : _oldSizes[i];

            LocalizeStringEvent stringEvent;
            if (stringEvent = setting.TextComponent.GetComponent<LocalizeStringEvent>())
            {
                stringEvent.enabled = false;
                stringEvent.enabled = true;
            }
        }

        for (int i = 0; i < LargerUnityUIObjectsSettings.Count; i++)
        {
            var settings = LargerUnityUIObjectsSettings[i];
            GameObject obj = settings.Object;
            obj.transform.localScale = enabled ? _oldScales[i] * settings.Scale : _oldScales[i];
        }
    }
}
