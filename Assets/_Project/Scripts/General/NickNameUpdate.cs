using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NickNameUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    private PlayerInfoData playerInfoData => FindObjectOfType<PlayerInfoData>().GetComponent<PlayerInfoData>();
    private int playerID => GetComponent<InHero>().ownerPlayerID.Value;

    void Awake()
    {
        GlobalEventSystem.AllPlayerSpawned.AddListener(UpdatePlayerNickname);
    }

    private void UpdatePlayerNickname()
    {
        Debug.Log("UpdatePlayerNickname");
        playerName.gameObject.SetActive(true);
        playerName.text = playerInfoData.NicknameList[playerID];
    }
}
