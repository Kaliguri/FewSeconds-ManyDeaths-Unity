using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class BossTurnStage : GameState
{
    public void Initialize(CombatStageManager manager)
    {
        gameStateManager = manager;
    }

    public override void Enter()
    {
        if (NetworkManager.Singleton.IsServer && gameStateManager.IsSpawned)
        {
            // пока просто кидаемся на следущую стадию
            StartCoroutine(nameof(GiveTimePass));
        }
    }


    public override void Exit()
    {
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

    IEnumerator GiveTimePass()
    {
        yield return new WaitForSeconds(1f);
        gameStateManager.TransitionToNextStage();
    }
}