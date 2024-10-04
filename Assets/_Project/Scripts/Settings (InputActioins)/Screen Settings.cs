using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSettings : MonoBehaviour
{
    [Title("Gameobject Reference")]
    [SerializeField] TMP_Dropdown resolutionDropdown;
    
    private SaveLoader saveLoader => FindObjectOfType<SaveLoader>();
    private string settingsFileName => saveLoader.settingsFileName;

    private List<string> screenSaveNamesList => saveLoader.screenSaveNamesList;
    private List<Resolution> resolutionsList => saveLoader.resolutionsList;

    void Start()
    {
        ResolutionsDropdownFill();
    }
    void ResolutionsDropdownFill()
    {
        resolutionDropdown.ClearOptions();

        var currentResolutionIndex = 0;
        var resolutionsNameList = new List<string>();

        for (int i = 0; i < resolutionsList.Count; i++)
        {
            //Debug.Log(i);
            var resolution = resolutionsList[i];
            string resolutionName = resolution.width + "x" + resolution.height + " " + (int)resolution.refreshRateRatio.value + "Hz";
            resolutionsNameList.Add(resolutionName);

            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            { currentResolutionIndex = i; }


        }

        resolutionDropdown.AddOptions(resolutionsNameList);
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        //Debug.Log(resolutionsList.Count + " " + resolutionIndex);
        Resolution resolution = resolutionsList[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        ES3.Save(screenSaveNamesList[0], resolutionIndex, filePath: settingsFileName);
    }

    public void SetWindowMode(bool IsFullscreen)
    {
        Screen.fullScreen = IsFullscreen;

        ES3.Save(screenSaveNamesList[1], IsFullscreen, filePath: settingsFileName);
    }



}
