using MoreMountains.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BerserkDash : BossActionScript
{
    [SerializeField] private float TimeBetweenDash = 0.7f;
    [SerializeField] private float DashTime = 0.9f;
    [SerializeField] private float damage = 10f;

    public override void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints, act);

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

        if (act > 1)
        {
            Vector2 randomTile1 = mapClass.AllTiles[UnityEngine.Random.Range(0, mapClass.AllTiles.Count)];
            List<PathNode> pathToRandomNodes = mapClass.gridPathfinding.FindPath((int)fromTile.x, (int)fromTile.y, (int)randomTile1.x, (int)randomTile1.y);
            yield return MonoInstance.instance.StartCoroutine(MoveAlongPath(pathToRandomNodes));
            Vector2 randomTile2 = mapClass.AllTiles[UnityEngine.Random.Range(0, mapClass.AllTiles.Count)];
            pathToRandomNodes = mapClass.gridPathfinding.FindPath((int)randomTile1.x, (int)randomTile1.y, (int)randomTile2.x, (int)randomTile2.y);
            yield return MonoInstance.instance.StartCoroutine(MoveAlongPath(pathToRandomNodes));
            fromTile = randomTile2;
        }


        Vector2 toTile = TargetPoints[0];
        List<PathNode> pathNodes = mapClass.gridPathfinding.FindPath((int)fromTile.x, (int)fromTile.y, (int)toTile.x, (int)toTile.y);
        yield return MonoInstance.instance.StartCoroutine(MoveAlongPath(pathNodes));

        bossManager.CurrentCoordinates = toTile;
        mapClass.SetBoss(toTile);

        CastEnd();
    }

    IEnumerator MoveAlongPath(List<PathNode> pathNodes)
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

                bossManager.BossGameObject.transform.position = Vector2.Lerp(startPos, endPos, t);

                yield return null;
            }

            bossManager.BossGameObject.transform.position = endPos;

            if (act > 0) DamageEveryOneInTiles(new List<Vector2> { endPos - mapClass.tileZero }, damage);

            currentDashTime = Mathf.Min(currentDashTime * 1.1f, DashTime);
        }

        yield return new WaitForSeconds(TimeBetweenDash);
    }    
}
