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
            // ���� ������ �������� �� �������� ������
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
            // ������, ����������� ������ �� �������
        }
        else
        {
            // ������, ����������� �� ��������
        }
        //��� ����, �� ���������
    }

    IEnumerator GiveTimePass()
    {
        yield return new WaitForSeconds(1f);
        gameStateManager.TransitionToNextStage();
    }
}