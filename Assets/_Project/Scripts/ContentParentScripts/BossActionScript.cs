using Sonity;
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class BossActionScript
{
    [Header("Prefabs")]
    [SerializeField] protected GameObject affectedTile;

    protected MapClass mapClass => GameObject.FindObjectOfType<MapClass>();
    protected BossManager bossManager => GameObject.FindObjectOfType<BossManager>();
    protected BossMultiplayerMethods bossMultiplayerMethods => GameObject.FindObjectOfType<BossMultiplayerMethods>();
    protected CombatPlayerDataInStage combatPlayerDataInStage => GameObject.FindObjectOfType<CombatPlayerDataInStage>();
    protected List<Vector2> TargetPoints = new();
    protected int act;
    protected List<GameObject> affectedTiles;
    protected GameState castStage;

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
        castStage = CombatStageManager.instance.currentStage;
    }

    protected void CastEnd()
    {
        //Debug.Log("CastActionEnd");
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

    protected virtual void CastAreaForSkill(List<Vector2> circularList)
    {
        for (int i = 0; i < circularList.Count; i++)
        {
            Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(circularList[i] + mapClass.tileZero);
            if (mapClass.gameplayTilemap.HasTile(tile))
            {
                GameObject AffectedTile = MonoInstance.Instantiate(affectedTile, circularList[i] + mapClass.tileZero, Quaternion.identity);
                if (CombatStageManager.instance.currentStage is PlayerTurnStage)
                {
                    Color bossColor = AffectedTile.GetComponent<SpriteRenderer>().color;
                    AffectedTile.GetComponent<SpriteRenderer>().color = new Color(bossColor.r, bossColor.g, bossColor.b, bossColor.a * bossManager.alfhaForGhost);
                }
                affectedTiles.Add(AffectedTile);
            }
        }
    }

    protected void DestroyAffectedTilesPrefabs()
    {
        for (int i = affectedTiles.Count - 1; i >= 0; i--)
        {
            if (i < affectedTiles.Count)
            {
                GameObject tile = affectedTiles[i];
                if (tile != null)
                {
                    MonoInstance.Destroy(tile);
                }
            }
        }
        affectedTiles.Clear();
    }

    protected List<Vector2> ClearListFromOccupiedTiles(List<Vector2> TilesList)
    {
        List<Vector2> ClearTilesList = new();
        for (int j = 0; j < TilesList.Count; j++)
        {
            Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(TilesList[j] + mapClass.tileZero);
            if (mapClass.gameplayTilemap.HasTile(tile))
            {
                List<MapObject> mapObjects = mapClass.GetMapObjectList(TilesList[j]);
                if (!mapObjects.Exists(x => x is Hero or Boss or TempBloked or NoPlayableTile)) ClearTilesList.Add(TilesList[j]);
            }
        }
        return ClearTilesList;
    }

    protected void DamageEveryOneInTiles(List<Vector2> tiles, float damage, SoundEvent hitSound)
    {
        List<CombatObject> affectedCombatObjectList = GetAffectedCombatObjectList(tiles);
        BossCombatObject bossCombatObject = new BossCombatObject(bossManager);

        foreach (CombatObject combatObject in affectedCombatObjectList) 
        {
            CombatMethods.ApplayDamage(damage, bossCombatObject, combatObject); 
            hitSound.Play(bossManager.transform); 
        }
    }
}
