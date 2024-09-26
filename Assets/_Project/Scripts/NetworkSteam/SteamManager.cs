using UnityEngine;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using Netcode.Transports.Facepunch;
using UnityEngine.SceneManagement;

public class SteamManager : MonoBehaviour
{   
    //[SerializeField] private TMP_InputField LobbyIDInputField;
    //[SerializeField] private TextMeshProUGUI LobbyID;

    [Header("UI")]
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject InLobbyMenu;

    [Header("Controller")]
    [SerializeField] private GameObject ControllersSession;

    private string FirstSessionScene => FindObjectOfType<SceneLoaderManager>().FirstSessionScene;
    private GameObject Controller;

    private SceneLoaderManager sceneManager => FindObjectOfType<SceneLoaderManager>();

    private void OnEnable()
    {
        SteamMatchmaking.OnLobbyCreated += LobbyCreated;
        SteamMatchmaking.OnLobbyEntered += LobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested += GameLobbyJoinRequested;
    }

    private void OnDisable()
    {
        SteamMatchmaking.OnLobbyCreated -= LobbyCreated;
        SteamMatchmaking.OnLobbyEntered -= LobbyEntered;
        SteamFriends.OnGameLobbyJoinRequested -= GameLobbyJoinRequested;
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

    private void LobbyCreated(Result result, Lobby lobby)
    {
        if (result == Result.OK)
        {
            lobby.SetPublic();
            lobby.SetJoinable(true);
            lobby.SetGameServer(lobby.Owner.Id);
            NetworkManager.Singleton.StartHost();
        }
    }

    public async void HostLobby()
    {
        Controller = Instantiate(ControllersSession);
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
            sceneManager.LoadScene(FirstSessionScene, true);
        }
    }

}
