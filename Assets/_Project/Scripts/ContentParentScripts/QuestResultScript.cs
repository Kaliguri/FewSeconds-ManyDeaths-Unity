using System;
using UnityEngine;

[Serializable]
public class QuestResultScript 
{
    public virtual void Cast(int ResultNumber)
    {
        Debug.Log("Cast QuestResult!");
    }
}
