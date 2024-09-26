
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UISkillV3 : MonoBehaviour
{

    [Header("GameObj")]
    [SerializeField] Image IconObj;
    [SerializeField] TooltipV3Manager tooltipManager;

    [Header("Other")]
    public int UINumber;

    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private PlayerSkillManager playerSkillManager => GameObject.FindObjectOfType<PlayerSkillManager>();
    private int playerID => playerInfoData.PlayerIDThisPlayer;
    private int variation => playerInfoData.SkillChoiceList[playerID][UINumber];
    private HeroData heroData => playerInfoData.HeroDataList[playerID];

    private SkillData skillData => heroData.SkillList[UINumber].SkillVariationsList[variation];

    private Sprite skillIcon => skillData.SkillIcon;
    private LocalizedString skillname => skillData.Name;
    private LocalizedString description => skillData.Description;

    private SkillScript skillScript => skillData.SkillScript;

    void Awake()
    {
        GlobalEventSystem.SkillChanged.AddListener(DataTransfer);
    }
    void Start()
    {
        DataTransfer();
    }
    void DataTransfer()
    {
        IconObj.sprite = skillIcon;
        tooltipManager.Header.StringReference = skillname;
        tooltipManager.Description.StringReference = description;
    }

    public void SelectSkill()
    {
        playerSkillManager.GetSkill(skillScript.Select(), UINumber);
    }
}

