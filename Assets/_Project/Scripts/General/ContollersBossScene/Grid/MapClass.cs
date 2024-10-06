using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapClass : NetworkBehaviour
{
    [Title("Settings")]
    [SerializeField] public int Max_A;
    [SerializeField] public int Max_B;

    [HideInInspector]
    public Vector2 tileZero;
    public Tilemap gameplayTilemap => FindObjectOfType<GameplayTilemapTag>().gameObject.GetComponent<Tilemap>();
    private CombatPlayerDataInStage combatPlayerDataInStage => FindObjectOfType<CombatPlayerDataInStage>();
    public List<Vector2> AllTiles = new();
    public Pathfinding gridPathfinding;

    private TileInfo[,] TileMap;
    private GameObject DownLeftPoint => FindObjectOfType<DownLeftPointTag>().gameObject;
    private Vector3Int DownLeftTile;
    //[SerializeField] TileBase terrainTile;

    private void Awake()
    {
        GlobalEventSystem.PlayerDied.AddListener(PlayerDied);
    }

    void Start()
    {
        TileMap = new TileInfo[Max_A, Max_B];
        DownLeftTile = gameplayTilemap.WorldToCell(DownLeftPoint.transform.position);
        tileZero = gameplayTilemap.GetCellCenterWorld(DownLeftTile);
        FindTerrain();
        gridPathfinding = new Pathfinding(Max_A, Max_B);
    }

    void FindTerrain()
    {
        for (int i = 0; i < Max_A; i++)
        {
            for (int j = 0; j < Max_B; j++)
            {
                Vector2 TempPoint = new((float)i, (float)j);
                TempPoint += tileZero;
                Vector3Int TempTile = gameplayTilemap.WorldToCell(TempPoint);

                //if (gameplayTilemap.GetTile(TempTile) != terrainTile) Map[i,j] = GetMapObjectLists.Free;
                //else Map[i,j] = GetMapObjectLists.Terrain;
                TileMap[i, j] = new TileInfo();
                if (!gameplayTilemap.HasTile(TempTile))
                {
                    TileMap[i, j].MapObjectList = new()
                    {
                        new NoPlayableTile()
                    };
                }
                else AllTiles.Add(new Vector2(i, j));
            }
        }
    }

    public bool IsPlayable(Vector2 tile)
    {
        if (TileMap[(int)tile.x, (int)tile.y].MapObjectList.Exists(x => x is NoPlayableTile)) return false;
        return true;
    }

    public List<MapObject> GetMapObjectList(Vector2 tile)
    {
        return TileMap[(int)tile.x, (int)tile.y].MapObjectList;
    }

    public List<MapObject> MapObjectCheck(List<Vector2> areaList)
    {
        List<MapObject> mapObjectList = new List<MapObject>();

        foreach (Vector2 tile in areaList)
        {
            Vector3Int tileOnMap = gameplayTilemap.WorldToCell(tile + tileZero);
            if (gameplayTilemap.HasTile(tileOnMap)) mapObjectList.AddRange(GetMapObjectList(tile));
        }

        return mapObjectList;
    }

    private void PlayerDied(int playerID)
    {
        Vector2 heroCoordinate = combatPlayerDataInStage.HeroCoordinates[playerID];
        RemoveHero(heroCoordinate, playerID);
        SetCorpse(heroCoordinate, playerID);
    }

    #region SetRemove

    public void SetHero(Vector2 tile, int playerID)
    {
        TileMap[(int)tile.x, (int)tile.y].MapObjectList.Add(new Hero(playerID));
    }

    public void SetBoss(Vector2 tile)
    {
        TileMap[(int)tile.x, (int)tile.y].MapObjectList.Add(new Boss());
    }

    public void SetTempBloked(Vector2 tile)
    {
        TileMap[(int)tile.x, (int)tile.y].MapObjectList.Add(new TempBloked());
    }

    public void SetCorpse(Vector2 tile, int playerID)
    {
        TileMap[(int)tile.x, (int)tile.y].MapObjectList.Add(new Corpse(playerID));
    }

    public void RemoveHero(Vector2 tile, int playerID)
    {
        TileMap[(int)tile.x, (int)tile.y].MapObjectList.RemoveAll(x => x is Hero && x.ID == playerID);
    }

    public void RemoveCorpse(Vector2 tile, int playerID)
    {
        TileMap[(int)tile.x, (int)tile.y].MapObjectList.RemoveAll(x => x is Corpse && x.ID == playerID);
    }

    public void RemoveBoss(Vector2 tile)
    {
        TileMap[(int)tile.x, (int)tile.y].MapObjectList.RemoveAll(x => x is Boss);
    }

    public void RemoveTempBloked(Vector2 tile)
    {
        TileMap[(int)tile.x, (int)tile.y].MapObjectList.RemoveAll(x => x is TempBloked);
    }

    #endregion

}

[Serializable]
public class TileInfo
{
    public bool isFreeToMove => MapObjectList.Count == 0;
    public List<MapObject> MapObjectList = new();
    public List<TileFffect> TileEffectList;

}

[Serializable]
public class MapObject
{
    public int ID;
}

[Serializable]
public class Hero : MapObject
{
    public Hero(int playerID)
    {
        ID = playerID;
    }
}

[Serializable]
public class Corpse : MapObject
{
    public Corpse(int playerID)
    {
        ID = playerID;
    }
}

[Serializable]
public class Boss : MapObject
{

}

[Serializable]
public class TempBloked : MapObject
{

}

public class NoPlayableTile : MapObject
{

}


[Serializable]
public class TileFffect
{
    public TileEffectData tileEffectData;
}