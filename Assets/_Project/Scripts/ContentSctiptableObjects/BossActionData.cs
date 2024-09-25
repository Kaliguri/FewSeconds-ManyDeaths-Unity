using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "BossActionData", menuName = "FewSecondsManyDeaths/BossData/BossActionData")]
public class BossActionData : ScriptableObject
{
    [Header("General")]
    public string Name;
    [TextArea] public string Description;

    [Header("Script")]
    [SerializeReference, SubclassSelector]
    public BossActionScript ActionScript = new BossActionScript();
    
}
