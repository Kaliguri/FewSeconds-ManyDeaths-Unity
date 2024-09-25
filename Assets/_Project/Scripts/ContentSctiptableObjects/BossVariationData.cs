using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossVariationData", menuName = "FewSecondsManyDeaths/BossData/BossVariationData")]
public class BossVariationData : ScriptableObject
{
    [Header("General")]
    public string TraitName;
    [TextArea] public string Description;

    [Header("Combat")]
    public List<float> HPActList;
    public List<AttacksInAct> AttacksInActList;

    [Header("Visual")]
    public GameObject GameObjectSpritePrefab;

    [Header("Scene")]
    public string SceneName;

}

[Serializable]
public class AttacksInAct
{
    public List<BossComboData> ComboAttackList;
}
