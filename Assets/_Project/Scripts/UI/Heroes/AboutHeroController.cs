using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class AboutHeroController : MonoBehaviour
{
    [Title("Object Reference")]
    [SerializeField] private LocalizeStringEvent HeroNameText;
    [SerializeField] private LocalizeStringEvent HeroDescriptionText;
    [SerializeField] private Image HeroIcon;

    [Title("Extra")]
    [SerializeField] Image HeroImage;
    [SerializeField] private LocalizeStringEvent HeroQuote;

    private PlayerInfoData PlayerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private HeroData HeroData => PlayerInfoData.HeroDataList[PlayerInfoData.PlayerIDThisPlayer];
    private GameObject SpritePrefab => HeroData.GameObjectSpritePrefab;

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
        HeroNameText.StringReference = HeroData.Name;
        HeroDescriptionText.StringReference = HeroData.Description;

        HeroIcon.sprite = HeroData.HeroIcon;
        HeroQuote.StringReference = HeroData.Quoute;

        HeroImageDataTransfer();
        
    }
    void HeroImageDataTransfer()
    {
        HeroImage.sprite = SpritePrefab.transform.GetComponentInChildren<SpriteRenderer>().sprite;
    }
        
}
