using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BerserkCircularSlice : BossActionScript
{
    [SerializeField] private int damage = 30;
    [SerializeField] private GameObject affectedTile;
    [SerializeField] private float timeBetweenCastAndDamage = 1f;

    public override void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints, act);

        Debug.Log("Cast Berserk Circular Slice!");
        CastCircularSlice();
    }

    public override List<Vector2> GetCastPoint(int act)
    {
        return new List<Vector2>() { new Vector2 (0 , 0) };
    }

    private void CastCircularSlice()
    {
        int radius = 2;
        if (act > 1) radius = 3;

        List<Vector2> CircularList = GridAreaMethods.SquareAOE(bossManager.CurrentCoordinates, bossManager.CurrentCoordinates, radius, true);

        CastAreaForSkill(CircularList);
        
        MonoInstance.instance.StartCoroutine(DamagePlayers(GetAffectedCombatObjectList(CircularList)));
    }

    private void CastAreaForSkill(List<Vector2> circularList)
    {
        for (int i = 0; i < circularList.Count; i++)
        {
            Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(circularList[i] + mapClass.tileZero);
            if (mapClass.gameplayTilemap.HasTile(tile))
            {
                GameObject AffectedTile = MonoInstance.Instantiate(affectedTile, circularList[i] + mapClass.tileZero, Quaternion.identity);
                MonoInstance.Destroy(AffectedTile, timeBetweenCastAndDamage);
            }
        }
    }

    IEnumerator DamagePlayers(List<CombatObject> affectedCombatObjectList)
    {
        yield return new WaitForSeconds(timeBetweenCastAndDamage);
        
        BossCombatObject bossCombatObject = new BossCombatObject(bossManager);

        foreach (CombatObject combatObject in affectedCombatObjectList)
        {
            CombatMethods.ApplayDamage(damage, bossCombatObject, combatObject);
        }

        CastEnd();
    }
}
