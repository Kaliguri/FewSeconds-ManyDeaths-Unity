using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using Unity.Netcode;

public class InitialiseCombatPlayerData : NetworkBehaviour
{
    private CombatPlayerDataInSession combatPlayerData => GameObject.FindObjectOfType<CombatPlayerDataInSession>();

    public void Initialise()
    {
        InitialiseRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void InitialiseRpc()
    {
        combatPlayerData.Inizialize();
    }
}
