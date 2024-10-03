using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Title("Gameobject Reference")]
    [SerializeField] List<Slider> volumeSlidersList;


    private SaveLoader saveLoader => FindObjectOfType<SaveLoader>();
    private string settingsFileName => saveLoader.settingsFileName;
    private AudioMixer audioMixer => saveLoader.audioMixer;
    private float defaultVolumeValue => saveLoader.defaultVolumeValue;

    private List<string> volumeSaveNamesList => saveLoader.volumeSaveNamesList;
    private string sliderSavePrefix => saveLoader.sliderSavePrefix;

    // Start is called before the first frame update

    void Start()
    {
        LoadingSave();
    }

    void LoadingSave()
    {
        for (int i = 0; i < volumeSlidersList.Count; i++)
        { LoadingSliderStartValue(i, volumeSaveNamesList[i]); }
    }
    
    void LoadingSliderStartValue(int sliderNumber, string volumeName)
    {
        float volumeValue = ES3.Load(volumeName + sliderSavePrefix, defaultValue: defaultVolumeValue, filePath: settingsFileName);
        volumeSlidersList[sliderNumber].value = volumeValue;

    }

    public void SetVolume(int sliderNumber)
    {
        float sliderValue = volumeSlidersList[sliderNumber].value;
        float volume = Mathf.Log10(sliderValue)*20;
        
        audioMixer.SetFloat(volumeSaveNamesList[sliderNumber], volume);

        ES3.Save(volumeSaveNamesList[sliderNumber] + sliderSavePrefix, sliderValue, filePath: settingsFileName);
        ES3.Save(volumeSaveNamesList[sliderNumber], volume, filePath: settingsFileName);
    }
}
