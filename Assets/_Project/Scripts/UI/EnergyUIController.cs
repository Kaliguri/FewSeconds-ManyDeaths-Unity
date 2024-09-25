using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class EnergyUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI EnergyText;
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    private int localId => FindObjectOfType<PlayerInfoData>().PlayerIDThisPlayer;

    void Awake()
    {
        GlobalEventSystem.EnergyChange.AddListener(ChangeEnergyText);
        GlobalEventSystem.PlayerInfoDataInitialized.AddListener(ChangeEnergyText);  
    }
    private void ChangeEnergyText()
    {
        EnergyText.text = combatPlayerDataInStage._TotalStatsList[localId].currentCombat.CurrentEnergy.ToString() + "/" + combatPlayerDataInStage._TotalStatsList[localId].general.MaxEnergy.ToString();
    }
}
