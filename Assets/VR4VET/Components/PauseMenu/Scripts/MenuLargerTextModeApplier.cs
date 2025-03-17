using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.PropertyVariants;
using UnityEngine.Localization.Settings;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;


public enum TextType
{
    TextMeshPro,
    UnityEngineUI
}

[System.Serializable]
public class LargerTextComponentSettings
{
    [Tooltip("Text component class. This will decide which of the provided text components to enlarge. Only one will be enlarged.")]
    public TextType TextComponentType;

    [Header("Text components")]
    [Tooltip("The TextMesh Pro text component.")]
    public TMP_Text TMPTextComponent;
    [Tooltip("The UnityEngine UI text component.")]
    public Text UnityEngineUITextComponent;

    [Header("Attributes when enlarged")]
    [Tooltip("Size of the font when larger text accessibility setting is active.")]
    public float FontSize;
    [Tooltip("The scale of the object when larger text is enabled.")]
    public float Scale;
    [Tooltip("Width of the text object after size is changed. This prevents wrapping/formatting issues.")]
    public float Width;
    [Tooltip("Height of the text object after size is changed. This prevents wrapping/formatting issues.")]
    public float Height;

    // hidden variables keeping track of old attribute values
    [HideInInspector] public float OldFontSize, OldWidth, OldHeight;
    [HideInInspector] public Vector3 OldScale;
}

[System.Serializable]
public class LargerUnityUIObjectsSettings
{
    public GameObject Object;
    public float Scale;
    public float Width;
    public float Height;

    // hidden variables keeping track of old attribute values
    [HideInInspector] public float OldFontSize, OldWidth, OldHeight;
    [HideInInspector] public Vector3 OldScale;
}

public class MenuLargerTextModeApplier : MonoBehaviour
{
    private NewMenuManger _newMenuManger;
    [SerializeField] private List<LargerTextComponentSettings> LargerTextComponentSettings = new();
    [SerializeField] private List<LargerUnityUIObjectsSettings> LargerUnityUIObjectsSettings = new();

    // Start is called before the first frame update
    void Start()
    {
        // storing old values so components can be reset
        foreach (LargerTextComponentSettings textSettings in LargerTextComponentSettings)
        {
            RectTransform textTransform = null;
            if (textSettings.TextComponentType == TextType.TextMeshPro)
            {
                textSettings.OldFontSize = textSettings.TMPTextComponent.fontSize;
                textTransform = textSettings.TMPTextComponent.GetComponent<RectTransform>();
            }
            else if (textSettings.TextComponentType == TextType.UnityEngineUI)
            {
                textSettings.OldFontSize = textSettings.UnityEngineUITextComponent.fontSize;
                textTransform = textSettings.UnityEngineUITextComponent.GetComponent<RectTransform>();
            }
            
            textSettings.OldHeight = textTransform.rect.height;
            textSettings.OldWidth = textTransform.rect.width;
            textSettings.OldScale = textTransform.localScale;
        }

        // storing old values so components can be reset
        foreach (LargerUnityUIObjectsSettings objectSettings in LargerUnityUIObjectsSettings)
        {
            RectTransform objectTransform = objectSettings.Object.GetComponent<RectTransform>();
            objectSettings.OldScale = objectTransform.transform.localScale;
            objectSettings.OldWidth = objectTransform.rect.width;
            objectSettings.OldHeight = objectTransform.rect.height;
        }

        // adding listener to Main Menu, so script applies adjustments when Main Menu checkmark is toggled by player or loaded
        _newMenuManger = FindObjectOfType<NewMenuManger>();
        if (_newMenuManger)
            _newMenuManger.m_LargerTextSizeToggled.AddListener(OnLargerTextSizeToggled);
    }

    /// <summary>
    /// Adjusting size of elements if 'enabled' is true and provided values are > 0.
    /// </summary>
    /// <param name="enabled"></param>
    private void OnLargerTextSizeToggled(bool enabled)
    {
        foreach (LargerTextComponentSettings settings in LargerTextComponentSettings)
        { 
            // adjusting font size and getting the right Rect Transform component
            RectTransform textTransform = null;
            if (settings.TextComponentType == TextType.TextMeshPro)
            {
                settings.TMPTextComponent.fontSize = enabled && settings.FontSize > 0 ? settings.FontSize : settings.OldFontSize;
                textTransform = settings.TMPTextComponent.GetComponent<RectTransform>();
            }
            else if (settings.TextComponentType == TextType.UnityEngineUI)
            {
                settings.UnityEngineUITextComponent.fontSize = (int)(enabled && settings.FontSize > 0 ? settings.FontSize : settings.OldFontSize);
                textTransform = settings.UnityEngineUITextComponent.GetComponent<RectTransform>();
            }

            // adjusting scale, width, height
            textTransform.localScale = enabled && settings.Scale > 0 ? settings.OldScale * settings.Scale : settings.OldScale;
            textTransform.rect.Set(
                textTransform.rect.x,
                textTransform.rect.y,
                enabled && settings.Width > 0 ? settings.Width : settings.OldWidth,
                enabled && settings.Height > 0 ? settings.Height : settings.OldHeight);

            // disabling and enabling Localize String Event component (if present) to make it detect changes to font size so it applies the right formatting using smart strings
            LocalizeStringEvent stringEvent;
            if (stringEvent = textTransform.GetComponent<LocalizeStringEvent>())
            {
                stringEvent.enabled = false;
                stringEvent.enabled = true;
            }

            // applying locale variant to make Game Object Localizer component (if present) detect changes to font size so it applies the right formatting using smart strings
            GameObjectLocalizer localizer;
            if (localizer = textTransform.GetComponent<GameObjectLocalizer>())
                localizer.ApplyLocaleVariant(LocalizationSettings.SelectedLocale);
        }

        foreach (LargerUnityUIObjectsSettings settings in LargerUnityUIObjectsSettings)
        { 
            GameObject obj = settings.Object;
            RectTransform objectTransform = obj.GetComponent<RectTransform>();

            // adjusting scale, width, height
            obj.transform.localScale = enabled ? settings.OldScale * settings.Scale : settings.OldScale;
            objectTransform.rect.Set(
                objectTransform.rect.x,
                objectTransform.rect.y,
                enabled && settings.Width > 0 ? settings.Width : settings.OldWidth,
                enabled && settings.Height > 0 ? settings.Height : settings.OldHeight);
        }
    }
}
