using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageSelectionButtons : MonoBehaviour
{
    private void Start()
    {
        // loading previously selected locale if it was ever set by the player
        int index = PlayerPrefs.GetInt("localeIndex", -1);
        if (index >= 0)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[index];
            Button localeButton = GetComponentsInChildren<Button>().ToList().Find(button => locale.LocaleName.Contains(button.name)); // this (dangerously) assumes the buttons are named the same as the locales (minus country code), for example "Norwegian"
            localeButton.onClick.Invoke(); // onClick event calls ChangeLocale below and sets button checkmarks. Equivelant to the player pointing at the button an clicking it
        }
    }

    /// <summary>
    /// Sets locale as provided locale arguement and saves index to PlayerPrefs.
    /// </summary>
    /// <param name="locale"></param>
    public void ChangeLocale(Locale locale)
    {
        LocalizationSettings.SelectedLocale = locale;
        int index = LocalizationSettings.AvailableLocales.Locales.IndexOf(locale);
        PlayerPrefs.SetInt("localeIndex", index);
    }
}
