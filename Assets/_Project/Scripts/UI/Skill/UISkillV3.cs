
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class UISkillV3 : MonoBehaviour
{

    [Title("GameObj")]
    [SerializeField] protected Image IconObj;
    [SerializeField] protected TooltipV3SkillManager tooltipManager;
    

    [Title("Other")]
    public int UINumber;

    protected PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    protected PlayerSkillManager playerSkillManager => GameObject.FindObjectOfType<PlayerSkillManager>();

    protected int playerID => playerInfoData.PlayerIDThisPlayer;
    protected int variation => playerInfoData.SkillChoiceList[playerID].variationList[UINumber];

    protected HeroData heroData;
    protected SkillData skillData;

    void Awake()
    {
        GlobalEventSystem.SkillChanged.AddListener(DataTransfer);
        GlobalEventSystem.PlayerInfoDataInitialized.AddListener(DataTransfer);
        GlobalEventSystem.StartCombat.AddListener(DataTransfer);
    }

    public virtual void DataTransfer()
    {
        heroData = playerInfoData.HeroDataList[playerID];
        skillData = heroData.SkillList[UINumber].SkillVariationsList[variation];

        IconObj.sprite = skillData.SkillIcon;
        tooltipManager.SkillDataTransfer(skillData);
    }

    public void SelectSkill()
    {
        playerSkillManager.GetSkill(skillData.SkillScript.Select(), UINumber);
    }
}

