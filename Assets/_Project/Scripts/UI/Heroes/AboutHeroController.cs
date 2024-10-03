using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class AboutHeroController : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private LocalizeStringEvent HeroNameText;
    [SerializeField] private LocalizeStringEvent HeroDescriptionText;
    [SerializeField] private Image HeroIcon;

    [Space]
    [SerializeField] Transform SpriteParent;
    [SerializeField] GameObject SpriteTempObj;
    [SerializeField] Material material;

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

        SpriteDataTransfer();
        
    }
    void SpriteDataTransfer()
    {
        var NewSprite = Instantiate(SpritePrefab, SpriteParent);

        NewSprite.transform.position = SpriteTempObj.transform.position;
        NewSprite.transform.localScale = SpriteTempObj.transform.localScale;

        NewSprite.GetComponentInChildren<SpriteRenderer>().sortingLayerName = "UI";
        NewSprite.GetComponentInChildren<SpriteRenderer>().material = material;

        Destroy(SpriteTempObj);

        SpriteTempObj = NewSprite;
    }
        
}
