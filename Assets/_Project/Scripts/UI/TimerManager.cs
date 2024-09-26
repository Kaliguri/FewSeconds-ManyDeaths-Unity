using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [Title("Gameobjects Reference")]
    [SerializeField] TimerDoubleSlider doubleSlider;
    [SerializeField] TextMeshProUGUI timerText;

    private float turnTime => FindObjectOfType<CombatStageManager>().PlayerTurnTime;

    void Start()
    {
        doubleSlider.ValueTransfer(turnTime);
    }
}
