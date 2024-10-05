using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class TypeHeroInHeroPanelConrtoller : MonoBehaviour
{
    [Title("Hero Type")]
    [SerializeField] HeroTypeData heroTypeData;

    [Title("Object Reference")]
    [SerializeField] LocalizeStringEvent HeroTypeNameText;
    [SerializeField] Image HeroTypeIcon;

    public List<GameObject> HeroIcons;

    void Start()
    {
        DataTranfer();
    }
    void DataTranfer()
    {
        HeroTypeNameText.StringReference = heroTypeData.Name;
        HeroTypeIcon.sprite = heroTypeData.Icon;
        HeroTypeIcon.color = heroTypeData.IconColor;
    
    }
}
