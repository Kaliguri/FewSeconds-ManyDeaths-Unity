using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AboutTypeConroller : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private TextMeshProUGUI HeroTypeNameText;
    [SerializeField] private TextMeshProUGUI HeroTypeDescriptionText;
    [SerializeField] private Image HeroTypeIcon;

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
        HeroTypeNameText.text = HeroTypeData.Name;
        HeroTypeDescriptionText.text = HeroTypeData.Description;
        HeroTypeIcon.sprite = HeroTypeData.Icon;
        HeroTypeIcon.color = HeroTypeData.IconColor;
    
    }
}
