
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UISkillV2 : MonoBehaviour
{

    [Header("GameObj")]
    [SerializeField] private Image IconObj;
    [SerializeField] private LocalizeStringEvent LeftText;
    [SerializeField] private LocalizeStringEvent RightText;

    [Header("Other")]
    public int UINumber;

    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private PlayerSkillManager playerSkillManager => GameObject.FindObjectOfType<PlayerSkillManager>();
    private int playerID => playerInfoData.PlayerIDThisPlayer;
    private int variation => playerInfoData.SkillChoiceList[playerID].variationList[UINumber];
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
        LeftText.StringReference = skillname;
        RightText.StringReference = description;
    }

    public void SelectSkill()
    {
        playerSkillManager.GetSkill(skillScript.Select(), UINumber);
    }
}

