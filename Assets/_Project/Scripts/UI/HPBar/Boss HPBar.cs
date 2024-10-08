using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    [Title("GameObj Ref")]
    [SerializeField] List<GameObject> HPBarList;

    [Title("Color")]
    [SerializeField] Color ColorCurrentPrimaryHPBar;
    [SerializeField] Color ColorOthersPrimaryHPBar;
    [SerializeField] Color ColorSecondaryHPBar;

    [Title("Secondary Slider")]
    [SerializeField] float _lerpSpeed = 0.04f;
    [SerializeField] float animationSpeed = 40f;
    [SerializeField] float errorDue = 0.2f;

    [Title("Visual Settings")]
    [SerializeField] float OffsetCurrentHPBar = 24f;
    //[ShowInInspector]
    //private float OffsetCurrentHPBar => HPBarList[0].transform.position.x - HPBarList[1].transform.position.x;
    


    private BossHPBarReference CurrentHPBarReference => HPBarList[currentAct].GetComponent<BossHPBarReference>();
    private Slider secondarySliderCurrentHPBar => CurrentHPBarReference.SecondarySlider;
    private Slider primarySliderCurrentHPBar => CurrentHPBarReference.PrimarySlider;

    private BossManager bossManager => FindObjectOfType<BossManager>();
    private int currentAct = 0;
    private float maxHP;
    private float currentHP;

    void Awake()
    {
        GlobalEventSystem.BossHPChanged.AddListener(HPBarUpdate);
        GlobalEventSystem.BossManagerInitialized.AddListener(Inizialize);
        GlobalEventSystem.BossActChanged.AddListener(ChangeAct);
    }

    void Inizialize()
    {
        //CurrentHPBarReference = HPBarList[currentAct].GetComponent<BossHPBarReference>();
        SetColor();
        StartHPBarUpdate();
    }

    void ChangeAct()
    {
        StopAllCoroutines();
        secondarySliderCurrentHPBar.value = 0;

        currentAct ++;
        //CurrentHPBarReference = HPBarList[currentAct].GetComponent<BossHPBarReference>();
        //SwapOffset();
        SetColor();
        StartHPBarUpdate();
    }

    void StartHPBarUpdate()
    {
        maxHP = bossManager.MaxHPInCurrentAct;
        currentHP = bossManager.CurrentHPInCurrentAct;

        primarySliderCurrentHPBar.maxValue = maxHP;
        primarySliderCurrentHPBar.value = currentHP;

        secondarySliderCurrentHPBar.maxValue = maxHP;
        secondarySliderCurrentHPBar.value = currentHP;
    }
    void HPBarUpdate()
    {
        maxHP = bossManager.MaxHPInCurrentAct;
        currentHP = bossManager.CurrentHPInCurrentAct;

        primarySliderCurrentHPBar.maxValue = maxHP;
        primarySliderCurrentHPBar.value = currentHP;

        secondarySliderCurrentHPBar.maxValue = maxHP;

        StartCoroutine(SecondarySlideAnimation(primarySliderCurrentHPBar, secondarySliderCurrentHPBar));
    }

    void SetColor()
    {
        for (int i = 0; i < HPBarList.Count; i++)
        {
            var HPBarRef = HPBarList[i].GetComponent<BossHPBarReference>();

            if (i == currentAct) { HPBarRef.PrimaryFill.color = ColorCurrentPrimaryHPBar; }
            else        { HPBarRef.PrimaryFill.color = ColorOthersPrimaryHPBar;  }

            HPBarRef.SecondaryFill.color = ColorSecondaryHPBar;
        }
    }
    IEnumerator SecondarySlideAnimation(Slider primary, Slider secondary)
    {
        while (primary.value <= secondary.value - errorDue)
        {     
        
        if (primary.value != currentHP)
        {
            primary.value = currentHP;
        }

        if (primary.value != secondary.value)
        {
            secondary.value = Mathf.Lerp(secondary.value, currentHP, _lerpSpeed);
        }
        //Debug.Log(primary.value+ ": " + secondary.value);
        yield return new WaitForSeconds(1/animationSpeed);
        }

        Debug.Log("SO");
    }

    void SwapOffset()
    {
        Vector2 pos = HPBarList[currentAct-1].transform.localPosition;
        Debug.Log(pos.x + OffsetCurrentHPBar);
        pos = new Vector2(pos.x + OffsetCurrentHPBar, pos.y);
        HPBarList[currentAct-1].transform.localPosition = pos;

        pos = HPBarList[currentAct].transform.localPosition;
        pos = new Vector2(pos.x - OffsetCurrentHPBar, pos.y);
        HPBarList[currentAct].transform.localPosition = pos;
    }

    
    [Title("Test")]
    public float Damage = 30f;

    [Button ("DamageApply!")]
    public void DamageApply()
    {
        bossManager.bossStats.CurrentHP -= Damage;

        GlobalEventSystem.SendBossHPChanged();
        
    }
}
