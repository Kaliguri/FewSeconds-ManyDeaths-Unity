using System;
using UnityEngine;

[Serializable]
public class BerserkDash : BossActionScript
{
    public override void Cast(int act)
    {
        Debug.Log("Cast Berserk Dash!");

        GlobalEventSystem.SendBossActionEnd();
    }
}
