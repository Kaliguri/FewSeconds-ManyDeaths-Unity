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

    [TabGroup("Turn Order")]
    [Title("Turn Order")]
    public List<int> TurnPriority;



    [PropertySpace(SpaceBefore = 30)]
    [Title("Coordinates & Hero Reference")]

    [Title("Hero Coordinates")]
    public Vector2[] HeroCoordinates;

    [Title("Heroes GameObj Ref")]
    public GameObject[] PlayersHeroes;

    [Title("Alive Status")]
    public bool[] aliveStatus;

    private CombatPlayerDataInSession sessionCombatData => GameObject.FindObjectOfType<CombatPlayerDataInSession>();
    private int playerCount => PlayerInfoData.instance.PlayerCount;

    #endregion

    public GameObject[] SortedHeroesByHP() // Sorting from lowest health to highest
    {
        return null;
    }
    public GameObject[] SortedHeroesByDistance() // The distance is calculated from the Boss
    {
        return null;
    }

    public static CombatPlayerDataInStage instance = null;

    private void Awake()
    {
        if (instance == null) {instance = this;}
        
        GlobalEventSystem.PlayerHPChanged.AddListener(CheckIfAlive);
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
        aliveStatus = new bool[playerCount];
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

    public void UpdateAliveStatus(bool status, int PlayerId)
    {
        aliveStatus[PlayerId] = status;
    }

    private void CheckIfAlive()
    {
        for (int i = 0; i < _TotalStatsList.Length; i++)
        {
            if (_TotalStatsList[i].currentCombat.CurrentHP == 0)
            {
                aliveStatus[i] = false;
                GlobalEventSystem.SendPlayerDied(i);
            }
        }
        if (IsEveryOneDead()) GlobalEventSystem.SendAllPlayersDied();
    }

    private bool IsEveryOneDead()
    {
        foreach (bool status in aliveStatus) if (status) return false;
        return true;
    }

    public int CountOfAlivePlayers()
    {
        int count = 0;
        foreach (bool status in aliveStatus) if (status) count++;
        return count;
    }
}