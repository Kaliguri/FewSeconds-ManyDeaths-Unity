using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "EffectData", menuName = "FewSecondsManyDeaths/EffectData")]
public class EffectData : ScriptableObject
{
    [Header("General")]
    public string Name;
    [TextArea] public string Description;

    [Header("Visual")]
    public Sprite Icon;

    [Header ("Script")]
    [SerializeReference, SubclassSelector]
    public EffectScript EffectScript = new EffectScript();

    [Header("Stacks Effect")]
    public bool IsStacks;
    public int MaxStacks; 

}
