
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
        IconObj.sprite = skillData.SkillIcon;
        
        tooltipManager.SkillDataTransfer(skillData);
    }

    public void SelectSkill()
    {
        playerSkillManager.GetSkill(skillData.SkillScript.Select(), UINumber);
    }
}

