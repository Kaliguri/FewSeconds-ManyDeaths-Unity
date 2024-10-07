using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkInstance : NetworkBehaviour
{
    public static NetworkInstance instance;
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();

    private void Start()
    {
        instance = this;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void ChangePlayerEnergyRpc(int newEnergy, int id)
    {
        combatPlayerDataInStage._TotalStatsList[id].currentCombat.CurrentEnergy = newEnergy;
    }
}
