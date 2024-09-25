using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AvailableEntitiesData", menuName = "FewSecondsManyDeaths/_General/AvailableEntitiesData")]
public class AvailableEntitiesData : ScriptableObject
{
    [Header("Encounter DataBase")]
    public List<BossEncounterData> BossEncounterDataBaseList;
    public List<QuestData> QuestDataBaseList;

    [Header("Item DataBase")]
    public List<ItemData> ItemDataList;
    public List<CommandArtifactData> CommandArtifactDataList;

}
