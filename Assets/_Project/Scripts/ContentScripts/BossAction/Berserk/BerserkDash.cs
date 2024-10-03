using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BerserkDash : BossActionScript
{
    public override void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints);

        Debug.Log("Cast Berserk Dash!");
        CastBerserkDash();

        CastEnd();
    }

    public override List<Vector2> GetCastPoint(int act)
    {
        return bossMultiplayerMethods.GetRandomPointNearRandomPlayer();
    }

    private void CastBerserkDash()
    {
        mapClass.RemoveBoss(bossManager.CurrentCoordinates);
        bossManager.BossGameObject.transform.position = TargetPoints[0] + mapClass.tileZero;
        bossManager.CurrentCoordinates = TargetPoints[0];
        mapClass.SetBoss(TargetPoints[0]);
    }

}
