using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "HeroTypeData", menuName = "FewSecondsManyDeaths/HeroData/HeroTypeData")]
public class HeroTypeData : ScriptableObject
{
    [Header("General")]
    public LocalizedString Name;
    public LocalizedString Description;

    [Header("Visual")]
    public Sprite Icon;
    public Color IconColor;

}
