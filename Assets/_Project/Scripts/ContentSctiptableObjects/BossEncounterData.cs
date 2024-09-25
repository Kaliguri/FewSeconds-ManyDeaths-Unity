using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BossEncounterData", menuName = "FewSecondsManyDeaths/BossData/BossEncounterData")]
public class BossEncounterData : ScriptableObject
{
    [Header("General")]
    public string BossName;
    public string Rank;
    [TextArea] public string Description;

    [Header("Variations")]
    public List<BossVariationData> BossVariationsList;



}
