using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BossActionScript
{
    protected MapClass mapClass => GameObject.FindObjectOfType<MapClass>();
    protected BossManager bossManager => GameObject.FindObjectOfType<BossManager>();
    protected BossMultiplayerMethods bossMultiplayerMethods => GameObject.FindObjectOfType<BossMultiplayerMethods>();
    protected CombatPlayerDataInStage combatPlayerDataInStage => GameObject.FindObjectOfType<CombatPlayerDataInStage>();
    protected List<Vector2> TargetPoints = new();

    public virtual void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints);

        Debug.Log("Cast BossAction!");

        CastEnd();
    }

    protected void CastStart(List<Vector2> targetPoints)
    {
        TargetPoints = targetPoints;
    }

    protected void CastEnd()
    {
        GlobalEventSystem.SendBossActionEnd();
    }

    public virtual List<Vector2> GetCastPoint(int act)
    {
        return null;
    }

}
