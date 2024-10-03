using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

public class SaveLoader : MonoBehaviour
{
    [Title("Settings")]
    public string settingsFileName = "Settings.es3";


    [HideInInspector]
    public Resolution[] resolutionsList;

    [FoldoutGroup("Screen")]
    [Title("Save Names")]
    public List<string> screenSaveNamesList = new List<string>{"Resolution", "WindowMode"};



    [FoldoutGroup("Sound")]
    [Title("Audio Mixer")]
    public AudioMixer audioMixer;
    
    [FoldoutGroup("Sound")]
    [Title("Save Names")]
    public List<string> volumeSaveNamesList = new List<string>{"MasterVolume", "MusicVolume", "SFXVolume"};
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
        ScreenSettingsLoading();
        VolumeSettingsLoading();
    }

    void ScreenSettingsLoading()
    {
        ResolutionsFill();

        if (ES3.KeyExists(screenSaveNamesList[0])) //Resolution Loading
        {
            int resolutionIndex = ES3.Load<int>(screenSaveNamesList[0], filePath: settingsFileName); 

            Resolution resolution = resolutionsList[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        }

        if (ES3.KeyExists(screenSaveNamesList[1])) //WindowMode Loading
        {
            Screen.fullScreen = ES3.Load<bool>(screenSaveNamesList[1], filePath: settingsFileName); 
        }
    }

    void ResolutionsFill()
    {
        resolutionsList = Screen.resolutions;
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
