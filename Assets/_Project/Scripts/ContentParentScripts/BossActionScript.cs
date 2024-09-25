using System;
using UnityEngine;


[Serializable]
public class BossActionScript
{
    public virtual void Cast(int act)
    {
        Debug.Log("Cast BossAction!");

        GlobalEventSystem.SendBossActionEnd();
    }
}
