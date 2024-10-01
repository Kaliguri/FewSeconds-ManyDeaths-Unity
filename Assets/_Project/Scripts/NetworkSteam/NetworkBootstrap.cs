using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkBootstrap : MonoBehaviour
{
    [SerializeField] private List<NetworkBehaviour> networkBehaviours = new();
    [SerializeField] private List<MonoBehaviour> monoBehaviours = new();

    private void Awake()
    {
        GlobalEventSystem.AllPlayersLoadedScene.AddListener(StartScripts);
    }

    private void StartScripts()
    {
        foreach (NetworkBehaviour networkBehaviour in networkBehaviours)
        {
            networkBehaviour.enabled = true;
        }

        foreach (MonoBehaviour monoBehaviour in monoBehaviours)
        {
            monoBehaviour.enabled = true;
        }
    }
}
