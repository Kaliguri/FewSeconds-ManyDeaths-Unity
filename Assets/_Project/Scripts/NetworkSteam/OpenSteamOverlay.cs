using System.Collections;
using System.Collections.Generic;
using Steamworks.Data;
using UnityEngine;

public class OpenSteamOverlay : MonoBehaviour
{
    public void OpenOverlay(string type)
    {
        if (Steamworks.SteamUtils.IsOverlayEnabled) { }
        else { Debug.Log("Disabled!"); }

        Steamworks.SteamFriends.OpenOverlay(type);
    }
}
