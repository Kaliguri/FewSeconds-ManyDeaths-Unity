using System;
using UnityEngine;

[Serializable]
public class HeadsOrTails : QuestResultScript
{
    public override void Cast(int ResultNumber)
    {
        if (ResultNumber == 0)      {Debug.Log("Tails"); }
        else if (ResultNumber == 1) {Debug.Log("Heads");}
        else if (ResultNumber == 2) {Debug.Log("RUN!");}
    }
}
