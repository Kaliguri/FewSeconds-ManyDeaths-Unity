using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


[CreateAssetMenu(fileName = "AvailableEntitiesData", menuName = "FewSecondsManyDeaths/_General/AvailableEntitiesData")]
public class AvailableEntitiesData : ScriptableObject
{
    [Title("Hero DataBase")]
    public List<HeroData> HeroDataList;
    
    [Title("Encounter DataBase")]
    public List<BossEncounterData> BossEncounterDataBaseList;
    public List<QuestData> QuestDataBaseList;

    [Title("Item DataBase")]
    public List<ItemData> ItemDataList;
    public List<CommandArtifactData> CommandArtifactDataList;

}
