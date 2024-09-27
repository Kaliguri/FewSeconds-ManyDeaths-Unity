using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChangePlayersListNetwork : NetworkBehaviour
{
    PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>().GetComponent<PlayerInfoData>();
    private DBManager dBManager => FindObjectOfType<DBManager>().GetComponent<DBManager>();
    List<HeroData> heroDataList => dBManager.GetHeroDataList();
    int playerID => FindObjectOfType<PlayerInfoData>().PlayerIDThisPlayer;

    private void Awake()
    {
        GlobalEventSystem.PlayerColorChange.AddListener(ChangeColor);
        GlobalEventSystem.PlayerHeroChange.AddListener(ChangeHero);
        GlobalEventSystem.PlayerChoiceActionUpdate.AddListener(ChangeSkill);
    }

    private void ChangeSkill(int SkillNumber, int playerId, int variationSkillNumber)
    {
        ChangeSkillRpc(SkillNumber, playerId, variationSkillNumber);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ChangeSkillRpc(int SkillNumber, int ID, int VariationSkillNumber)
    {
        playerInfoData.SkillChoiceList[ID][SkillNumber] = VariationSkillNumber;
        GlobalEventSystem.SendSkillChanged();
    }

    private void ChangeHero()
    {
        HeroData heroData = playerInfoData.HeroDataList[playerID];
        int heroDataID = GetHeroDataID(heroData);
        ChangeHeroRpc(playerID, heroDataID);
    }

    private int GetHeroDataID(HeroData heroData) => heroDataList.IndexOf(heroData);

    [Rpc(SendTo.ClientsAndHost)]
    private void ChangeHeroRpc(int ID, int HeroDataID)
    {
        HeroData heroData = heroDataList[HeroDataID];
        if (heroData != null)
        {
            playerInfoData.HeroDataList[ID] = heroData;
            RestoreChoiceSkills();
            GlobalEventSystem.SendHeroChanged();
        }
    }

    void RestoreChoiceSkills()
    {
        for (int SkillNumber = 0; SkillNumber < playerInfoData.SkillChoiceList[playerID].Count; SkillNumber++)
        { 
            playerInfoData.SkillChoiceList[playerID][SkillNumber] = 0; 
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void ChangeColor()
    {
        ChangeColorRpc(playerInfoData.ColorList[playerID], playerID);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ChangeColorRpc(Color color, int ID)
    {
        playerInfoData.ColorList[ID] = color;
        GlobalEventSystem.SendPlayerDataChanged();
    }
}
