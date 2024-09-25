using System;
using UnityEngine;


[Serializable]
public class TalentScript 
{
    public virtual void Cast()
    {
        Debug.Log("Cast Talent Effect!");
    }
}
