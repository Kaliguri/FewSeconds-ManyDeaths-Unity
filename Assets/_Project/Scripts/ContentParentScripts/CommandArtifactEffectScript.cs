using System;
using UnityEngine;


[Serializable]
public class CommandArtifactEffectScript
{
    public virtual void Cast()
    {
        Debug.Log("Cast CommandArtifactEffect!");
    }
}
