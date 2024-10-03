using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class BossTurnStage : GameState
{
    private BossManager bossManager => GameObject.FindObjectOfType<BossManager>();

    public void Initialize(CombatStageManager manager)
    {
        gameStateManager = manager;
        GlobalEventSystem.BossEndCombo.AddListener(EndTurn);
    }

    public override void Enter()
    {
        if (NetworkManager.Singleton.IsServer && gameStateManager.IsSpawned)
        {
            
        }

        GlobalEventSystem.SendBossTurnStageStarted();

        bossManager.CastCombo();
    }


    public override void Exit()
    {
        GlobalEventSystem.SendBossTurnStageEnded();
        //Debug.Log("Exiting Boss Turn Stage");
    }

    public override void UpdateStage()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            // Логика, выполняемая только на сервере
        }
        else
        {
            // Логика, выполняемая на клиентах
        }
        //бей босс, да посильнее
    }

    private void EndTurn()
    {
        gameStateManager.TransitionToNextStage();
    }

}