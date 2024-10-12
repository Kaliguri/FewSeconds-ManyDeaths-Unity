
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class HeroListUISkill : UISkillV3
{
    [Header("HeroListUISkill")]
    public int HeroListID;
    new int variation => playerInfoData.SkillChoiceList[HeroListID].variationList[UINumber];


    void Awake()
    {
        GlobalEventSystem.SkillChanged.AddListener(DataTransfer);
        GlobalEventSystem.PlayerInfoDataInitialized.AddListener(DataTransfer);
        GlobalEventSystem.StartCombat.AddListener(DataTransfer);
    }

    public override void DataTransfer()
    {
        heroData = playerInfoData.HeroDataList[HeroListID];
        skillData = heroData.SkillList[UINumber].SkillVariationsList[variation];

        IconObj.sprite = skillData.SkillIcon;
        tooltipManager.SkillDataTransfer(skillData);
    }
}

