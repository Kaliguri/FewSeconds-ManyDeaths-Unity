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

    public override void OnNetworkSpawn()
    {
        SceneLoaded();
    }

    private void SceneLoaded()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            int i = 0;
            foreach (ulong id in NetworkManager.Singleton.ConnectedClientsIds)
            {
                SpawnPlayer(i, id);
                i++;
            }
        }
    }

    private void SpawnPlayer(int i, ulong id)
    {
        Vector2 Coordinates = SpawnCoordinate[i] + zeroPoint;
        GameObject player = Instantiate(Player, Coordinates, Quaternion.identity);

        player.name = "Player_" + id;
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(id, true);
        //player.GetComponent<CoordinatesController>().ChangeCoordinates(SpawnCoordinate[i]);

        GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>().ChangeCell(SpawnCoordinate[i], MapClass.TileStates.Player);

        OnlyForClientRpc(SpawnCoordinate[i], id);
    }

    [ClientRpc]
    void OnlyForClientRpc(Vector2 PlayerMapCoordinates, ulong sourceNetworkObjectId)
    {
        GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>().ChangeCell(PlayerMapCoordinates, MapClass.TileStates.Player);
        GlobalEventSystem.SendPlayerSpawned();
    }
}
