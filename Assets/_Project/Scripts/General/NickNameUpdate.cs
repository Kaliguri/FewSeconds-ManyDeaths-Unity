using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NickNameUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    private PlayerInfoData playerInfoData => FindObjectOfType<PlayerInfoData>().GetComponent<PlayerInfoData>();
    private int playerID => playerInfoData.PlayerIDThisPlayer;

    void Start()
    {
        playerName.gameObject.SetActive(true);
        playerName.text = playerInfoData.NicknameList[playerID];
    }
}
