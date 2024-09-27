using UnityEngine.Events;
using UnityEngine;

public static class GlobalEventSystem
{
    public static UnityEvent HeroChanged = new();
    public static void SendHeroChanged() { HeroChanged.Invoke(); }

    public static UnityEvent PlayerTurnEndConfirmed = new();
    public static void SendPlayerTurnEndConfirmed() { PlayerTurnEndConfirmed.Invoke(); }

    public static UnityEvent PlayerInfoDataInitialized = new();
    public static void SendPlayerInfoDatanInitialized() { PlayerInfoDataInitialized.Invoke(); }

    public static UnityEvent CombatPlayerDataInStageInitialized = new();
    public static void SendCombatPlayerDataInStageInitialized() { CombatPlayerDataInStageInitialized.Invoke(); }

    public static UnityEvent BossManagerInitialized = new();
    public static void SendBossManagerInitialized() { BossManagerInitialized.Invoke(); }

    public static UnityEvent EnergyChange = new();
    public static void SendEnergyChange() { EnergyChange.Invoke(); }

    public static UnityEvent<int> PlayerEndResultTurn = new();
    public static void SendPlayerEndResultTurn(int orderInTurnPreority) { PlayerEndResultTurn.Invoke(orderInTurnPreority); }

    public static UnityEvent StartCombat = new();
    public static void SendStartCombat() { StartCombat.Invoke(); }

    public static UnityEvent<int> StartResultStageForPlayer = new();
    public static void SendStartResultStageForPlayer(int orderInTurnPreority) { StartResultStageForPlayer.Invoke(orderInTurnPreority); }

    public static UnityEvent<int> StartCastPlayer = new();
    public static void SendStartCastPlayer(int orderInTurnPreority) { StartCastPlayer.Invoke(orderInTurnPreority); }

    #region PlayerInfoUpdate

    public static UnityEvent PlayerDataChanged = new();
    public static void SendPlayerDataChanged() { PlayerDataChanged.Invoke(); }

    public static UnityEvent PlayerSpawned = new();
    public static void SendPlayerSpawned() { PlayerSpawned.Invoke(); }

    public static UnityEvent AllPlayerSpawned = new();
    public static void SendAllPlayerSpawned() { AllPlayerSpawned.Invoke(); }

    public static UnityEvent <ulong> PlayerLobbyUpdate = new();
    public static void SendPlayerLobbyUpdate(ulong id) { PlayerLobbyUpdate.Invoke(id); }

    public static UnityEvent PlayerHeroChange = new();
    public static void SendPlayerHeroChange() { PlayerHeroChange.Invoke(); }

    public static UnityEvent PlayerColorChange = new();
    public static void SendPlayerColorChange() {  PlayerColorChange.Invoke(); }

    #endregion

    #region Players Moving

    public static UnityEvent <int> PlayerEndMoving = new();
    public static void SendPlayerEndMoving(int orderInTurnPreority) { PlayerEndMoving.Invoke(orderInTurnPreority); }

    public static UnityEvent PathChanged = new();   
    public static void SendPathChanged() { PathChanged.Invoke(); }

    public static UnityEvent PlayerStartMove = new();
    public static void SendPlayerStartMove() { PlayerStartMove.Invoke(); }

    public static UnityEvent PlayerEndMove = new();
    public static void SendPlayerEndMove() { PlayerEndMove.Invoke(); }

    public static UnityEvent AllPlayersEndMoving = new();
    public static void SendAllPlayersEndMoving() { AllPlayersEndMoving.Invoke(); }

    #endregion

    #region Actions

    public static UnityEvent BossActionEnd = new();
    public static void SendBossActionEnd() { BossActionEnd.Invoke(); }

    public static UnityEvent PlayerActionEnd = new();
    public static void SendPlayerActionEnd() { PlayerActionEnd.Invoke(); }

    public static UnityEvent PlayerActionChoosed = new();
    public static void SendPlayerActionChoosed() { PlayerActionChoosed.Invoke(); }

    public static UnityEvent PlayerActionUnchoosed = new();
    public static void SendPlayerActionUnchoosed() {  PlayerActionUnchoosed.Invoke(); }

    public static UnityEvent PlayerActionAproved = new();
    public static void SendPlayerActionAproved() {  PlayerActionAproved.Invoke(); }

    public static UnityEvent SkillChanged = new();
    public static void SendSkillChanged() { SkillChanged.Invoke(); }

    public static UnityEvent PlayerActionUpdate = new();
    public static void SendPlayerActionUpdate() {  PlayerActionUpdate.Invoke(); }

    public static UnityEvent<int> PlayerChoiceActionUpdate = new();
    public static void SendPlayerChoiceActionUpdate(int id) { PlayerChoiceActionUpdate.Invoke(id); }

    #endregion

    #region Stage StartEnd Events

    public static UnityEvent PredictionStageStarted = new();
    public static void SendPredictionStageStarted() { PredictionStageStarted.Invoke(); }

    public static UnityEvent PredictionStageEnded = new();
    public static void SendPredictionStageEnded() { PredictionStageEnded.Invoke(); }


    public static UnityEvent PlayerTurnStageStarted = new();
    public static void SendPlayerTurnStageStarted() { PlayerTurnStageStarted.Invoke(); }

    public static UnityEvent PlayerTurnStageEnded = new();
    public static void SendPlayerTurnStageEnded() { PlayerTurnStageEnded.Invoke(); }


    public static UnityEvent ResultStageEnded = new();
    public static void SendResultStageEnded() { ResultStageEnded.Invoke(); }

    public static UnityEvent ResultStageStarted = new();
    public static void SendResultStageStarted() { ResultStageStarted.Invoke(); }


    public static UnityEvent BossTurnStageEnded = new();
    public static void SendBossTurnStageEnded() { BossTurnStageEnded.Invoke(); }

    public static UnityEvent BossTurnStageStarted = new();
    public static void SendBossTurnStageStarted() { BossTurnStageStarted.Invoke(); }

    public static UnityEvent <float> PlayerTurnStageTimerUpdate = new();
    public static void SendPlayerTurnStageTimerUpdate(float timeValue) { PlayerTurnStageTimerUpdate.Invoke(timeValue); }

    #endregion

    #region Combat

    public static UnityEvent PlayerHPChanged = new();
    public static void SendPlayerHPChanged() { PlayerHPChanged.Invoke(); }

    public static UnityEvent PlayerShieldChanged = new();
    public static void SendPlayerShieldChanged() { PlayerShieldChanged.Invoke(); }

    public static UnityEvent BossHPChanged = new();
    public static void SendBossHPChanged() { BossHPChanged.Invoke(); }

    public static UnityEvent BossActChanged = new();
    public static void SendBossActChanged() { BossActChanged.Invoke(); }

    #endregion
}