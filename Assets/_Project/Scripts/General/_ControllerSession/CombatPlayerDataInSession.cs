using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CombatPlayerDataInSession : MonoBehaviour
{
    #region Fields

    [Title("Stats")]
    [Title("Total Stats")]
    public AllStats[] _TotalStatsList;


    [TabGroup("Hero Stats")]
    [Title("Hero Stats Effect On Stats")]
    public GeneralStats[] _HeroStatsEffectList;

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
    public GeneralStats[] _StatsEffectList;
    [TabGroup("Effects")]
    [Title("Effects")]
    public EffectData[] CurrentEffectList;




    [TabGroup("Talents")]
    [Title("Talents Effect On Stats")]
    public GeneralStats[] _TalentsStatsEffectList;

    [TabGroup("Talents")]
    [Title("Talents")]
    public PlayerTalents[] PlayersTalentsList;




    [TabGroup("Items")]
    [Title("Items Effect On Stats")]
    public GeneralStats[] _ItemsStatsEffectList;

    [TabGroup("Items")]
    [Title("Items")]
    public PlayerItems[] PlayersItemsList;




    [TabGroup("Command Artifacts")]
    [Title("Artifacts Effect On Stats")]
    public GeneralStats[] _CommandArtifactsStatsEffectList;


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

        _TotalStatsList = new AllStats[maxPlayer];

        _HeroStatsEffectList = new GeneralStats[maxPlayer];
        _TalentsStatsEffectList = new GeneralStats[maxPlayer];
        _ItemsStatsEffectList = new GeneralStats[maxPlayer];
        _CommandArtifactsStatsEffectList = new GeneralStats[maxPlayer];
        _StatsEffectList = new GeneralStats[maxPlayer];

        PlayersTalentsList = new PlayerTalents[maxPlayer];
        PlayersItemsList = new PlayerItems[maxPlayer];
        PlayersCommandArtifactsList = new CommandArtifactData[maxPlayer];
        CurrentEffectList = new EffectData[maxPlayer];

        for (int DataNumber = 0; DataNumber < maxPlayer; DataNumber++ )
        {

        _TotalStatsList[DataNumber] = new AllStats();

        _HeroStatsEffectList[DataNumber] = new GeneralStats();
        _TalentsStatsEffectList[DataNumber] = new GeneralStats();
        _ItemsStatsEffectList[DataNumber] = new GeneralStats();
        _CommandArtifactsStatsEffectList[DataNumber] = new GeneralStats();
        _StatsEffectList[DataNumber] = new GeneralStats();

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

    public AllStats[] AddtionHeroStats(AllStats[] resultList, GeneralStats[] addedList)
    {
        for (int DataNumber = 0; DataNumber < maxPlayer; DataNumber++ )
        {

            resultList[DataNumber].general.MaxHP += addedList[DataNumber].MaxHP;
            resultList[DataNumber].general.StartHP += addedList[DataNumber].MaxHP;

            resultList[DataNumber].general.MaxEnergy += addedList[DataNumber].MaxEnergy;
            resultList[DataNumber].general.EnergyPerTurn += addedList[DataNumber].EnergyPerTurn;

            resultList[DataNumber].general.AbilityPower += addedList[DataNumber].AbilityPower;

            _TotalStatsList[DataNumber].currentCombat.CurrentHP = resultList[DataNumber].general.StartHP;
        }

        return resultList;
    }
}

[Serializable]
public class GeneralStats
{
    [Title("HP")]
    public float MaxHP = 0;
    public float StartHP = 0;

    [Title("Energy")]
    public int MaxEnergy;
    public int EnergyPerTurn;

    [Title("Skill")]
    public float AbilityPower;

    [Title("Level")]
    public int Level = 1;
    public float HPIncreaseFromLVLUP = 0.2f;
}

[Serializable]
public class CombatStats
{
    public float CurrentHP = 0;
    public float CurrentShield = 0;
    public int CurrentEnergy = 0;
}

[Serializable]
public class AllStats
{
    [Title("Combat (Current...)")]
    public CombatStats currentCombat = new CombatStats();

    [Title("General")]
    public GeneralStats general = new GeneralStats();
}

[Serializable]
public class PlayerTalents
{
     public List<ItemData> TalentsList;

    public static implicit operator PlayerTalents(GeneralStats v)
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