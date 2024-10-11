using Sonity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BerserkCircularSliceWithStep : BossActionScript
{
    [Header("Stats")]
    [SerializeField] private int damage = 30;
    [SerializeField] private int radius = 2;
    [SerializeField] private int radiusUpgrade = 1;

    [Header("Visual")]
    [SerializeField] private float timeBetweenCastAndDamage = 1f;


    [Header("Prefabs")]
    [SerializeField] private GameObject affectedTile;


    [Header("SFX")]
    [SerializeField] SoundEvent castSFX;
    [SerializeField] SoundEvent hitSFX;


    public override void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints, act);
        castSFX.Play(bossManager.transform);

        Debug.Log("Cast Berserk Circular Slice!");
        CastCircularSlice();
    }

    public override List<Vector2> GetCastPoint(int act)
    {
        List<Vector2> tilesAroundBoss = GridAreaMethods.AllCardinalLines(new Vector2 (0, 0), new Vector2(0, 0), 1, 1);
        return new List<Vector2>() { tilesAroundBoss[UnityEngine.Random.Range(0, tilesAroundBoss.Count)] };
    }

    private void CastCircularSlice()
    {
        StepInRandomDirrection();

        int _radius = radius;
        if (act > 1) _radius = radiusUpgrade + radius;

        List<Vector2> CircularList = GridAreaMethods.SquareAOE(bossManager.CurrentCoordinates, bossManager.CurrentCoordinates, _radius, true);

        CastAreaForSkill(CircularList);

        MonoInstance.instance.StartCoroutine(DamagePlayers(GetAffectedCombatObjectList(CircularList)));
    }

    private void StepInRandomDirrection()
    {
        Vector2 targetPoint = TargetPoints[0] + bossManager.CurrentCoordinates;
        if (IsFreeTile(targetPoint))
        {
            mapClass.RemoveBoss(bossManager.CurrentCoordinates);
            bossManager.BossGameObject.transform.position = targetPoint + mapClass.tileZero;
            bossManager.CurrentCoordinates = targetPoint;
            mapClass.SetBoss(targetPoint);
        }
    }

    private bool IsFreeTile(Vector2 targetPoint)
    {
        Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(targetPoint + mapClass.tileZero);
        if (mapClass.gameplayTilemap.HasTile(tile))
        {
            List<MapObject> mapObjects = mapClass.GetMapObjectList(targetPoint);
            if (!mapObjects.Exists(x => x is Hero or Boss or TempBloked or NoPlayableTile)) return true;
        }
        return false;
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
            hitSFX.Play(bossManager.transform);
        }

        CastEnd();
    }
}
