using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatPlayerDataInStage : MonoBehaviour
{
    #region Fields


    [Title("Stats")]
    [Title("Total Stats")]
    [PropertySpace(SpaceAfter = 30)]
    public AllPlayerStats[] _TotalStatsList;



    [TabGroup("Hero Stats")]
    [Title("Hero Stats Effect On Stats")]
    public GeneralPlayerStats[] _HeroStatsEffectList;


    [TabGroup("Effects")]
    [Title("Effects On Stats")]
    public GeneralPlayerStats[] _StatsEffectList;
    [TabGroup("Effects")]
    [Title("Effects")]
    public EffectData[] CurrentEffectList;
    


    [TabGroup("Talents")]
    [Title("Talents Effect On Stats")]
    public GeneralPlayerStats[] _TalentsStatsEffectList;
    [TabGroup("Talents")]
    [Title("Talents")]
    public PlayerTalents[] PlayersTalentsList;



    [TabGroup("Items")]
    [Title("Items Effect On Stats")]
    public GeneralPlayerStats[] _ItemsStatsEffectList;
    [TabGroup("Items")]
    [Title("Items")]
    public PlayerItems[] PlayersItemsList;



    [TabGroup("Command Artifacts")]
    [Title("Artifacts Effect On Stats")]
    public GeneralPlayerStats[] _CommandArtifactsStatsEffectList;

    [TabGroup("Command Artifacts")]
    [Title("Artifacts")]
    public CommandArtifactData[] PlayersCommandArtifactsList;



    [PropertySpace(SpaceBefore = 30)]
    [Title("Coordinates & Hero Reference")]

    [Title("Hero Coordinates")]
    public Vector2[] HeroCoordinates;

    [Title("Heroes GameObj Ref")]
    public GameObject[] PlayersHeroes;


    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private CombatPlayerDataInSession sessionCombatData => GameObject.FindObjectOfType<CombatPlayerDataInSession>();
    private int playerCount => playerInfoData.PlayerCount;
    private List<HeroData> heroDataList => playerInfoData.HeroDataList;

    #endregion

    public GameObject[] SortedHeroesByHP() // Sorting from lowest health to highest
    {
        return null;
    }
    public GameObject[] SortedHeroesByDistance() // The distance is calculated from the Boss
    {
        return null;
    }

    private void Start()
    {
        Inizialize();
    }

    public void Inizialize()
    {
        InitialiseArrayes();
        DataTransferByCombatPlayerDataInSession();

        GlobalEventSystem.SendCombatPlayerDataInStageInitialized();
    }

    void InitialiseArrayes()
    {
        PlayersHeroes = new GameObject[playerCount];
        HeroCoordinates = new Vector2[playerCount];
    }

    void DataTransferByCombatPlayerDataInSession()
    {

        _TotalStatsList = sessionCombatData._TotalStatsList;

        _HeroStatsEffectList = sessionCombatData._HeroStatsEffectList;
        _TalentsStatsEffectList = sessionCombatData._TalentsStatsEffectList;
        _ItemsStatsEffectList = sessionCombatData._ItemsStatsEffectList;
        _CommandArtifactsStatsEffectList = sessionCombatData._CommandArtifactsStatsEffectList;
        _StatsEffectList = sessionCombatData._StatsEffectList;

        PlayersTalentsList = sessionCombatData.PlayersTalentsList;
        PlayersItemsList = sessionCombatData.PlayersItemsList;
        PlayersCommandArtifactsList = sessionCombatData.PlayersCommandArtifactsList;
        CurrentEffectList = sessionCombatData.CurrentEffectList;
        
    }

    public void UpdatePlayersCoordinates(Vector2 newCoordinates, int PlayerId)
    {
        HeroCoordinates[PlayerId] = newCoordinates;
    }

    public void UpdatePlayersHeroes(GameObject Hero, int PlayerId)
    {
        PlayersHeroes[PlayerId] = Hero;
    }
}




