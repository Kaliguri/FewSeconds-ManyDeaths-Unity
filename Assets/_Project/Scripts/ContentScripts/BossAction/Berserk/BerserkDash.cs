using MoreMountains.Tools;
using Sirenix.OdinInspector;
using Sonity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BerserkDash : BossActionScript
{
    [Header("Stats")]
    [SerializeField] private float damage = 10f;


    [Header("Visual")]
    [SerializeField] private float TimeBetweenDash = 0.7f;
    [SerializeField] private float DashTime = 0.9f;
    

    [Header("SFX")]
    [SerializeField] SoundEvent castSFX;
    [SerializeField] SoundEvent hitSFX;


    public override void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints, act);
        castSFX.Play(bossManager.transform);

        Debug.Log("Cast Berserk Dash!");
        CastBerserkDash();
    }

    public override List<Vector2> GetCastPoint(int act)
    {
        List<Vector2> targetPoints = new();
        if (act > 1)
        {
            targetPoints.Add(mapClass.AllTiles[UnityEngine.Random.Range(0, mapClass.AllTiles.Count)]);
            targetPoints.Add(mapClass.AllTiles[UnityEngine.Random.Range(0, mapClass.AllTiles.Count)]);
        }
        targetPoints.Add(bossMultiplayerMethods.GetRandomPointNearRandomPlayer());
        return targetPoints;
    }

    private void CastBerserkDash()
    {
        if (CombatStageManager.instance.currentStage is BossTurnStage) mapClass.RemoveBoss(bossManager.CurrentCoordinates);
        MonoInstance.instance.StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        Vector2 fromTile = bossManager.CurrentCoordinates;
        List<List<Vector2>> pathList = new();

        for (int i = 0; i < TargetPoints.Count; i++)
        {
            if (i > 0) fromTile = TargetPoints[i - 1];
            List<PathNode> pathToRandomNodes = mapClass.gridPathfinding.FindPath((int)fromTile.x, (int)fromTile.y, (int)TargetPoints[i].x, (int)TargetPoints[i].y);
            pathList.Add(PathNodeToVector2(pathToRandomNodes));
            if (act > 0) CastAreaForSkill(PathNodeToVector2(pathToRandomNodes));
        }

        for (int i = 0; i < pathList.Count; i++) 
        { 
            yield return MonoInstance.instance.StartCoroutine(MoveAlongPath(pathList[i]));
        }

        if (CombatStageManager.instance.currentStage is BossTurnStage) mapClass.SetBoss(TargetPoints[TargetPoints.Count - 1]);
        bossManager.CurrentCoordinates = TargetPoints[TargetPoints.Count - 1];

        DestroyAffectedTilesPrefabs();

        CastEnd();
    }

    IEnumerator MoveAlongPath(List<Vector2> pathNodes)
    {
        float currentDashTime = DashTime * 0.075f;

        for (int i = 0; i < pathNodes.Count - 1; i++)
        {
            Vector2 startPos = new Vector2(pathNodes[i].x, pathNodes[i].y) + mapClass.tileZero;
            Vector2 endPos = new Vector2(pathNodes[i + 1].x, pathNodes[i + 1].y) + mapClass.tileZero;

            float timeElapsed = 0f;

            while (timeElapsed < currentDashTime)
            {
                timeElapsed += Time.deltaTime;
                float t = timeElapsed / currentDashTime;

                if (CombatStageManager.instance.currentStage is PlayerTurnStage) bossManager.GhostBossGameObject.transform.position = Vector2.Lerp(startPos, endPos, t);
                else if(CombatStageManager.instance.currentStage is not PlayerTurnStage) bossManager.BossGameObject.transform.position = Vector2.Lerp(startPos, endPos, t);

                yield return null;
            }

            if (CombatStageManager.instance.currentStage is PlayerTurnStage) bossManager.GhostBossGameObject.transform.position = endPos;
            else bossManager.BossGameObject.transform.position = endPos;

            if (act > 0 && CombatStageManager.instance.currentStage is BossTurnStage) DamageEveryOneInTiles(new List<Vector2> { endPos - mapClass.tileZero }, damage, hitSFX);

            currentDashTime = Mathf.Min(currentDashTime * 1.1f, DashTime);
        }

        yield return new WaitForSeconds(TimeBetweenDash);
    }


    private List<Vector2> PathNodeToVector2(List<PathNode> pathToRandomNodes)
    {
        List<Vector2> result = new List<Vector2>();
        for (int i = 0; i < pathToRandomNodes.Count; i++)
        {
            result.Add(new Vector2(pathToRandomNodes[i].x, pathToRandomNodes[i].y));
        }
        return result;
    }
}
