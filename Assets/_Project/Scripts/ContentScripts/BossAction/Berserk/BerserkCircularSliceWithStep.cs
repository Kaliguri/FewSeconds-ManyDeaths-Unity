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

    [Header("SFX")]
    [SerializeField] SoundEvent castSFX;
    [SerializeField] SoundEvent hitSFX;


    public override void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints, act);
        castSFX.Play(bossManager.transform);

        Debug.Log("Cast Berserk Circular Slice With Step!");
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
            if (CombatStageManager.instance.currentStage is BossTurnStage) mapClass.RemoveBoss(bossManager.CurrentCoordinates);

            if (CombatStageManager.instance.currentStage is PlayerTurnStage) bossManager.GhostBossGameObject.transform.position = targetPoint + mapClass.tileZero;
            else bossManager.BossGameObject.transform.position = targetPoint + mapClass.tileZero;

            bossManager.CurrentCoordinates = targetPoint;
            if (CombatStageManager.instance.currentStage is BossTurnStage) mapClass.SetBoss(targetPoint);
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

    protected override void CastAreaForSkill(List<Vector2> circularList)
    {
        for (int i = 0; i < circularList.Count; i++)
        {
            Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(circularList[i] + mapClass.tileZero);
            if (mapClass.gameplayTilemap.HasTile(tile))
            {
                GameObject AffectedTile = MonoInstance.Instantiate(affectedTile, circularList[i] + mapClass.tileZero, Quaternion.identity);
                if (CombatStageManager.instance.currentStage is PlayerTurnStage)
                {
                    Color bossColor = AffectedTile.GetComponentInChildren<SpriteRenderer>().color;
                    AffectedTile.GetComponentInChildren<SpriteRenderer>().color = new Color(bossColor.r, bossColor.g, bossColor.b, bossColor.a * bossManager.alfhaForGhost);
                }
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
            if (CombatStageManager.instance.currentStage is BossTurnStage) CombatMethods.ApplayDamage(damage, bossCombatObject, combatObject);
            hitSFX.Play(bossManager.transform);
        }

        CastEnd();
    }
}
