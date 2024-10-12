using UnityEngine;
using Unity.Netcode;
using System;
using TMPro;

public class OnSpawn : NetworkBehaviour
{
    public static event Action<OnSpawn> LocalClientSpawned;
    public static event Action LocalClientDespawned;

    private MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private int ownerPlayerID => GetComponent<InHero>().ownerPlayerID.Value;

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
        GlobalEventSystem.AllPlayerSpawned.AddListener(TransferData);
    }

    private void TransferData()
    {
        combatPlayerDataInStage.UpdatePlayersHeroes(gameObject, ownerPlayerID);

        combatPlayerDataInStage.UpdateAliveStatus(true, ownerPlayerID);

        Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(transform.position);
        Vector2 tileCenterPos = mapClass.gameplayTilemap.GetCellCenterWorld(tile);
        Vector2 targetPoint = tileCenterPos - mapClass.tileZero;
        combatPlayerDataInStage.UpdatePlayersCoordinates(targetPoint, ownerPlayerID);

        GlobalEventSystem.SendPlayerDataUpdated();
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