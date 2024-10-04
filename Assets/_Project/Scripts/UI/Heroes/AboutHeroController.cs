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
    [SerializeField] Image HeroImage;

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

        HeroImageDataTransfer();
        
    }
    void HeroImageDataTransfer()
    {
        HeroImage.sprite = SpritePrefab.transform.GetComponentInChildren<SpriteRenderer>().sprite;
    }
        
}
