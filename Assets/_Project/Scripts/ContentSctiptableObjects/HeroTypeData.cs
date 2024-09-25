using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroTypeData", menuName = "FewSecondsManyDeaths/HeroData/HeroTypeData")]
public class HeroTypeData : ScriptableObject
{
    [Header("General")]
    public string Name;
    [TextArea] public string Description;

    [Header("Visual")]
    public Sprite Icon;
    public Color IconColor;

}
