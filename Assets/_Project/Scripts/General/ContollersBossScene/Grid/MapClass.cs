using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapClass : NetworkBehaviour
{
    [Title("Settings")]
    [SerializeField] int Max_A;
    [SerializeField] int Max_B;

    [HideInInspector]
    public Vector2 tileZero;
    public enum TileStates
    {
        Free = 0,
        Terrain = 1,
        Player = 2,
        Boss = 3,
        Other = 4
    }
    public Tilemap gameplayTilemap => FindObjectOfType<GameplayTilemapTag>().gameObject.GetComponent<Tilemap>();
    private TileStates[,] Map;
    private GameObject DownLeftPoint => FindObjectOfType<DownLeftPointTag>().gameObject;
    private Vector3Int DownLeftTile;
    //[SerializeField] TileBase terrainTile;

    void Awake()
    {
        Map = new TileStates[Max_A, Max_B];
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
                //Vector2 TempPoint = new((float)i, (float)j);
                //TempPoint += tileZero;
                //Vector3Int TempTile = gameplayTilemap.WorldToCell(TempPoint);
                
                //if (gameplayTilemap.GetTile(TempTile) != terrainTile) Map[i,j] = TileStates.Free;
                //else Map[i,j] = TileStates.Terrain;
                Map[i,j] = TileStates.Free;
            }
        }
    }

    public bool IsFree(Vector2 tile)
    {
        if (Map[(int)tile.x, (int)tile.y] == TileStates.Free) return true;
        return false;
    }

    public TileStates TileState(Vector2 tile)
    {
        return Map[(int)tile.x, (int)tile.y];
    }

    public void ChangeCell(Vector2 tile, TileStates State)
    {
        Map[(int)tile.x, (int)tile.y] = State;
    }
}

[Serializable]
public class TileInfo
{
    public bool isFreeToMove => MapObjectList.Count == 0;
    public List<MapObject> MapObjectList = new List<MapObject>{new PermanentlyBlocked()};
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
}

[Serializable]
public class Boss : MapObject
{

}


[Serializable]
public class TempBloked : MapObject
{

}

public class PermanentlyBlocked : MapObject
{

}


[Serializable]
public class TileFffect : MapObject
{

}