using Unity.Netcode;
using UnityEngine;


public class ResultStage : GameState
{
    [SerializeField] private float TimeBeforeFirstPlayerMovement = 1f;
    [SerializeField] private float TimeBetweenPlayerMovement = 1f;

    private NetworkVariable<int> PlayerEndMovind = new();
    private NetworkVariable<int> PlayerEndResultTurn = new();
    private PlayerInfoData playerInfoData => FindObjectOfType<PlayerInfoData>();
    private int playerCount;
    private int _orderInTurnPriority;
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
        _orderInTurnPriority = orderInTurnPriority + 1;
        SendResultStageStartedAfterTime();
        ConfirmPlayerEndMovingRpc();
    }

    private void SendResultStageStartedAfterTime()
    {
        Invoke(nameof(SendStartResultStageForPlayer), TimeBetweenPlayerMovement);
    }

    private void SendStartResultStageForPlayer()
    {
        SendStartResultStageForPlayerRpc(_orderInTurnPriority);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendStartResultStageForPlayerRpc(int orderInTurnPriority)
    {
        GlobalEventSystem.SendStartResultStageForPlayer(orderInTurnPriority);
    }

    private void ConfirmPlayerEndResultTurning(int orderInTurnPriority)
    {
        SendSendAllPlayersEndMovingRpc(orderInTurnPriority + 1);
        ConfirmPlayerEndResultTurnRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void SendSendAllPlayersEndMovingRpc(int orderInTurnPriority)
    {
        GlobalEventSystem.SendStartCastPlayer(orderInTurnPriority);
    }

    public override void Enter()
    {
        //Debug.Log("Enter Result Stage");

        if (NetworkManager.Singleton.IsServer)
        {
            PlayerEndMovind.Value = 0;
            PlayerEndResultTurn.Value = 0;
        }

        GlobalEventSystem.SendResultStageStarted();
        Invoke(nameof(SendStartResultStageForFirstPlayer), TimeBeforeFirstPlayerMovement);
    }

    private void SendStartResultStageForFirstPlayer()
    {
        GlobalEventSystem.SendStartResultStageForPlayer(0);
    }

    public override void Exit()
    {
        GlobalEventSystem.SendResultStageEnded();
        //Debug.Log("Exiting Result Stage");
    }

    public override void UpdateStage()
    {
        if (PlayerEndMovind.Value >= playerCount && !EndMoving)
        {
            EndMoving = true;
            GlobalEventSystem.SendAllPlayersEndMoving();
            GlobalEventSystem.SendStartCastPlayer(0);
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