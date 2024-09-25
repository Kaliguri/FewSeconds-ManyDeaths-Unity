using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public class AdventuresData : MonoBehaviour
{
    [Header("Current Session")]
    public int AdventureNumber;
    public int EncounterNumber;
    [SerializeField] List<Adventure> AdventureList = new(6);


    [Space]
    [Header("Settings")]
    [Header("Data Base")]
    public AvailableEntitiesData DB;
    [Header("General Settings")]
    public int AdventuresCount;

    [Header("Qusst Settings")]
    public int MinQuestInAdventure;
    public int MaxQuestInAdventure;

    [Header("Boss Encounter Settings")]
    public int CountBossToChoose;

    /*
    [Header("Rewards Settings")]
    public int MinItemsForEncounter;
    public int MaxItemsForEncounter;
    public int CommandArtifactsForEncounterCount;
    */

    private SessionSeed Seed => GetComponent<SessionSeed>();

    [Button("Generate Adventures From Seed")]
    public void GenerateAdventuresFromSeed()
    {

    }
    public void MoveToNextEncounter()
    {
        if (EncounterNumber > AdventureList[AdventureNumber].QuestList.Count + 1)
        {
            AdventureNumber += 1;
            EncounterNumber = 0;

            GoBoss();
        }

        else
        {
            EncounterNumber += 1;

            GoQuest();
        }
    }

    void GoQuest()
    {

    }

    void GoBoss()
    {

    }
}

[Serializable]
public class Adventure
{
    public List<QuestData> QuestList;
    public BossEncounterChoice _BossEncounterChoice; 
    //public RewardsForEncounter _RewardsForEncounter;
}
[Serializable]
public class BossEncounterChoice
{
    //List size = 3
    public List<BossEncounterData> BossEncounterChoiceList = new(3);
}

[Serializable]
public class RewardsForEncounter
{
    public List<RewardsData> RewardsList = new(3);
}
    
[Serializable]
public class RewardsData
{
    public List<TalentData> TalentList;
    public List<ItemData> ItemList;
    public List<CommandArtifactData> CommandArtifactList;
}

