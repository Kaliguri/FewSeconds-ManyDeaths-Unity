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
    [SerializeField] float OffsetCurrentHPBar = 20f;
    //[ShowInInspector]
    //private float OffsetCurrentHPBar => HPBarList[0].transform.position.x - HPBarList[1].transform.position.x;
    


    private BossHPBarReference CurrentHPBarReference => HPBarList[currentAct].GetComponent<BossHPBarReference>();
    private Slider secondarySliderCurrentHPBar => CurrentHPBarReference.SecondarySlider;
    private Slider primarySliderCurrentHPBar => CurrentHPBarReference.PrimarySlider;

    private BossManager bossManager => FindObjectOfType<BossManager>();
    private int currentAct = 0;
    private float maxHP;
    private float currentHP;
    private Coroutine secondaryCoroutine;

    void Awake()
    {
        GlobalEventSystem.BossHPChanged.AddListener(HPBarUpdate);
        GlobalEventSystem.BossManagerInitialized.AddListener(NewActInizialize);
        GlobalEventSystem.BossActChanged.AddListener(ChangeAct);
    }

    void NewActInizialize()
    {
        //CurrentHPBarReference = HPBarList[currentAct].GetComponent<BossHPBarReference>();
        //SwapOffset(currentAct, OffsetCurrentHPBar);
        SetColor();
        StartHPBarUpdate();
    }

    void ChangeAct()
    {
        if (secondaryCoroutine != null) StopCoroutine(secondaryCoroutine);
        primarySliderCurrentHPBar.value = 0;
        secondarySliderCurrentHPBar.value = 0;

        currentAct ++;
        NewActInizialize();
        VisualBarCorrect();
        //SwapUpper();
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

        if (secondaryCoroutine != null) StopCoroutine(secondaryCoroutine);
        secondaryCoroutine = StartCoroutine(SecondarySlideAnimation(primarySliderCurrentHPBar, secondarySliderCurrentHPBar));
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

        secondary.value = Mathf.Lerp(secondary.value, currentHP, _lerpSpeed);
        //Debug.Log(primary.value+ ": " + secondary.value);
        yield return new WaitForSeconds(1/animationSpeed);
        }

        Debug.Log("SO");
    }

    void SwapOffset(int barNumber, float offset)
    {
        Vector2 pos = HPBarList[barNumber].transform.localPosition;
        pos = new Vector2(pos.x - offset, pos.y);
        HPBarList[barNumber].transform.localPosition = pos;
    }

    void SwapUpper()
    {
        for (int barNumber = HPBarList.Count-1; barNumber >= 0; barNumber--)
        {
            if (barNumber < currentAct)
            {
                HPBarList[barNumber].SetActive(false);
            }
            else
            {
                HPBarList[barNumber].transform.position = HPBarList[barNumber-1].transform.position;
            }
        }
    }

    void VisualBarCorrect()
    {
        HPBarList[currentAct].GetComponent<Canvas>().sortingOrder = HPBarList[currentAct-1].GetComponent<Canvas>().sortingOrder + 1;
        //SwapOffset(currentAct-1, -OffsetCurrentHPBar);
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
