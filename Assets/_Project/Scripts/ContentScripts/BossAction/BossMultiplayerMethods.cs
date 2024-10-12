using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

public class BossMultiplayerMethods : NetworkBehaviour
{
    private MapClass mapClass => GameObject.FindObjectOfType<MapClass>();
    private BossManager bossManager => GameObject.FindObjectOfType<BossManager>();
    private CombatPlayerDataInStage combatPlayerDataInStage => GameObject.FindObjectOfType<CombatPlayerDataInStage>();

    public Vector2 GetRandomPointNearRandomPlayer()
    {
        Vector2 heroCoordinate = GetRandomAliveHeroCoordinate();

        List<Vector2> cellsAroundPlayer = GridAreaMethods.SquareAOE(heroCoordinate, heroCoordinate, 1, true);
        List<Vector2> cellsAroundPlayers = ClearListFromOccupiedTiles(cellsAroundPlayer);

        Vector2 TargetPoint = cellsAroundPlayers[UnityEngine.Random.Range(0, cellsAroundPlayers.Count)];
        return TargetPoint;
    }

    private List<Vector2> ClearListFromOccupiedTiles(List<Vector2> TilesList)
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

    private Vector2 GetRandomAliveHeroCoordinate()
    {
        List<Vector2> HeroCoordinates = new();
        for (int j = 0; j < combatPlayerDataInStage.HeroCoordinates.Length; j++) if (combatPlayerDataInStage.aliveStatus[j]) HeroCoordinates.Add(combatPlayerDataInStage.HeroCoordinates[j]);
        if (HeroCoordinates.Count > 0) return HeroCoordinates[UnityEngine.Random.Range(0, HeroCoordinates.Count)];
        else return combatPlayerDataInStage.HeroCoordinates[UnityEngine.Random.Range(0, combatPlayerDataInStage.HeroCoordinates.Length)];
    }
}
