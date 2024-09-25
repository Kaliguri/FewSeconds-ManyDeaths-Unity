using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChangePlayersListNetwork : NetworkBehaviour
{
    PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>().GetComponent<PlayerInfoData>();
    int playerID => (int)NetworkManager.Singleton.LocalClientId;

    private void Awake()
    {
        GlobalEventSystem.PlayerColorChange.AddListener(ChangeColor);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void ChangeColor()
    {
        Debug.Log("ChangeColor");
        ChangeColorRpc(playerInfoData.ColorList[playerID], playerID);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ChangeColorRpc(Color color, int ID)
    {
        Debug.Log("ChangeColorRpc");
        playerInfoData.ColorList[ID] = color;
        GlobalEventSystem.SendPlayerDataChanged();
    }
}
