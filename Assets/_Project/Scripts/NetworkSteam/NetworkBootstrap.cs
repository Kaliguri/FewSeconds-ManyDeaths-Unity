using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkBootstrap : MonoBehaviour
{
    [SerializeField] private List<NetworkBehaviour> networkBehaviours = new();
    [SerializeField] private CombatPlayerDataInStage combatPlayerDataInStage;
    [SerializeField] private float TimeBeforeEnabled = 1f;

    void Start()
    {
        Invoke(nameof(StartScripts), TimeBeforeEnabled);
    }

    private void StartScripts()
    {
        for (int i = 0; i < networkBehaviours.Count; i++)
        {
            networkBehaviours[i].enabled = true;
        }
        combatPlayerDataInStage.enabled = true;
    }
}
