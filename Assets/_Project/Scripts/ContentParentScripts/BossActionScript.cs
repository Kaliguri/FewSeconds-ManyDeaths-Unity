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

    protected HeroCombatObject GetHeroCombatObject(int playerID)
    {
        return new HeroCombatObject(playerID, combatPlayerDataInStage);
    }

    protected List<CombatObject> GetAffectedCombatObjectList(List<Vector2> area)
    {
        List<MapObject> MapObjectList = GetAffectedMapObjectList(area);
        List<CombatObject> combatObjectList = new List<CombatObject>();

        foreach (MapObject mapObject in MapObjectList)
        {
            if (mapObject is Hero)
            {
                combatObjectList.Add(new HeroCombatObject(mapObject.ID, combatPlayerDataInStage));
            }
            if (mapObject is Boss)
            {
                combatObjectList.Add(new BossCombatObject(bossManager));
            }
        }

        return combatObjectList;
    }

    protected List<MapObject> GetAffectedMapObjectList(List<Vector2> area)
    {
        return mapClass.MapObjectCheck(area);
    }
}
