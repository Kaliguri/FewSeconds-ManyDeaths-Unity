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
    protected int act;

    public virtual void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints, act);

        Debug.Log("Cast BossAction!");

        CastEnd();
    }

    protected void CastStart(List<Vector2> targetPoints, int _act)
    {
        TargetPoints = targetPoints;
        act = _act;
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

    protected void DamageEveryOneInTiles(List<Vector2> tiles, float damage)
    {
        List<CombatObject> affectedCombatObjectList = GetAffectedCombatObjectList(tiles);
        BossCombatObject bossCombatObject = new BossCombatObject(bossManager);
        foreach (CombatObject combatObject in affectedCombatObjectList) CombatMethods.ApplayDamage(damage, bossCombatObject, combatObject);
    }
}
