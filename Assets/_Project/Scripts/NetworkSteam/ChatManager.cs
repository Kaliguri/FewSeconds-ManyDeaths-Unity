using UnityEngine;
using TMPro;
using Steamworks;
using UnityEngine.EventSystems;
using Steamworks.Data;
using System;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField MessageInputField;
    [SerializeField] private TextMeshProUGUI MessageTemplate;
    [SerializeField] private GameObject MessageContaner;

    private void Start()
    {
        MessageTemplate.text = "";
    }

    private void OnEnable()
    {
        SteamMatchmaking.OnChatMessage += ChatSend;
        SteamMatchmaking.OnLobbyEntered += LobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined += LobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave += LobbyMemberLeaved;
    }

    private void OnDisable()
    {
        SteamMatchmaking.OnChatMessage -= ChatSend;
        SteamMatchmaking.OnLobbyEntered -= LobbyEntered;
        SteamMatchmaking.OnLobbyMemberJoined -= LobbyMemberJoined;
        SteamMatchmaking.OnLobbyMemberLeave -= LobbyMemberLeaved;
    }

    private void LobbyMemberLeaved(Lobby lobby, Friend friend) => AddMessageToBox(friend.Name + " покинул игру");

    private void LobbyMemberJoined(Lobby lobby, Friend friend) => AddMessageToBox(friend.Name + " присоеденился к игре");

    private void LobbyEntered(Lobby lobby) => AddMessageToBox("Ты присоеденился к игре");

    private void ChatSend(Lobby lobby, Friend friend, string msg)
    {
        AddMessageToBox(friend.Name + ": " + msg);
    }

    private void AddMessageToBox(string msg)
    {
        GameObject message = Instantiate(MessageTemplate.gameObject, MessageContaner.transform);
        message.GetComponent<TextMeshProUGUI>().text = msg;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ToggleChatBox();
        }
    }

    private void ToggleChatBox()
    {
        if (MessageInputField.gameObject.activeSelf)
        {
            if (!string.IsNullOrEmpty(MessageInputField.text))
            {
                LobbySaver.instance.currentLobby?.SendChatString(MessageInputField.text);
                MessageInputField.text = "";
            }

            MessageInputField.gameObject.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
            MessageInputField.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(MessageInputField.gameObject);
        }
    }
}
