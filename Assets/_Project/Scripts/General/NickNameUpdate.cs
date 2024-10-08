using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class NickNameUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;

    private PlayerInfoData playerInfoData => FindObjectOfType<PlayerInfoData>().GetComponent<PlayerInfoData>();

    [Title("Cutscene")]
    [SerializeField] bool IsCutscene = false;    
    [EnableIf("IsCutscene")] [SerializeField] int CutsceneHeroID;

    void Awake()
    {
        GlobalEventSystem.AllPlayerSpawned.AddListener(UpdatePlayerNickname);
        GlobalEventSystem.PlayerDied.AddListener(AmIDied);
    }

    void Start()
    {
        if (IsCutscene)
        {
            UpdatePlayerNickname();
        }
    }

    private void UpdatePlayerNickname()
    {
        playerName.gameObject.SetActive(true);
        if (!IsCutscene)
        {
            var playerID = GetComponentInParent<InHero>().ownerPlayerID.Value;
            playerName.text = playerInfoData.NicknameList[playerID];
        }
        else
        {
            playerName.text = playerInfoData.NicknameList[CutsceneHeroID];
        }

    }

    private void AmIDied(int playerID)
    {
        if (playerID == GetComponentInParent<InHero>().ownerPlayerID.Value)
        {
            playerName.text = "";
        }
    }
}
