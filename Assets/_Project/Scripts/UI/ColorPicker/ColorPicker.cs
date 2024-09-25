using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [Title("Colors")]
    public List<Color> ColorList;

    [Title("GameObject Reference")]
    [SerializeField] GameObject ColorPickerWindow;
    [SerializeField] List<GameObject> ColorBoxList;
    private int UINumber => GetComponentInParent<HeroListController>().UINumber;
    private int playerID => GameObject.FindObjectOfType<PlayerInfoData>().GetComponent<PlayerInfoData>().PlayerIDThisPlayer;

    void Start()
    {
        ColorDataTransfer();
    }

    void ColorDataTransfer()
    {
        for (int i = 0; i < ColorBoxList.Count; i++)
        {
            ColorBoxList[i].GetComponent<Image>().color = ColorList[i];
        }
    }

    public void SwitchWindowActive() 
    { 
        if (UINumber == playerID)
        ColorPickerWindow.SetActive(!ColorPickerWindow.activeSelf); 
    }

}
