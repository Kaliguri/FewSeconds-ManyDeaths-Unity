using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SaveLoader : MonoBehaviour
{
    [Title("Settings")]
    [PropertySpace(SpaceAfter = 20)]
    public string settingsFileName = "Settings.es3";

    [FoldoutGroup("General")]
    [Title("Save Names")]
    public List<string> generalSaveNamesList = new List<string>{"Language"};

    [FoldoutGroup("General")]
    public List<Locale> languagesList;


    [FoldoutGroup("Screen")]
    [Title("Save Names")]
    public List<string> screenSaveNamesList = new List<string>{"Resolution", "WindowMode"};

    [HideInInspector]
    public List<Resolution> resolutionsList;



    [FoldoutGroup("Sound")]
    [Title("Audio Mixer")]
    public AudioMixer audioMixer;
    
    [FoldoutGroup("Sound")]
    [Title("Save Names")]
    public List<string> volumeSaveNamesList = new List<string>{"MasterVolume", "MusicVolume", "SFXVolume"};

    [FoldoutGroup("Sound")]
    // MasterVolume, MusicVolume, SFXVolume
    public string sliderSavePrefix = "Slider";
    // MasterVolumeSlider, MusicVolumeSlider, SFXVolumeSlider

    [FoldoutGroup("Sound")]
    [Title("Settings")]
    public float defaultVolumeValue;

    void Start()
    {
        Loading();
    }

    void Loading()
    {
        GeneralSettingsLoading();
        ScreenSettingsLoading();
        VolumeSettingsLoading();
    }

    void GeneralSettingsLoading()
    {
        LanguageListFill();

        if (ES3.KeyExists(generalSaveNamesList[0], filePath: settingsFileName)) //Language Loading
        {
            int localeIndex = ES3.Load<int>(generalSaveNamesList[0], filePath: settingsFileName);

            Locale locale = languagesList[localeIndex];
            LocalizationSettings.SelectedLocale = locale;
            Debug.Log(LocalizationSettings.SelectedLocale.name);
        }
    }

    void LanguageListFill()
    {
        languagesList = LocalizationSettings.AvailableLocales.Locales;
    }

    void ScreenSettingsLoading()
    {
        ResolutionsFill();

        if (ES3.KeyExists(screenSaveNamesList[0], filePath: settingsFileName)) //Resolution Loading
        {
            int resolutionIndex = ES3.Load<int>(screenSaveNamesList[0], filePath: settingsFileName); 

            Resolution resolution = resolutionsList[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        }

        if (ES3.KeyExists(screenSaveNamesList[1], filePath: settingsFileName)) //WindowMode Loading
        {
            Screen.fullScreen = ES3.Load<bool>(screenSaveNamesList[1], filePath: settingsFileName); 
        }
    }

    void ResolutionsFill()
    {
        var allResolutions = Screen.resolutions;
        var currentResfreshRate = Screen.currentResolution.refreshRateRatio;

        resolutionsList = new List<Resolution>();
        for (int i = 0; i < allResolutions.Length; i++)
        {
            if (allResolutions[i].refreshRateRatio.value == currentResfreshRate.value)
            {
                resolutionsList.Add(allResolutions[i]);
            }
        }
        //Debug.Log(resolutionsList.Count);
    }
    
    void VolumeSettingsLoading()
    {
        for (int i = 0; i < volumeSaveNamesList.Count; i++)
        {
            AudioMixerVolumeLoading(volumeSaveNamesList[i]);
        }
    }

    void AudioMixerVolumeLoading(string volumeName)
    {
        float volumeValue = ES3.Load(volumeName, defaultValue: defaultVolumeValue, filePath: settingsFileName);
        audioMixer.SetFloat(volumeName, volumeValue);
        
    }
}
