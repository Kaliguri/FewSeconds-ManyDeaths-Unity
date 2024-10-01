using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Steamworks;
using Steamworks.Data;
using Sirenix.OdinInspector;
using System;

public class PlayerInfoData : MonoBehaviour
{
    [TabGroup("Multiplayer")]
    public int PlayerCount = 0;
    [TabGroup("Multiplayer")]
    public int PlayerIDThisPlayer = -1;

    [TabGroup("Player Colors & Nicknames")]
    public List<UnityEngine.Color> ColorList;
    [TabGroup("Player Colors & Nicknames")]
    public List<string> NicknameList = new();

    [TabGroup("Heroes")]
    public List<HeroData> HeroDataList;

    [TabGroup("Heroes")]
    public List<SkillChoiceContainer> SkillChoiceList = new()
    {
        new SkillChoiceContainer(),
        new SkillChoiceContainer(),
        new SkillChoiceContainer(),
        new SkillChoiceContainer()
    };

    [TabGroup("Turn Order")]
    public List<int> TurnPriority;

    private void Awake()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += UpdatePlayerNickList;
        NetworkManager.Singleton.OnClientDisconnectCallback += UpdatePlayerNickList;
        GlobalEventSystem.PlayerLobbyUpdate.AddListener(UpdatePlayerNickList);
    }

    private void Start()
    {
        InitializeNetwork();
    }

    private void InitializeNetwork()
    {
        GlobalEventSystem.SendPlayerInfoDatanInitialized();
    }

    public void UpdatePlayerNickList(ulong id)
    {
        if (LobbySaver.instance.currentLobby != null)
        {
            Lobby lobby = (Lobby)LobbySaver.instance.currentLobby;
            IEnumerable<Friend> friends = lobby.Members;
            int i = 0;
            NicknameList = new List<string> { "", "", "", "" };
            foreach (Friend friend in friends)
            {
                NicknameList[i] = friend.Name;
                i++;
            }
            PlayerCount = i;
            if (PlayerIDThisPlayer == -1) PlayerIDThisPlayer = i - 1;
            GlobalEventSystem.SendPlayerDataChanged();
        }

        SteamFriends.SetRichPresence("steam_player_group", "Fun");
        SteamFriends.SetRichPresence("steam_player_group_size", PlayerCount.ToString());
    }

    /*void InizializeSkillChoiceList()
    {
        List<int> TempList = new List<int>() {0,0,0,0};

        for (int i = 0; i < 4; i++)
        {SkillChoiceList.Add(TempList);}
    }
    */
}

[Serializable]
public class SkillChoiceContainer
{
    public List<int> variationList = new() {0,0,0,0};
}
