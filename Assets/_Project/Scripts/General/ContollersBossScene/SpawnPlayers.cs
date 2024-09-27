using UnityEngine.Tilemaps;
using UnityEngine;
using System;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using static UnityEngine.Rendering.DebugUI;
using Steamworks;
using System.Runtime.ConstrainedExecution;
using TMPro;

public class SpawnPlayers : NetworkBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] Vector2[] SpawnCoordinate;
    private Vector2 zeroPoint => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>().tileZero;
    private int localID => GameObject.FindObjectOfType<PlayerInfoData>().PlayerIDThisPlayer;
    private float TimeBeforeSendAllPlayersSpawned = 1f;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void Start()
    {
        SpawnAllPlayers();
    }

    private void SpawnAllPlayers()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            int i = 0;
            foreach (ulong id in NetworkManager.Singleton.ConnectedClientsIds)
            {
                SpawnPlayer(i, id);
                i++;
            }
            Invoke(nameof(SendAllPlayersSpawnedRpc), TimeBeforeSendAllPlayersSpawned);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendAllPlayersSpawnedRpc()
    {
        GlobalEventSystem.SendAllPlayerSpawned();
    }

    private void SpawnPlayer(int i, ulong id)
    {
        Vector2 Coordinates = SpawnCoordinate[i] + zeroPoint;
        GameObject player = Instantiate(Player, Coordinates, Quaternion.identity);

        player.name = "Player_" + id;
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);

        SendPlayerSpawnedRpc(SpawnCoordinate[i], localID);
    }

    [Rpc(SendTo.ClientsAndHost)]
    void SendPlayerSpawnedRpc(Vector2 PlayerMapCoordinates, int playerID)
    {
        GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>().SetHero(PlayerMapCoordinates, playerID);
        GlobalEventSystem.SendPlayerSpawned();
    }
}
