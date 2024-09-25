using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PredictionStage : GameState
{
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private int localId;

    public void Initialize(CombatStageManager manager)
    {
        gameStateManager = manager;
        //GlobalEventSystem.PlayerSpawned.AddListener(RestoreEnergy);

        localId = (int)NetworkManager.Singleton.LocalClientId;
    }

    public override void Enter()
    {
        //Debug.Log("Entering Prediction Stage");
        RestoreEnergy();

        if (NetworkManager.Singleton.IsServer && gameStateManager.IsSpawned)
        {
            // ���� ������ �������� �� �������� ������
            StartCoroutine("GiveTimePass");
        }
    }

    public override void Exit()
    {
        //Debug.Log("Exiting Prediction Stage");
    }

    public override void UpdateStage()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            // ������, ����������� ������ �� �������
        }
        else
        {
            // ������, ����������� �� ��������
        }
        //���� ���������� �������� ������? � ����������� ��� ���� ���� �� �������� ���
    }

    private void RestoreEnergy()
    {
        for (int i = 0; i < playerInfoData.PlayerCount; i++)
        {
            if (combatPlayerDataInStage._TotalStatsList[i] != null)
            {
                int newEnergy = combatPlayerDataInStage._TotalStatsList[i].currentCombat.CurrentEnergy + combatPlayerDataInStage._TotalStatsList[i].general.EnergyPerTurn;
                if (newEnergy > combatPlayerDataInStage._TotalStatsList[i].general.MaxEnergy) ChangePlayerEnergyRpc(combatPlayerDataInStage._TotalStatsList[i].general.MaxEnergy, (int)combatPlayerDataInStage.PlayersHeroes[localId].GetComponent<NetworkObject>().OwnerClientId);
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

    IEnumerator GiveTimePass()
    {
        yield return new WaitForSeconds(1f);
        gameStateManager.TransitionToNextStage();
    }
}
