using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public abstract class GameState : NetworkBehaviour
{
    protected CombatStageManager gameStateManager;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    public abstract void Enter();

    public abstract void Exit();

    public abstract void UpdateStage();
}
