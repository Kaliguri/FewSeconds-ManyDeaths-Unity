using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [Title("GameObject Reference")]
    
    [SerializeField] Slider _secondarySlider;
    [SerializeField] Slider _primarySlider;
    [SerializeField] TextMeshProUGUI HPValueText;
    public Image HPFillObj;

    [Title("Settings")]
    [SerializeField] float _lerpSpeed = 0.08f;
    

    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private float _maxHeatlh;
    private float _currentHeatlh;

    private int UINumber;
    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private Color playerColor => playerInfoData.ColorList[UINumber];
    private int playerCount => playerInfoData.PlayerCount;

    void Awake()
    {
        GlobalEventSystem.PlayerHPChanged.AddListener(HPBarUpdate);
        GlobalEventSystem.CombatPlayerDataInStageInitialized.AddListener(Inizialize);
    }

    void Start()
    {
        UINumber = GetComponentInParent<PlayerInfoVisual>().UINumber;
    }

    void OnDisable()
    {
        GlobalEventSystem.PlayerHPChanged.RemoveListener(HPBarUpdate);
    }

    void Inizialize()
    {
        if   ( playerCount >= UINumber + 1) 
        {
        HPFillObj.color = playerColor;
        HPBarUpdate();
        }        
    }


    void HPBarUpdate()
    {
        _maxHeatlh = combatPlayerDataInStage._TotalStatsList[UINumber].general.MaxHP;
        _currentHeatlh = combatPlayerDataInStage._TotalStatsList[UINumber].currentCombat.CurrentHP;

        _primarySlider.maxValue = _maxHeatlh;
        _primarySlider.value = _currentHeatlh;

        _secondarySlider.maxValue = _maxHeatlh;

        HPValueTextUpdate();

    }

    void HPValueTextUpdate()
    {
        HPValueText.text = _currentHeatlh + " / " + _maxHeatlh;
    }
    void Update()
    {
        if (_primarySlider.value != _currentHeatlh)
        {
            _primarySlider.value = _currentHeatlh;
        }

        if (_primarySlider.value != _secondarySlider.value)
        {
            _secondarySlider.value = Mathf.Lerp(_secondarySlider.value, _currentHeatlh, _lerpSpeed);
        }
    }

    [Title ("Damage Apply Test")]
    public float Damage = 30f;
    [Button ("DamageApply!")]
    public void DamageApply()
    {
        combatPlayerDataInStage._TotalStatsList[UINumber].currentCombat.CurrentHP -= Damage;
        GlobalEventSystem.SendPlayerHPChanged();
    }
}
