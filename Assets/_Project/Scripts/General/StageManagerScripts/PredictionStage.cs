using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class PredictionStage : GameState
{
    [SerializeField] float timeBeforeBossCombo = 0.5f;

    private BossManager bossManager => GameObject.FindObjectOfType<BossManager>();
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private int localId;

    public void Initialize(CombatStageManager manager)
    {
        gameStateManager = manager;
        GlobalEventSystem.BossEndCombo.AddListener(EndTurn);

        localId = playerInfoData.PlayerIDThisPlayer;
    }

    public override void Enter()
    {
        if (NetworkManager.Singleton.IsServer && gameStateManager.IsSpawned)
        {
            Invoke(nameof(ChoiceNewBossCombo), timeBeforeBossCombo);
        }

        GlobalEventSystem.SendPredictionStageStarted();

        Debug.Log("Entering Prediction Stage");
        RestoreEnergy();


        ShieldsRemove();

        Invoke(nameof(StartNewBossCombo), timeBeforeBossCombo * 2f);
    }

    private void ChoiceNewBossCombo()
    {
        bossManager.ChoiceCombo();
    }

    private void StartNewBossCombo()
    {
        BossManager.instance.CastCombo();
    }

    public override void Exit()
    {
        GlobalEventSystem.SendPredictionStageEnded();
    }

    public override void UpdateStage()
    {
        if (NetworkManager.Singleton.IsServer)
        {

        }
        else
        {

        }
    }

    private void RestoreEnergy()
    {
        for (int i = 0; i < playerInfoData.PlayerCount; i++)
        {
            if (combatPlayerDataInStage._TotalStatsList[i] != null)
            {
                int newEnergy = combatPlayerDataInStage._TotalStatsList[i].currentCombat.CurrentEnergy + combatPlayerDataInStage._TotalStatsList[i].general.EnergyPerTurn;
                if (newEnergy > combatPlayerDataInStage._TotalStatsList[i].general.MaxEnergy) ChangePlayerEnergyRpc(combatPlayerDataInStage._TotalStatsList[i].general.MaxEnergy, localId);
                else ChangePlayerEnergyRpc(newEnergy, i);
            }
        }
        GlobalEventSystem.SendEnergyChange();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ChangePlayerEnergyRpc(int newEnergy, int id)
    {
        combatPlayerDataInStage._TotalStatsList[id].currentCombat.CurrentEnergy = newEnergy;
    }

    private void EndTurn()
    {
        if (CombatStageManager.instance.currentStage is PredictionStage) gameStateManager.TransitionToNextStage();
    }

    private void ShieldsRemove()
    {
        bossManager.bossStats.CurrentShield = 0;
        List<AllPlayerStats> playerStats = combatPlayerDataInStage._TotalStatsList.ToList();
        for (int i = 0; i < playerStats.Count; i++) playerStats[i].currentCombat.CurrentShield = 0;
        GlobalEventSystem.SendPlayerShieldChanged();
    }
}
