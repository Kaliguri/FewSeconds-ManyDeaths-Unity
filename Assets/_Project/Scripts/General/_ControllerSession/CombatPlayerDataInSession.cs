using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatPlayerDataInSession : MonoBehaviour
{
    #region Fields

    [Title("Stats")]
    [Title("Total Stats")]
    public AllPlayerStats[] _TotalStatsList;


    [TabGroup("Hero Stats")]
    [Title("Hero Stats Effect On Stats")]
    public GeneralPlayerStats[] _HeroStatsEffectList;

    [TabGroup("Hero Stats")]
    [Title("Settings")]
    public float StartMaxHP = 100f;
    [TabGroup("Hero Stats")]
    public float StartAbilityPower = 100f;
    [TabGroup("Hero Stats")]
    public int StartMaxEnergy = 8;
    [TabGroup("Hero Stats")]
    public int StartEnergyPerTurn = 6;



    
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


    public const int maxPlayer = 4; 


    private PlayerInfoData playerInfoData => GameObject.FindObjectOfType<PlayerInfoData>();
    private int playerCount => playerInfoData.PlayerCount;
    private List<HeroData> heroDataList => playerInfoData.HeroDataList;

    #endregion

    public void Inizialize()
    {
        InitialiseArrayes();

        HeroStatsDataTransferByPlayerInfoData();

        TotalStatsInizialize();
        TotatStatsCalcutation();
    }

    void InitialiseArrayes()
    {

        _TotalStatsList = new AllPlayerStats[maxPlayer];

        _HeroStatsEffectList = new GeneralPlayerStats[maxPlayer];
        _TalentsStatsEffectList = new GeneralPlayerStats[maxPlayer];
        _ItemsStatsEffectList = new GeneralPlayerStats[maxPlayer];
        _CommandArtifactsStatsEffectList = new GeneralPlayerStats[maxPlayer];
        _StatsEffectList = new GeneralPlayerStats[maxPlayer];

        PlayersTalentsList = new PlayerTalents[maxPlayer];
        PlayersItemsList = new PlayerItems[maxPlayer];
        PlayersCommandArtifactsList = new CommandArtifactData[maxPlayer];
        CurrentEffectList = new EffectData[maxPlayer];

        for (int DataNumber = 0; DataNumber < maxPlayer; DataNumber++ )
        {

            _TotalStatsList[DataNumber] = new AllPlayerStats();

            _HeroStatsEffectList[DataNumber] = new GeneralPlayerStats();
            _TalentsStatsEffectList[DataNumber] = new GeneralPlayerStats();
            _ItemsStatsEffectList[DataNumber] = new GeneralPlayerStats();
            _CommandArtifactsStatsEffectList[DataNumber] = new GeneralPlayerStats();
            _StatsEffectList[DataNumber] = new GeneralPlayerStats();

            PlayersTalentsList[DataNumber] = new PlayerTalents();
            PlayersItemsList[DataNumber] = new PlayerItems();
            PlayersCommandArtifactsList[DataNumber] = ScriptableObject.CreateInstance<CommandArtifactData>();
            CurrentEffectList[DataNumber] = ScriptableObject.CreateInstance<EffectData>();

        }
    }

    void HeroStatsDataTransferByPlayerInfoData()
    {   
        for (int DataNumber = 0; DataNumber < maxPlayer; DataNumber++ )
        {
            _HeroStatsEffectList[DataNumber].MaxHP = heroDataList[DataNumber].MaxHPModif;
            _HeroStatsEffectList[DataNumber].StartHP = heroDataList[DataNumber].MaxHPModif;
        }
    }
    void TotalStatsInizialize()
    {
        for (int DataNumber = 0; DataNumber < maxPlayer; DataNumber++ )
        {
            _TotalStatsList[DataNumber].general.MaxHP = StartMaxHP;
            _TotalStatsList[DataNumber].general.StartHP = StartMaxHP;

            _TotalStatsList[DataNumber].general.MaxEnergy = StartMaxEnergy;
            _TotalStatsList[DataNumber].general.EnergyPerTurn = StartEnergyPerTurn;

            _TotalStatsList[DataNumber].general.AbilityPower = StartAbilityPower;

            _TotalStatsList[DataNumber].currentCombat.CurrentHP = StartMaxHP;
            _TotalStatsList[DataNumber].currentCombat.CurrentEnergy = 0;
        }

    }
    void TotatStatsCalcutation()
    {
        TotalStatsInizialize();

        _TotalStatsList = AddtionHeroStats(_TotalStatsList, _HeroStatsEffectList);
        _TotalStatsList = AddtionHeroStats(_TotalStatsList, _TalentsStatsEffectList);
        _TotalStatsList = AddtionHeroStats(_TotalStatsList, _ItemsStatsEffectList);
        _TotalStatsList = AddtionHeroStats(_TotalStatsList, _CommandArtifactsStatsEffectList);
        _TotalStatsList = AddtionHeroStats(_TotalStatsList, _StatsEffectList);

    }

    public AllPlayerStats[] AddtionHeroStats(AllPlayerStats[] resultList, GeneralPlayerStats[] addedList)
    {
        for (int DataNumber = 0; DataNumber < maxPlayer; DataNumber++ )
        {

            resultList[DataNumber].general.MaxHP += addedList[DataNumber].MaxHP;
            resultList[DataNumber].general.StartHP += addedList[DataNumber].MaxHP;

            resultList[DataNumber].general.MaxEnergy += addedList[DataNumber].MaxEnergy;
            resultList[DataNumber].general.EnergyPerTurn += addedList[DataNumber].EnergyPerTurn;

            resultList[DataNumber].general.AbilityPower += addedList[DataNumber].AbilityPower;

            _TotalStatsList[DataNumber].currentCombat.MaxHP = resultList[DataNumber].general.MaxHP;
            _TotalStatsList[DataNumber].currentCombat.CurrentHP = resultList[DataNumber].general.StartHP;
        }

        return resultList;
    }
}

[Serializable]
public class PlayerTalents
{
     public List<ItemData> TalentsList;

    public static implicit operator PlayerTalents(GeneralPlayerStats v)
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public class PlayerItems
{
    public List<ItemData> ActiveItemList = new(2);
    public List<ItemData> PassiveItemList;
    
}