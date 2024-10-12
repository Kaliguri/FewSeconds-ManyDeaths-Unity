using MoreMountains.Tools;
using Sirenix.OdinInspector;
using Sonity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BerserkCircularSlice : BossActionScript
{
    [Header("Stats")]
    [SerializeField] private int damage = 30;
    [SerializeField] private int radius = 2;
    [SerializeField] private int radiusUpgrade = 1;


    [Header("Visual")]
    [SerializeField] private float timeBetweenCastAndDamage = 1f;

    [Header("SFX")]
    [SerializeField] SoundEvent castSFX;
    [SerializeField] SoundEvent hitSFX;
    

    public override void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints, act);
        castSFX.Play(bossManager.transform);

        //Debug.Log("Cast Berserk Circular Slice!");
        CastCircularSlice();
    }

    public override List<Vector2> GetCastPoint(int act)
    {
        return new List<Vector2>() { new Vector2 (0 , 0) };
    }

    private void CastCircularSlice()
    {
        int _radius = radius;
        if (act > 1) _radius = radiusUpgrade + radius;

        List<Vector2> CircularList = GridAreaMethods.SquareAOE(bossManager.CurrentCoordinates, bossManager.CurrentCoordinates, _radius, true);

        CastAreaForSkill(CircularList);
        
        MonoInstance.instance.StartCoroutine(DamagePlayers(GetAffectedCombatObjectList(CircularList)));
    }

    protected override void CastAreaForSkill(List<Vector2> circularList)
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
        
        BossCombatObject bossCombatObject = new(bossManager);

        foreach (CombatObject combatObject in affectedCombatObjectList)
        {
            if (CombatStageManager.instance.currentStage is BossTurnStage) CombatMethods.ApplayDamage(damage, bossCombatObject, combatObject);
            hitSFX.Play(bossManager.transform);
        }

        CastEnd();
    }
}
