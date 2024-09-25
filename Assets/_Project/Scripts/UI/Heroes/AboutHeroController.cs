using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AboutHeroController : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private TextMeshProUGUI HeroNameText;
    [SerializeField] private TextMeshProUGUI HeroDescriptionText;
    [SerializeField] private Image HeroIcon;

    [Space]
    [SerializeField] Transform SpriteParent;
    [SerializeField] GameObject SpriteTempObj;

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
        HeroNameText.text = HeroData.Name;
        HeroDescriptionText.text = HeroData.Description;
        HeroIcon.sprite = HeroData.HeroIcon;

        SpriteDataTransfer();
        
    }
    void SpriteDataTransfer()
    {
        var NewSprite = Instantiate(SpritePrefab, SpriteParent);

        NewSprite.transform.position = SpriteTempObj.transform.position;
        NewSprite.transform.localScale = SpriteTempObj.transform.localScale;

        Destroy(SpriteTempObj);

        SpriteTempObj = NewSprite;
    }
        
}
