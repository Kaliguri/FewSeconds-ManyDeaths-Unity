using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Title("Slider Reference")]
    [SerializeField] List<Slider> volumeSlidersList;


    private SaveLoader saveLoader => FindObjectOfType<SaveLoader>();
    private AudioMixer audioMixer => saveLoader.audioMixer;
    private List<string> volumeNamesList => saveLoader.volumeNamesList;
    private float defaultVolumeValue => saveLoader.defaultVolumeValue;

    // Start is called before the first frame update

    void Start()
    {
        SetLoadingValues();
    }

    void SetLoadingValues()
    {
        for (int i = 0; i < volumeSlidersList.Count; i++)
        { SetSliderStartValue(i, volumeNamesList[i]); }
    }
    
    void SetSliderStartValue(int sliderNumber, string volumeName)
    {
        if (ES3.KeyExists(volumeName))
        {
            float volumeValue = ES3.Load(volumeName + "Slider", defaultValue: defaultVolumeValue);
            volumeSlidersList[sliderNumber].value = volumeValue;
        }

    }

    public void SetVolume(int sliderNumber)
    {
        float sliderValue = volumeSlidersList[sliderNumber].value;
        float volume = Mathf.Log10(sliderValue)*20;
        
        audioMixer.SetFloat(volumeNamesList[sliderNumber], volume);

        ES3.Save(volumeNamesList[sliderNumber] + "Slider", sliderValue);
        ES3.Save(volumeNamesList[sliderNumber], volume);
    }
}
