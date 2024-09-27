using UnityEngine;
using Unity.Netcode;
using System;
using TMPro;

public class OnSpawn : NetworkBehaviour
{
    public static event Action<OnSpawn> LocalClientSpawned;
    public static event Action LocalClientDespawned;

    private MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private PlayerInfoData playerInfoData => FindObjectOfType<PlayerInfoData>().GetComponent<PlayerInfoData>();
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private int ownerPlayerID => GetComponent<InHero>().ownerPlayerID;

    public override void OnNetworkSpawn()
    {

        base.OnNetworkSpawn();

        if (NetworkManager.Singleton.IsClient && IsOwner)
        {
            LocalClientSpawned?.Invoke(this);
        }
    }

    private void Awake()
    {
        Debug.Log("AddListener");
        GlobalEventSystem.AllPlayerSpawned.AddListener(TransferData);
    }

    private void TransferData()
    {
        combatPlayerDataInStage.UpdatePlayersHeroes(gameObject, ownerPlayerID);

        Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(transform.position);
        Vector2 tileCenterPos = mapClass.gameplayTilemap.GetCellCenterWorld(tile);
        Vector2 targetPoint = tileCenterPos - mapClass.tileZero;
        combatPlayerDataInStage.UpdatePlayersCoordinates(targetPoint, ownerPlayerID);
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (IsClient && IsOwner)
        {
            LocalClientDespawned?.Invoke();
        }
    }
}