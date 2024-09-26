using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnStage : GameState
{
    private float maxTurnTime;
    private TextMeshProUGUI timerText;
    private NetworkVariable<float> remainingTime = new NetworkVariable<float>(0f);
    private NetworkVariable<int> playersConfirmed = new NetworkVariable<int>(0);
    private TextMeshProUGUI confirmationText;
    private Button confirmationButton;
    private Coroutine timer;
    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private GUIDataBase dataBaseGUI => GameObject.FindObjectOfType<GUIDataBase>();
    private int playerCount => playerInfoData.PlayerCount;

    public void Initialize(CombatStageManager manager, float maxTurnTime, TextMeshProUGUI timerText, TextMeshProUGUI confirmationText, Button confirmationButton)
    {
        gameStateManager = manager;
        this.maxTurnTime = maxTurnTime;
        this.timerText = timerText;
        this.confirmationText = confirmationText;
        this.confirmationButton = confirmationButton;
        this.confirmationButton.onClick.AddListener(ConfirmEndTurn);
        playersConfirmed.OnValueChanged += UpdateConfirmationUI;

        UpdateConfirmationUI(0, playersConfirmed.Value);
    }

    private void UpdateConfirmationUI(int previousValue, int newValue)
    {
        if (confirmationText != null)
        {
            confirmationText.text = $"{newValue}/{playerCount} players confirmed";
        }
    }

    public override void Enter()
    {
        //Debug.Log("Enter Player Turn Stage");
        if (NetworkManager.Singleton.IsServer && gameStateManager.IsSpawned)
        {
            remainingTime.Value = maxTurnTime;
            playersConfirmed.Value = 0;
            timer = gameStateManager.StartCoroutine(Timer());
        }

        EnablePlayerTurnUI(true);
        GlobalEventSystem.SendPlayerTurnStageStarted();
    }

    public override void Exit()
    {
        GlobalEventSystem.SendPlayerTurnStageEnded();
        //Debug.Log("Exiting Player Turn Stage");

        EnablePlayerTurnUI(false);
    }

    public override void UpdateStage()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (playersConfirmed.Value >= playerCount)
            {
                EndTurn();
                gameStateManager.StopCoroutine(timer);
            }
        }
    }

    private IEnumerator Timer() //Use TimerManager!
    {
        UpdateTimerUIRpc();
        while (remainingTime.Value > 0)
        {
            yield return new WaitForSeconds(1f);

            remainingTime.Value -= 1f;

            UpdateTimerUIRpc();

            if (remainingTime.Value <= 0)
            {
                EndTurn();
                yield break;
            }
        }
    }

    public void ConfirmEndTurn()
    {
        ConfirmEndTurnRpc();

        dataBaseGUI.EndTurnButtonActiveChange(false);

        GlobalEventSystem.SendPlayerTurnEndConfirmed();
    }

    [Rpc(SendTo.Server)]
    private void ConfirmEndTurnRpc()
    {
        playersConfirmed.Value += 1;
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateTimerUIRpc()
    {
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = $"{remainingTime.Value}";
            GlobalEventSystem.SendPlayerTurnStageTimerUpdate(remainingTime.Value);

        }
    }

    private void EndTurn()
    {
        if (NetworkManager.Singleton.IsServer) gameStateManager.TransitionToNextStage();

        GlobalEventSystem.SendPlayerTurnEndConfirmed();
    }

    private void EnablePlayerTurnUI(bool enable)
    {
        dataBaseGUI.EndTurnButtonActiveChange(enable);
        dataBaseGUI.TimerActiveChange(enable);
        dataBaseGUI.SkillButtonListActiveChange(enable);
    }
}
