
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class UISkillLobby : MonoBehaviour
{

    [Header("GameObj")]
    [SerializeField] private Image IconObj;
    [SerializeField] private LocalizeStringEvent NameObj;
    [SerializeField] private LocalizeStringEvent DescriptionObj;

    [Header("Other")]
    public int UINumber;

    [SerializeField] private GameObject HeroList;
    private int HeroListNumber => HeroList.GetComponent<HeroListController>().UINumber;

    [SerializeField] private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    
    private int playerID => playerInfoData.PlayerIDThisPlayer;
    private int variation => playerInfoData.SkillChoiceList[playerID][UINumber];
    private SkillData skillData => playerInfoData.HeroDataList[playerID].SkillList[UINumber].SkillVariationsList[variation]; 

    private Sprite skillIcon => skillData.SkillIcon;
    private LocalizedString skillname => skillData.Name;
    private LocalizedString description => skillData.Description;

    /*void Awake()
    {
        GlobalEventSystem.SkillChanged.AddListener(DataTransfer);
    }
    */
    void Start()
    {
        //skillData = heroData.SkillList[UINumber].SkillVariationsList[variation];
        //DataTransfer();
    }
    
    
    public void DataTransfer()
    {
        IconObj.sprite = skillIcon;
        NameObj.StringReference = skillname;
        DescriptionObj.StringReference = description;
    }

}
