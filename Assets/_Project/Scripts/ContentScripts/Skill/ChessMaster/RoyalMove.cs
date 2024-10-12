using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sonity;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class RoyalMove : SkillScript
{
    [Header("RoyalMove")]
    
    [Header("Prefabs")]
    [SerializeField] GameObject RoyalMovePrefab;

    private PlayerSkillManager playerSkillManager => GameObject.FindObjectOfType<PlayerSkillManager>();
    private Vector2 selectedheroPosition;

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);
        if (skillIndex == 0) CastFX();

        CastRoyalMove();

        CastEnd();

    }
    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> areaList = new() { selectedCellCoordinate };
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> areaList = new();
        if (skillIndex == 0)
        {
            areaList.AddRange(mapClass.AllTiles);
        }
        else
        {
            Vector2 selectedPlayer = playerSkillManager.TargetTileList[playerSkillManager.TargetTileList.Count - 1][0];
            areaList.AddRange(GridAreaMethods.SquareAOE(characterCellCoordinate, selectedPlayer));
        }
        
        return areaList;
    }

    private void CastRoyalMove()
    {
        List<MapObject> objectsInFirstPointList = GetObjectsFromPoint(SelectedCellCoordinate[0]).ToList();
        List<MapObject> objectsInSecondPointList = GetObjectsFromPoint(SelectedCellCoordinate[1]).ToList();

        MapObject heroMapObject = null;
        foreach (MapObject mapObject in objectsInFirstPointList) if (mapObject is Hero) heroMapObject = mapObject;

        if (objectsInSecondPointList.Count == 0 && objectsInFirstPointList.Count > 0)
        {
            for (int i = 0; i < objectsInFirstPointList.Count; i++)
            {
                if (objectsInFirstPointList[i] is Hero)
                {
                    GlobalEventSystem.SendPlayerStartMove();
                    int heroID = objectsInFirstPointList[i].ID;
                    mapClass.RemoveHero(SelectedCellCoordinate[0], heroID);
                    GameObject heroObject = combatPlayerDataInStage.PlayersHeroes[heroID];
                    if (heroObject.GetComponent<NetworkObject>().IsOwner) heroObject.transform.position = SelectedCellCoordinate[1] + mapClass.tileZero;
                    combatPlayerDataInStage.HeroCoordinates[heroID] = SelectedCellCoordinate[1];
                    GlobalEventSystem.SendPlayerEndMove();
                    mapClass.SetHero(SelectedCellCoordinate[1], heroID);
                }
                else if (objectsInFirstPointList[i] is Boss)
                {
                    mapClass.RemoveBoss(SelectedCellCoordinate[0]);
                    bossManager.BossGameObject.transform.position = SelectedCellCoordinate[1] + mapClass.tileZero;
                    bossManager.CurrentCoordinates = SelectedCellCoordinate[1];
                    mapClass.SetBoss(SelectedCellCoordinate[1]);
                }
            }
            
        }

        if (objectsInFirstPointList.Count > 0 && heroMapObject != null) NetworkInstance.instance.ChangePlayerEnergyRpc(combatPlayerDataInStage._TotalStatsList[heroMapObject.ID].currentCombat.CurrentEnergy + 1, heroMapObject.ID);
    }
}
