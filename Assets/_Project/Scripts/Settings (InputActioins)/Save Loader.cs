using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

public class SaveLoader : MonoBehaviour
{
    [FoldoutGroup("Sounds")]
    [Title("Audio Mixer")]
    public AudioMixer audioMixer;
    
    [FoldoutGroup("Sounds")]
    [Title("Volume Names")]
    public List<string> volumeNamesList;

    [FoldoutGroup("Sounds")]
    [Title("Sound Settings")]
    public float defaultVolumeValue;

    void Start()
    {
        Loading();
    }

    void Loading()
    {
        AllVolumeLoading();
    }
    
    void AllVolumeLoading()
    {
        for (int i = 0; i < volumeNamesList.Count; i++)
        {
            AudioMixerVolumeLoading(volumeNamesList[i]);
        }
    }

    void AudioMixerVolumeLoading(string volumeName)
    {
        if (ES3.KeyExists(volumeName))
        {
            float volumeValue = ES3.Load(volumeName, defaultValue: defaultVolumeValue);
            audioMixer.SetFloat(volumeName, volumeValue);
        }
    }
}
