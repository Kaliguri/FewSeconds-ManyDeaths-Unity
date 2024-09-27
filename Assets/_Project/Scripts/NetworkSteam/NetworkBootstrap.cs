using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkBootstrap : MonoBehaviour
{
    [SerializeField] private List<NetworkBehaviour> networkBehaviours = new();
    [SerializeField] private List<MonoBehaviour> monoBehaviours = new();
    [SerializeField] private float TimeBeforeEnabled = 1f;

    void Start()
    {
        Invoke(nameof(StartScripts), TimeBeforeEnabled);
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
