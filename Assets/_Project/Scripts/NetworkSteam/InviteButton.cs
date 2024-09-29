using Steamworks;
using Steamworks.Data;
using UnityEngine;


public class InviteFriendButton : MonoBehaviour 
{
    // Hooked up in Inspector 
    public void Invite()
    {
        if (Steamworks.SteamUtils.IsOverlayEnabled) { }
        else { Debug.Log("Disabled!"); }

        Lobby lobby = (Lobby)LobbySaver.instance.currentLobby;
        Steamworks.SteamFriends.OpenGameInviteOverlay(lobby.Id);
    }
}