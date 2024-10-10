using UnityEngine;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using Netcode.Transports.Facepunch;
using UnityEngine.SceneManagement;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using TMPro;

public class SteamManager : MonoBehaviour
{   
    //[SerializeField] private TMP_InputField LobbyIDInputField;
    //[SerializeField] private TextMeshProUGUI LobbyID;

    [Header("UI")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject InLobbyMenu;

    [Header("Controller")]
    [SerializeField] private GameObject ControllersSession;

    [Header("WarningText")]
    [SerializeField] GameObject warningText;

    private string FirstSessionScene => FindObjectOfType<MainMenuSceneContainer>().FirstSessionScene;
    private GameObject Controller;

    private MainMenuSceneContainer sceneManager => FindObjectOfType<MainMenuSceneContainer>();
    private SceneLoaderManager sceneLoaderManager => FindObjectOfType<SceneLoaderManager>();

    private void OnEnable()
    {
        SteamMatchmaking.OnLobbyCreated += LobbyCreated;
        SteamMatchmaking.OnLobbyEntered += LobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested += GameLobbyJoinRequested;
        SteamMatchmaking.OnLobbyMemberLeave += OnLobbyMemberLeave;
    }

    private void OnDisable()
    {
        SteamMatchmaking.OnLobbyCreated -= LobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= LobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested -= GameLobbyJoinRequested;
        SteamMatchmaking.OnLobbyMemberLeave -= OnLobbyMemberLeave;
    }

    private async void GameLobbyJoinRequested(Lobby lobby, SteamId steamId)
    {
        await lobby.Join();
    }

    private void LobbyEntered(Lobby lobby)
    {
        LobbySaver.instance.currentLobby = lobby;
        //LobbyID.text = lobby.Id.ToString();
        ChangeUI();
        if (!NetworkManager.Singleton.IsHost)
        {
            Controller = Instantiate(ControllersSession);
            NetworkManager.Singleton.gameObject.GetComponent<FacepunchTransport>().targetSteamId = lobby.Owner.Id;
            NetworkManager.Singleton.StartClient();
        }
        GlobalEventSystem.SendPlayerLobbyUpdate(lobby.Owner.Id);
    }

    private void OnLobbyMemberLeave(Lobby lobby, Friend friend)
    {
        if (friend.Id == lobby.Owner.Id)
        {
            if (!NetworkManager.Singleton.IsHost)
            {
                LeaveLobby();
            }
        }
    }

    private void LobbyCreated(Result result, Lobby lobby)
    {
        if (result == Result.OK)
        {
            lobby.SetPublic();
            lobby.SetJoinable(true);
            lobby.SetGameServer(lobby.Owner.Id);
            Controller = Instantiate(ControllersSession);
            NetworkManager.Singleton.StartHost();
        }
    }

    public async void HostLobby()
    {
        await SteamMatchmaking.CreateLobbyAsync(4);
    }

    /*public async void JoinLobbyWithID()
    {
        if (!ulong.TryParse(LobbyIDInputField.text, out ulong ID))
            return;

        Lobby [] lobbies = await SteamMatchmaking.LobbyList.WithSlotsAvailable(1).RequestAsync();
        
        foreach (Lobby lobby in lobbies)
        {
            if (lobby.Id == ID)
            {
                await lobby.Join();
                return;
            }
        }
    }*/

    /*public void CopyID()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = LobbyID.text;
        textEditor.SelectAll();
        textEditor.Copy();
    }*/

    public void LeaveLobby()
    {
        LobbySaver.instance.currentLobby?.Leave();
        LobbySaver.instance.currentLobby = null;
        Destroy(Controller);
        ChangeUI();
        NetworkManager.Singleton.Shutdown();
    }

    private void ChangeUI()
    {
        if (LobbySaver.instance.currentLobby == null)
        {
            MainMenu.SetActive(true);
            InLobbyMenu.SetActive(false);
        }
        else
        {
            MainMenu.SetActive(false);
            InLobbyMenu.SetActive(true);
        }
    }

    public void StrartGameServer()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            List<HeroTypeData> heroesList = new();
            for (int i = 0; i < PlayerInfoData.instance.PlayerCount; i++) heroesList.Add(PlayerInfoData.instance.HeroDataList[i].HeroTypeData);
            if (!HasDuplicates(heroesList)) sceneLoaderManager.LoadScene(FirstSessionScene, true);
            else
            {
                warningText.SetActive(true);
            }
        }
    }

    private bool HasDuplicates(List<HeroTypeData> list)
    {
        HashSet<HeroTypeData> uniqueElements = new HashSet<HeroTypeData>();

        foreach (HeroTypeData item in list)
        {
            if (!uniqueElements.Add(item))
            {
                return true;
            }
        }
        return false;
    }
}
