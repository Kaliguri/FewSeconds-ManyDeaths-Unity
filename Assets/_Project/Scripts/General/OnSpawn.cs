using UnityEngine;
using Unity.Netcode;
using System;
using TMPro;

public class OnSpawn : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    public static event Action<OnSpawn> LocalClientSpawned;
    public static event Action LocalClientDespawned;

    private MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    private PlayerInfoData playerInfoData => FindObjectOfType<PlayerInfoData>().GetComponent<PlayerInfoData>();
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();

    public override void OnNetworkSpawn()
    {

        base.OnNetworkSpawn();

        if (IsClient && IsOwner)
        {
            LocalClientSpawned?.Invoke(this);
        }
    }

    private void Start()
    {

        TransferData();

    }

    private void TransferData()
    {
        int id = (int)GetComponent<NetworkObject>().OwnerClientId;
        combatPlayerDataInStage.UpdatePlayersHeroes(gameObject, id);

        Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(transform.position);
        Vector2 tileCenterPos = mapClass.gameplayTilemap.GetCellCenterWorld(tile);
        Vector2 targetPoint = tileCenterPos - mapClass.tileZero;
        combatPlayerDataInStage.UpdatePlayersCoordinates(targetPoint, id);

        playerName.gameObject.SetActive(true);
        playerName.text = playerInfoData.NicknameList[id];
        
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
