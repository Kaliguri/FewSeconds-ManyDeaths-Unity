
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
    protected HeroData heroData => playerInfoData.HeroDataList[playerID];

    protected SkillData skillData => heroData.SkillList[UINumber].SkillVariationsList[variation];

    protected Sprite skillIcon => skillData.SkillIcon;
    protected LocalizedString skillname => skillData.Name;
    protected LocalizedString description => skillData.Description;

    protected SkillScript skillScript => skillData.SkillScript;

    void Awake()
    {
        GlobalEventSystem.SkillChanged.AddListener(DataTransfer);
    }
    void Start()
    {
        DataTransfer();
    }
    public virtual void DataTransfer()
    {
        IconObj.sprite = skillIcon;
        tooltipManager.SkillName.StringReference = skillname;
        tooltipManager.Description.StringReference = description;
    }

    public void SelectSkill()
    {
        playerSkillManager.GetSkill(skillScript.Select(), UINumber);
    }
}

