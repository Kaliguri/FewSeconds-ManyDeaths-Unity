using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerTurnStage : GameState
{
    [SerializeField] float timeBetweeenNewBossCombo;

    private float maxTurnTime;
    private TextMeshProUGUI timerText;
    private NetworkVariable<float> remainingTime = new NetworkVariable<float>(0f);
    private NetworkVariable<int> playersConfirmed = new NetworkVariable<int>(0);
    private TextMeshProUGUI confirmationText;
    private Button confirmationButton;
    private Coroutine timer;
    private InputActions inputActions;
    private bool endingTurn = false;

    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();

    private GUIDataBase dataBaseGUI => GameObject.FindObjectOfType<GUIDataBase>();

    public void Initialize(CombatStageManager manager, float maxTurnTime, TextMeshProUGUI timerText, TextMeshProUGUI confirmationText, Button confirmationButton)
    {
        inputActions = new InputActions();
        inputActions.Combat.EndPlayerTurn.performed += _ => ConfirmEndTurn();
        gameStateManager = manager;
        this.maxTurnTime = maxTurnTime;
        this.timerText = timerText;
        this.confirmationText = confirmationText;
        this.confirmationButton = confirmationButton;
        this.confirmationButton.onClick.AddListener(ConfirmEndTurn);
        playersConfirmed.OnValueChanged += UpdateConfirmationUI;
        GlobalEventSystem.BossEndCombo.AddListener(StartNewBossComboAfterTime);
        dataBaseGUI.EndTurnButtonActiveChange(false);
        dataBaseGUI.TimerActiveChange(false);
    }

    public override void Enter()
    {
        endingTurn = false;
        //Debug.Log("Enter Player Turn Stage");
        if (NetworkManager.Singleton.IsServer && gameStateManager.IsSpawned)
        {
            remainingTime.Value = maxTurnTime;
            playersConfirmed.Value = 0;
            timer = gameStateManager.StartCoroutine(Timer());
        }
        inputActions.Enable();

        EnablePlayerTurnUI(true);
        UpdateConfirmationUI(0, playersConfirmed.Value);
        GlobalEventSystem.SendPlayerTurnStageStarted();
    }

    public override void Exit()
    {
        GlobalEventSystem.SendPlayerTurnStageEnded();
    }

    public override void UpdateStage()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (playersConfirmed.Value >= combatPlayerDataInStage.CountOfAlivePlayers())
            {
                StartEndingTurn();
                gameStateManager.StopCoroutine(timer);
            }
        }
    }

    private void StartNewBossComboAfterTime()
    {
        if (CombatStageManager.instance.currentStage is PlayerTurnStage && !endingTurn) Invoke(nameof(StartNewBossCombo), timeBetweeenNewBossCombo);
        else if (CombatStageManager.instance.currentStage is PlayerTurnStage) EndTurn();
    }

    private void StartNewBossCombo()
    {
        BossManager.instance.CastCombo();
    }

    private void UpdateConfirmationUI(int previousValue, int newValue)
    {
        if (confirmationText != null)
        {
            confirmationText.text = $"{newValue}/{combatPlayerDataInStage.CountOfAlivePlayers()} players confirmed";
        }
    }

    private IEnumerator Timer()
    {
        UpdateTimerUIRpc();
        while (remainingTime.Value > 0)
        {
            yield return new WaitForSeconds(1f);

            remainingTime.Value -= 1f;

            UpdateTimerUIRpc();

            if (remainingTime.Value <= 0)
            {
                StartEndingTurn();
                yield break;
            }
        }
    }

    public void ConfirmEndTurn()
    {
        inputActions.Disable();
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
        gameStateManager.TransitionToNextStage();

        EnablePlayerTurnUI(false);

        GlobalEventSystem.SendPlayerTurnEndConfirmed();
    }

    private void StartEndingTurn()
    {
        Debug.Log("StartEndingTurn");
        endingTurn = true;
        GlobalEventSystem.SendPlayerTurnEnding();
        inputActions.Disable();
    }

    private void EnablePlayerTurnUI(bool enable)
    {
        if (combatPlayerDataInStage.aliveStatus[playerInfoData.PlayerIDThisPlayer])
        {
            dataBaseGUI.EndTurnButtonActiveChange(enable);
            //dataBaseGUI.SkillButtonListActiveChange(enable);
        }
        dataBaseGUI.TimerActiveChange(enable);
    }
}
