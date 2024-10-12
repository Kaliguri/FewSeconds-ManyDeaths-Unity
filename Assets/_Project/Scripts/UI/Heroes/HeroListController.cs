using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class HeroListController : MonoBehaviour
{
    [Title("GameObject Reference")]
    [Title("Player Info")]
    [SerializeField] Image PlayerColorObj;
    [SerializeField] TextMeshProUGUI NicknameObj;

    [Title("Hero Info")]
    [SerializeField] Image HeroTypeObj; 
    [SerializeField] LocalizeStringEvent HeroNameObj;
    
    [Title("Hero Image")]
    [SerializeField] Image HeroImage;

    [Title("Hero Skills")]
    [SerializeField] List<GameObject> SkillsList;

    [Title("UI")]
    [SerializeField] GameObject inLobbyMenu;
    [SerializeField] GameObject heroChoiceMenu;

    [Title("Other")]
    public int UINumber;

    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();

    private Color PlayerColor => playerInfoData.ColorList[UINumber];
    private string Nickname => playerInfoData.NicknameList[UINumber];

    private HeroData HeroData => playerInfoData.HeroDataList[UINumber];
    
    private Sprite HeroTypeIcon => HeroData.HeroTypeData.Icon;
    private Color HeroTypeIconColor => HeroData.HeroTypeData.IconColor;
    private LocalizedString HeroName => HeroData.Name;

    private GameObject SpritePrefab => HeroData.GameObjectSpritePrefab;

    private int playerID => playerInfoData.PlayerIDThisPlayer;

    void Awake()
    {
        GlobalEventSystem.PlayerInfoDataInitialized.AddListener(DataTransfer);
        GlobalEventSystem.HeroChanged.AddListener(DataTransfer);
        GlobalEventSystem.PlayerDataChanged.AddListener(PlayerDataTransfer);
    }

    void OnEnable()
    {
        DataTransfer();
    }

    public void DataTransfer()
    {
        PlayerDataTransfer();
        HeroDataTransfer();
        HeroImageDataTransfer();
        SkillDataTransfer();
    }

    void PlayerDataTransfer()
    {
        PlayerColorObj.color = PlayerColor;
        NicknameObj.text = Nickname;
    }
    void HeroDataTransfer()
    {
        HeroTypeObj.sprite = HeroTypeIcon;
        HeroTypeObj.color = HeroTypeIconColor;

        HeroNameObj.StringReference = HeroName;
        
    }
    void HeroImageDataTransfer()
    {
        HeroImage.sprite = SpritePrefab.transform.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    void SkillDataTransfer()
    {
        foreach (var Skill in SkillsList)
        {
            Skill.GetComponent<HeroListUISkill>().HeroListID = UINumber;
            Skill.GetComponent<HeroListUISkill>().DataTransfer();
        }
    }

    public void SwitchHeroChoiceUI()
    {
        if (UINumber == playerID)
        {
        inLobbyMenu.SetActive(!inLobbyMenu.activeSelf); 
        heroChoiceMenu.SetActive(!heroChoiceMenu.activeSelf); 
        }
    }

}
