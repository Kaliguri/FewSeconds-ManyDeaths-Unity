using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "FewSecondsManyDeaths/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("General")]
    public string Name;
    [TextArea] public string Description;

    [Header("Visual")]
    public Sprite Icon;

    [Header ("Script")]
    [SerializeReference, SubclassSelector]
    public ItemEffectScript ItemScript = new ItemEffectScript();

    [Header("Optional")]

    [Header("Active Item")]
    public bool IsActiveItem;
    public int UseCount;


    /*
    [Header("Effect")]
    public bool HasEffect;
    public EffectData EffectData;
    */

}
