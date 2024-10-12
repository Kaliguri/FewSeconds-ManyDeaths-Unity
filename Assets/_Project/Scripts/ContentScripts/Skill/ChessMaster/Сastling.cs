using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sonity;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class Сastling : SkillScript
{
    [Header("Сastling")]

    [Header("Prefabs")]
    [SerializeField] GameObject CastlingPrefab;

    
    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);
        if (skillIndex == 0)
        {
            CastFX();

            CastTeleport(skillIndex);
        }

        CastEnd();
    }

    private void CastTeleport(int skillIndex)
    {
        if (skillIndex == 0)
        {
            List<MapObject> objectsInFirstPointList = GetObjectsFromPoint(SelectedCellCoordinate[0]);
            List<MapObject> objectsInSecondPointList = GetObjectsFromPoint(SelectedCellCoordinate[1]);

            TeleportObjects(objectsInFirstPointList, SelectedCellCoordinate[1]);
            TeleportObjects(objectsInSecondPointList, SelectedCellCoordinate[0]);

            ChangeMapObjects(objectsInFirstPointList, SelectedCellCoordinate[0], SelectedCellCoordinate[1]);
            ChangeMapObjects(objectsInSecondPointList, SelectedCellCoordinate[1], SelectedCellCoordinate[0]);
        }
    }

    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> area = new List<Vector2> { selectedCellCoordinate };
        return area;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return mapClass.AllTiles;
    }

    private void TeleportObjects(List<MapObject> objectsInPointList, Vector2 TargetPont)
    {
        for (int i = 0; i < objectsInPointList.Count; i++)
        {
            if (objectsInPointList[i] is Hero) TeleportHero(objectsInPointList[i], TargetPont);
            else if (objectsInPointList[i] is Boss) TeleportBoss(objectsInPointList[i], TargetPont);
            else if (objectsInPointList[i] is TempBloked) TeleportTempBloked(objectsInPointList[i], TargetPont);
        }
    }

    private void TeleportTempBloked(MapObject tempBloked, Vector2 targetPont)
    {
        Debug.Log("TeleportTempBloked");
    }

    private void TeleportBoss(MapObject boss, Vector2 targetPont)
    {
        mapClass.RemoveBoss(bossManager.CurrentCoordinates);
        bossManager.BossGameObject.transform.position = targetPont + mapClass.tileZero;
        bossManager.CurrentCoordinates = targetPont;
        mapClass.SetBoss(targetPont);
    }

    private void TeleportHero(MapObject hero, Vector2 targetPont)
    {
        GlobalEventSystem.SendPlayerStartMove();
        int heroID = hero.ID;
        GameObject heroObject = combatPlayerDataInStage.PlayersHeroes[heroID];
        if (heroObject.GetComponent<NetworkObject>().IsOwner) heroObject.transform.position = targetPont + mapClass.tileZero;
        combatPlayerDataInStage.HeroCoordinates[heroID] = targetPont;
        GlobalEventSystem.SendPlayerEndMove();
    }

    private void ChangeMapObjects(List<MapObject> objectsInPointList, Vector2 fromPoint, Vector2 toPoint)
    {
        for (int i = 0; i < objectsInPointList.Count; i++)
        {
            if (objectsInPointList[i] is Hero)
            {
                MapObject hero = objectsInPointList[i];
                int ID = hero.ID;
                mapClass.RemoveHero(fromPoint, ID);
                mapClass.SetHero(toPoint, ID);
            }
            else if (objectsInPointList[i] is Boss) { mapClass.RemoveBoss(fromPoint); mapClass.SetBoss(toPoint); }
            else if (objectsInPointList[i] is TempBloked) { mapClass.RemoveTempBloked(fromPoint); mapClass.SetTempBloked(toPoint); }
        }
    }
}
