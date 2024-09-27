using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ActivateHeroList : MonoBehaviour
{
    [Header("GameObj Ref")]
    [SerializeField] GameObject activeHeroList;
    [SerializeField] GameObject disableHeroList;

    [Header("Other")]
    [SerializeField] int UINumber;
    [SerializeField] bool isActive = false;

    private PlayerInfoData playerInfoData => FindObjectOfType<PlayerInfoData>();
    private int playerCount => playerInfoData.PlayerCount;

    private void Awake()
    {
        activeHeroList.GetComponent<HeroListController>().UINumber = UINumber;
        NetworkManager.Singleton.OnClientConnectedCallback += ActivateList;
        NetworkManager.Singleton.OnClientDisconnectCallback += DisableList;
        GlobalEventSystem.PlayerDataChanged.AddListener(ChangeList);
        if (NetworkManager.Singleton.IsServer) ChangeList();
    }

    private void ChangeList()
    {
        ActivateList(NetworkManager.Singleton.LocalClientId);
        DisableList(NetworkManager.Singleton.LocalClientId);
    }

    void ActivateList(ulong PlayerID)
    {
        if (!isActive && UINumber + 1 <= playerCount)
        {
            activeHeroList.SetActive(true);
            disableHeroList.SetActive(false);
            isActive = true;
        }
    }

    void DisableList(ulong PlayerID)
    {
        if (playerInfoData == null)
        {
            activeHeroList.SetActive(false);
            disableHeroList.SetActive(true);
            isActive = false;
            return;
        }
        if (isActive && UINumber + 1 > playerCount)
        {
            activeHeroList.SetActive(false);
            disableHeroList.SetActive(true);
            isActive = false;
        }
    }
}
