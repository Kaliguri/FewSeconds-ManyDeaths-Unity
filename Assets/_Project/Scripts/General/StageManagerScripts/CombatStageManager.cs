using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using System;

public class CombatStageManager : NetworkBehaviour
{
    public float PlayerTurnTime;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private Button confirmationButton;
    
    private GameState currentStage;
    private int currentStageIndex;
    private NetworkVariable<int> currentStageIndexNet = new();

    private List<GameState> _stages;

    private void Awake()
    {
        GlobalEventSystem.StartCombat.AddListener(StartBattle);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }

    private void Initialize()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            currentStageIndexNet.Value = 0;
        }

        var predictionStage = gameObject.GetComponent<PredictionStage>();
        var playerTurnStage = gameObject.GetComponent<PlayerTurnStage>();
        var resultStage = gameObject.GetComponent<ResultStage>();
        var bossTurnStage = gameObject.GetComponent<BossTurnStage>();

        predictionStage.Initialize(this);
        playerTurnStage.Initialize(this, PlayerTurnTime, timerText, confirmationText, confirmationButton);
        resultStage.Initialize(this);
        bossTurnStage.Initialize(this);

        _stages = new List<GameState> { predictionStage, playerTurnStage, resultStage, bossTurnStage };

        //Invoke(nameof(StartBattle), 1f);
    }

    private void StartBattle()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NextStage(0);
        }
    }

    private void NextStage(int stageIndex)
    {
        if (currentStage != null)
        {
            currentStage.Exit();
        }

        currentStage = _stages[stageIndex];
        currentStageIndex = stageIndex;
        currentStage.Enter();
    }

    public void TransitionToNextStage()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            int nextStageIndex = (currentStageIndexNet.Value + 1) % _stages.Count;
            currentStageIndexNet.Value = nextStageIndex;
            NextStage(nextStageIndex);
        }
    }

    private void Update()
    {
        if (currentStageIndexNet.Value != currentStageIndex && !NetworkManager.Singleton.IsServer)
        {
            NextStage(currentStageIndexNet.Value);
        }

        if (currentStage != null)
        {
            currentStage.UpdateStage();
        }
    }
}
