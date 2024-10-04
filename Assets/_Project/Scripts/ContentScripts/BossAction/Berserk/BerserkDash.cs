using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BerserkDash : BossActionScript
{
    [SerializeField] private float TimeBetweenMovement = 0.7f;
    [SerializeField] private float TimeBetweenDifference = 0.9f;
    private float CurrentTimeBetweenMovement;

    public override void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints);

        Debug.Log("Cast Berserk Dash!");
        CastBerserkDash();
    }

    public override List<Vector2> GetCastPoint(int act)
    {
        return bossMultiplayerMethods.GetRandomPointNearRandomPlayer();
    }

    private void CastBerserkDash()
    {
        mapClass.RemoveBoss(bossManager.CurrentCoordinates);
        MonoInstance.instance.StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        Vector2 fromTile = bossManager.CurrentCoordinates;
        Vector2 toTile = TargetPoints[0];
        List<PathNode> pathNodes = mapClass.gridPathfinding.FindPath((int)fromTile.x, (int)fromTile.y, (int)toTile.x, (int)toTile.y);
        CurrentTimeBetweenMovement = TimeBetweenMovement;
        for (int i = 0; i < pathNodes.Count; i++)
        {
            Vector2 targetTile = new Vector2(pathNodes[i].x, pathNodes[i].y);
            bossManager.BossGameObject.transform.position = targetTile + mapClass.tileZero;
            CurrentTimeBetweenMovement *= TimeBetweenDifference;
            yield return new WaitForSeconds(CurrentTimeBetweenMovement);
        }

        bossManager.CurrentCoordinates = toTile;
        mapClass.SetBoss(toTile);

        CastEnd();
    }
}
