using System;
using UnityEngine;


[Serializable]
public class ItemEffectScript
{
    public virtual void Cast()
    {
        Debug.Log("Cast ItemEffect!");
    }
}
