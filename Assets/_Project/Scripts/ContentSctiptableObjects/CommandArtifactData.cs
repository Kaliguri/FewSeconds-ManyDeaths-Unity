using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CommandArtifactData", menuName = "FewSecondsManyDeaths/CommandArtifactData")]
public class CommandArtifactData : ScriptableObject
{
    [Header("General")]
    public string Name;
    [TextArea] public string Description;

    [Header("Visual")]
    public Sprite Icon;

    [Header ("Script")]
    [SerializeReference, SubclassSelector]
    public CommandArtifactEffectScript ItemScript = new CommandArtifactEffectScript();

}

