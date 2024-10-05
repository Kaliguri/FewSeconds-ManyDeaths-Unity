using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class AboutTypeConroller : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] LocalizeStringEvent HeroTypeNameText;
    [SerializeField] LocalizeStringEvent HeroTypeDescriptionText;
    [SerializeField] Image HeroTypeIcon;

    private PlayerInfoData PlayerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private HeroData HeroData => PlayerInfoData.HeroDataList[PlayerInfoData.PlayerIDThisPlayer];
    private HeroTypeData HeroTypeData => HeroData.HeroTypeData;
    
    void Awake()
    {
        GlobalEventSystem.HeroChanged.AddListener(DataTranfer);
    }
    void Start()
    {
        DataTranfer();
    }
    void DataTranfer()
    {
        HeroTypeNameText.StringReference = HeroTypeData.Name;
        HeroTypeDescriptionText.StringReference = HeroTypeData.Description;
        HeroTypeIcon.sprite = HeroTypeData.Icon;
        HeroTypeIcon.color = HeroTypeData.IconColor;
    
    }
}
