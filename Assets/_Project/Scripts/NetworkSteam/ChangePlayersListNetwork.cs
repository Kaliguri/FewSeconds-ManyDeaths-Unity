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

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void Awake()
    {
        GlobalEventSystem.PlayerColorChange.AddListener(ChangeColor);
        GlobalEventSystem.PlayerHeroChange.AddListener(ChangeHero);
        GlobalEventSystem.PlayerChoiceActionUpdate.AddListener(ChangeSkill);
        GlobalEventSystem.PlayerInfoDataInitialized.AddListener(UpdateClientData);
    }

    private void UpdateClientData()
    {
        Debug.Log("UpdateClientData");
        SendDataFromHostToNewClientRpc();
    }

    [Rpc(SendTo.Server)]
    private void SendDataFromHostToNewClientRpc()
    {
        Debug.Log("SendDataFromHostToNewClientRpc");
        for (int ID = 0; ID < playerInfoData.PlayerCount; ID++)
        {
            ChangeColorRpc(playerInfoData.ColorList[ID], ID);
            ChangeHeroRpc(ID, GetHeroDataID(playerInfoData.HeroDataList[ID]));
            for (int SkillNumber = 0; SkillNumber < playerInfoData.HeroDataList[ID].SkillList.Count; SkillNumber++)
            {
                ChangeSkill(SkillNumber, ID, playerInfoData.SkillChoiceList[ID].variationList[SkillNumber]);
            }
        }
    }

    private void ChangeSkill(int SkillNumber, int ID, int variationSkillNumber)
    {
        ChangeSkillRpc(SkillNumber, ID, variationSkillNumber);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ChangeSkillRpc(int SkillNumber, int ID, int variationSkillNumber)
    {
        //Debug.Log("SkillNumber = " + SkillNumber + "; playerId = " + ID + "; variationSkillNumber = " + variationSkillNumber);
        playerInfoData.SkillChoiceList[ID].variationList[SkillNumber] = variationSkillNumber;
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
        for (int SkillNumber = 0; SkillNumber < playerInfoData.SkillChoiceList[playerID].variationList.Count; SkillNumber++)
        { 
            playerInfoData.SkillChoiceList[playerID].variationList[SkillNumber] = 0; 
        }
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
