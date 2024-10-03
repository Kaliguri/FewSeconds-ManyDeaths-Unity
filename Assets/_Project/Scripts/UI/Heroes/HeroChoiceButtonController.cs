using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeroChoiceButtonController : MonoBehaviour
{
    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private int playerID => playerInfoData.PlayerIDThisPlayer;
    [SerializeField] Image icon;
    public HeroData HeroData;
    public bool IsTransfer = false;

    void Start()
    {
        if (!IsTransfer)
        gameObject.SetActive(false);
    }

    public void DataTranfer(HeroData data)
    {
        gameObject.SetActive(true);
        HeroData = data;
        icon.sprite = HeroData.HeroIcon;
        IsTransfer = true;
    } 
    public void HeroChoice()
    {
        if (HeroData != null)
        {
            playerInfoData.HeroDataList[playerID] = HeroData;
            RestoreChoiceSkills();
            GlobalEventSystem.SendHeroChanged();
            GlobalEventSystem.SendPlayerHeroChange();
        }
    }

    void RestoreChoiceSkills()
    {
        for (int SkillNumber = 0; SkillNumber < playerInfoData.SkillChoiceList[playerID].variationList.Count; SkillNumber++) 
        { playerInfoData.SkillChoiceList[playerID].variationList[SkillNumber] = 0; }

    }
}
