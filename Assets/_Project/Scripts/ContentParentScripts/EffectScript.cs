using System;
using UnityEngine;


[Serializable]
public class EffectScript
{
    public virtual void Cast()
    {
        Debug.Log("Cast Effect!");
    }
}
