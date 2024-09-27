using System;
using System.Collections.Generic;
using MoreMountains.Tools;
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
    private TileInfo[,] TileMap;
    private GameObject DownLeftPoint => FindObjectOfType<DownLeftPointTag>().gameObject;
    private Vector3Int DownLeftTile;
    //[SerializeField] TileBase terrainTile;

    void Start()
    {
        TileMap = new TileInfo[Max_A, Max_B];
        DownLeftTile = gameplayTilemap.WorldToCell(DownLeftPoint.transform.position);
        tileZero = gameplayTilemap.GetCellCenterWorld(DownLeftTile);
        FindTerrain();
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

    public void RemoveHero(Vector2 tile, int playerID)
    {
        TileMap[(int)tile.x, (int)tile.y].MapObjectList.Remove(new Hero(playerID));
    }

    public void RemoveBoss(Vector2 tile)
    {
        TileMap[(int)tile.x, (int)tile.y].MapObjectList.Remove(new Boss());
    }

    public void RemoveTempBloked(Vector2 tile)
    {
        TileMap[(int)tile.x, (int)tile.y].MapObjectList.Remove(new TempBloked());
    }
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

}
[Serializable]
public class Hero : MapObject
{
    public int PlayerID;

    public Hero(int playerID)
    {
        PlayerID = playerID;
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