using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "HeroData", menuName = "FewSecondsManyDeaths/HeroData/HeroData")]
public class HeroData : ScriptableObject
{
    [Header("General")]
    public LocalizedString Name;
    public LocalizedString Description;
    public LocalizedString Quoute;
    public HeroTypeData HeroTypeData;

    [Header("Visual")]
    public GameObject GameObjectSpritePrefab;
    public Sprite HeroIcon;

    [Header("Gameplay")]
    public float MaxHPModif = 0f;

    [Header("Skill")]
    public List<SkillVariations> SkillList;

    [Header("Talent")]
    public List<TalentData> TalentList;

}

[Serializable]
public class SkillVariations
{
    public List<SkillData> SkillVariationsList;
}
