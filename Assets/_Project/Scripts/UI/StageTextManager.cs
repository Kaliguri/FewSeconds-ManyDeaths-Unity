using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class StageTextManager : MonoBehaviour
{
    private LocalizeStringEvent localizeString => GetComponent<LocalizeStringEvent>();
    [SerializeField] List<LocalizedString> stageLocalizeName; 

    void Awake()
    {
        GlobalEventSystem.PredictionStageStarted.AddListener(SetTextPredictionStage);
        GlobalEventSystem.PlayerTurnStageStarted.AddListener(SetTextPlayerTurnStage);
        GlobalEventSystem.ResultStageStarted.AddListener(SetTextResultPlayerStage);
        GlobalEventSystem.BossTurnStageStarted.AddListener(SetTextBossTurnStage);
    }

    void SetTextPredictionStage() { localizeString.StringReference = stageLocalizeName[0]; }
    void SetTextPlayerTurnStage() { localizeString.StringReference = stageLocalizeName[1]; }
    void SetTextResultPlayerStage() { localizeString.StringReference = stageLocalizeName[2]; }
    void SetTextBossTurnStage() { localizeString.StringReference = stageLocalizeName[3]; }

}
