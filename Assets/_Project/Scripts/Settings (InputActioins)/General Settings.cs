using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class GeneralSettings : MonoBehaviour
{
    [Title("Gameobject Reference")]
    [SerializeField] TMP_Dropdown languagesDropdown;
    
    private SaveLoader saveLoader => FindObjectOfType<SaveLoader>();

    private string settingsFileName => saveLoader.settingsFileName;
    private List<string> generalSaveNamesList => saveLoader.generalSaveNamesList;
    private List<Locale> languagesList => saveLoader.languagesList;
    

    void Start()
    {
        LanguagesDropdownFill();
    }

    void LanguagesDropdownFill()
    {
        languagesDropdown.ClearOptions();

        var currentLanguageIndex = 0;
        var languagesNameList = new List<string>();

        for (int i = 0; i < languagesList.Count; i++)
        {
            var localeName = languagesList[i].LocaleName;
            languagesNameList.Add(localeName);

            if (localeName == LocalizationSettings.SelectedLocale.LocaleName)
            { currentLanguageIndex = i;}
        }

        languagesDropdown.AddOptions(languagesNameList);
        languagesDropdown.value = currentLanguageIndex;
        languagesDropdown.RefreshShownValue();
        
    }

        public void SetLanguage(int languageIndex)
    {
        Locale locale = languagesList[languageIndex];
        LocalizationSettings.SelectedLocale = locale;

        ES3.Save(generalSaveNamesList[0], languageIndex, filePath: settingsFileName);
        Debug.Log("Save:" + languageIndex);
    }
}
