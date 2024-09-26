using UnityEngine;
using Unity.Netcode;
using System;
using TMPro;

public class OnSpawn :  MonoBehaviour
{
    private MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private PlayerInfoData playerInfoData => FindObjectOfType<PlayerInfoData>().GetComponent<PlayerInfoData>();
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private int playerID => playerInfoData.PlayerIDThisPlayer;

    private void Awake()
    {
        Debug.Log("i was born!");
        GlobalEventSystem.AllPlayerSpawned.AddListener(TransferData);
    }

    private void TransferData()
    {
        Debug.Log("TransferData");
        combatPlayerDataInStage.UpdatePlayersHeroes(gameObject, playerID);

        Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(transform.position);
        Vector2 tileCenterPos = mapClass.gameplayTilemap.GetCellCenterWorld(tile);
        Vector2 targetPoint = tileCenterPos - mapClass.tileZero;
        combatPlayerDataInStage.UpdatePlayersCoordinates(targetPoint, playerID);        
    }
}
