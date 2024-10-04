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
        CastStart(targetPoints);

        Debug.Log("Cast Berserk Circular Slice!");
        CastCircularSlice();

        CastEnd();
    }

    public override List<Vector2> GetCastPoint(int act)
    {
        return new List<Vector2>() { new Vector2 (0 , 0) };
    }

    private void CastCircularSlice()
    {
        List<int> playersID = new List<int>();
        List<Vector2> CircularList = GridAreaMethods.CircleAOE(bossManager.CurrentCoordinates, bossManager.CurrentCoordinates, 2, true);
        for (int i = 0; i < CircularList.Count; i++)
        {
            Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(CircularList[i] + mapClass.tileZero);
            if (mapClass.gameplayTilemap.HasTile(tile))
            {
                GameObject AffectedTile = MonoInstance.Instantiate(affectedTile, CircularList[i] + mapClass.tileZero, Quaternion.identity);
                MonoInstance.Destroy(AffectedTile, timeBetweenCastAndDamage);
                List<MapObject> mapObjects = mapClass.GetMapObjectList(CircularList[i]);
                foreach (MapObject mapObject in mapObjects)
                {
                    if (mapObject is Hero)
                    {
                        int playerID = mapObject.ID;
                        playersID.Add(playerID);
                    }
                }
            }
        }
        MonoInstance.instance.StartCoroutine(DamagePlayers(playersID));
    }

    IEnumerator DamagePlayers(List<int> playerID)
    {
        yield return new WaitForSeconds(timeBetweenCastAndDamage);
        for (int i = 0;i < playerID.Count;i++)
        {
            BossCombatObject bossCombatObject = new BossCombatObject();
            HeroCombatObject heroCombatObject = new HeroCombatObject(playerID[i], combatPlayerDataInStage);
            CombatMethods.ApplayDamage(damage, bossCombatObject, heroCombatObject);
        }
    }
}
