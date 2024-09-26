using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Netcode;
using UnityEngine;


public class ResultStage : GameState
{
    private NetworkVariable<int> PlayerEndMovind = new();
    private NetworkVariable<int> PlayerEndResultTurn = new();
    private PlayerInfoData playerInfoData => FindObjectOfType<PlayerInfoData>();
    private int playerCount;
    private bool EndMoving = false;

    public void Initialize(CombatStageManager manager)
    {
        gameStateManager = manager;
        playerCount = playerInfoData.PlayerCount;
        GlobalEventSystem.PlayerEndMoving.AddListener(ConfirmPlayerEndMoving);
        GlobalEventSystem.PlayerEndResultTurn.AddListener(ConfirmPlayerEndResultTurning);
    }

    private void ConfirmPlayerEndMoving(int orderInTurnPriority)
    {
        SendResultStageStartedRpc(orderInTurnPriority + 1);
        ConfirmPlayerEndMovingRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendResultStageStartedRpc(int orderInTurnPriority)
    {
        GlobalEventSystem.SendResultStageStarted(orderInTurnPriority);
    }

    private void ConfirmPlayerEndResultTurning(int orderInTurnPriority)
    {
        SendSendAllPlayersEndMovingRpc(orderInTurnPriority + 1);
        ConfirmPlayerEndResultTurnRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendSendAllPlayersEndMovingRpc(int orderInTurnPriority)
    {
        GlobalEventSystem.SendAllPlayersEndMoving(orderInTurnPriority);
    }

    public override void Enter()
    {
        //Debug.Log("Enter Result Stage");

        if (NetworkManager.Singleton.IsServer)
        {
            PlayerEndMovind.Value = 0;
            PlayerEndResultTurn.Value = 0;
        }

        GlobalEventSystem.SendResultStageStarted(0);
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Result Stage");
    }

    public override void UpdateStage()
    {
        if (PlayerEndMovind.Value >= playerCount && !EndMoving)
        {
            EndMoving = true;
            GlobalEventSystem.SendAllPlayersEndMoving(0);
        }

        if (NetworkManager.Singleton.IsServer)
        {
            if (PlayerEndResultTurn.Value >= playerCount)
            {
                EndTurn();
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void ConfirmPlayerEndMovingRpc()
    {
        PlayerEndMovind.Value += 1;
    }

    [Rpc(SendTo.Server)]
    private void ConfirmPlayerEndResultTurnRpc()
    {
        PlayerEndResultTurn.Value += 1;
    }

    private void EndTurn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            gameStateManager.TransitionToNextStage();
            PlayerEndMovind.Value = 0;
            PlayerEndResultTurn.Value = 0;
        }
        EndMoving = false;
    }
}