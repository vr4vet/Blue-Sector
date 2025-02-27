/* Code from the package sample of the Localization package: 
 * https://docs.unity3d.com/Packages/com.unity.localization@1.3/manual/Package-Samples.html
 */

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Initializes a TMP dropdown menu with the available locales and updates the dropdown selection with selected locale.
/// </summary>
public class LanguageSelectionDropdown : MonoBehaviour
{
    TMP_Dropdown m_Dropdown;
    AsyncOperationHandle m_InitializeOperation;

    private int _oldNorwegianIndex, _newNorwegianIndex, _oldEnglishIndex, _newEnglishIndex;


    void Start() {
        // First we setup the dropdown component.
        m_Dropdown = GetComponent<TMP_Dropdown>();
        m_Dropdown.onValueChanged.AddListener(OnSelectionChanged);

        // Clear the options an add a loading message while we wait for the localization system to initialize.
        m_Dropdown.ClearOptions();
        m_Dropdown.options.Add(new TMP_Dropdown.OptionData("Loading..."));
        m_Dropdown.interactable = false;

        // SelectedLocaleAsync will ensure that the locales have been initialized and a locale has been selected.
        m_InitializeOperation = LocalizationSettings.SelectedLocaleAsync;
        if (m_InitializeOperation.IsDone) {
            InitializeCompleted(m_InitializeOperation);
        }
        else {
            m_InitializeOperation.Completed += InitializeCompleted;
        }
    }

    void InitializeCompleted(AsyncOperationHandle obj) {
        // Create an option in the dropdown for each Locale
        var options = new List<string>();
        int selectedOption = 0;
        var locales = LocalizationSettings.AvailableLocales.Locales;
        for (int i = 0; i < locales.Count; ++i) {
            var locale = locales[i];

            if (LocalizationSettings.SelectedLocale == locale)
                selectedOption = i;

            var displayName = locales[i].Identifier.CultureInfo != null ? locales[i].Identifier.CultureInfo.NativeName : locales[i].ToString();
            options.Add(displayName);
        }

        // If we have no Locales then something may have gone wrong.
        if (options.Count == 0) {
            options.Add("No Locales Available");
            m_Dropdown.interactable = false;
        }
        else {
            m_Dropdown.interactable = true;
        }

        // Removing unsupported languages while keeping track of their old index to make language selection work despite there being fewer elements in options list than there are locales
        _oldEnglishIndex = options.IndexOf("English");
        _oldNorwegianIndex = options.IndexOf("norsk");

        options = options.FindAll(x => x.Equals("English") || x.Equals("norsk"));

        _newEnglishIndex = options.IndexOf("English");
        _newNorwegianIndex = options.IndexOf("norsk");

        // Set up drop down menu
        m_Dropdown.ClearOptions();
        m_Dropdown.AddOptions(options);
        m_Dropdown.SetValueWithoutNotify(ConvertToPauseMenuIndex(selectedOption));

        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }

    void OnSelectionChanged(int index) {
        // Unsubscribe from SelectedLocaleChanged so we don't get an unnecessary callback from the change we are about to make.
        LocalizationSettings.SelectedLocaleChanged -= LocalizationSettings_SelectedLocaleChanged;

        var locale = LocalizationSettings.AvailableLocales.Locales[ConvertToLocaleIndex(index)];
        LocalizationSettings.SelectedLocale = locale;

        // Resubscribe to SelectedLocaleChanged so that we can stay in sync with changes that may be made by other scripts.
        LocalizationSettings.SelectedLocaleChanged += LocalizationSettings_SelectedLocaleChanged;
    }

    void LocalizationSettings_SelectedLocaleChanged(Locale locale) {
        // We need to update the dropdown selection to match.
        var selectedIndex = ConvertToLocaleIndex(LocalizationSettings.AvailableLocales.Locales.IndexOf(locale));
        m_Dropdown.SetValueWithoutNotify(selectedIndex);
    }

    /// <summary>
    /// Converting from pause menu language settings index to locale index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    int ConvertToLocaleIndex(int index)
    {
        if (index == _newEnglishIndex) { return _oldEnglishIndex; }
        if (index == _newNorwegianIndex) { return _oldNorwegianIndex; }
        return _oldEnglishIndex;
    }

    /// <summary>
    /// Converting from locale index to pause menu language settings index
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    int ConvertToPauseMenuIndex(int index)
    {
        if (index == _oldEnglishIndex) { return _newEnglishIndex; }
        if (index == _oldNorwegianIndex) { return _newNorwegianIndex; }
        return _newEnglishIndex;
    }
}