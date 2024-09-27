using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

public class SkillChoiceButtonController : MonoBehaviour
{
    [Header("GameObj Reference")]
    public Image IconObj;
    public LocalizeStringEvent NameObj;
    public LocalizeStringEvent DescriptionObj;

    [Header("Other")]
    public int SkillNumber;
    public int VariationSkillNumber;

    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private int playerID => playerInfoData.PlayerIDThisPlayer;
    
    public void SkillChoice()
    {
        //Debug.Log(playerInfoData.SkillChoiceList.Count);
        //Debug.Log(playerInfoData.SkillChoiceList[playerID].Count);

        playerInfoData.SkillChoiceList[playerID][SkillNumber] = VariationSkillNumber;
        GlobalEventSystem.SendSkillChanged();
        GlobalEventSystem.SendPlayerChoiceActionUpdate(SkillNumber, playerID, VariationSkillNumber);
    }

}
