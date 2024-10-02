using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[Serializable]
public class TeleportExampleSkill : SkillScript
{
    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);

        List<MapObject> objectsInFirstPointList = GetObjectsFromPoint(selectedCellCoordinate[0]);
        List<MapObject> objectsInSecondPointList = GetObjectsFromPoint(selectedCellCoordinate[1]);
        TeleportObjects(objectsInFirstPointList, selectedCellCoordinate[1]);
        TeleportObjects(objectsInSecondPointList, selectedCellCoordinate[0]);

        CastEnd();
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

    private List<MapObject> GetObjectsFromPoint(Vector2 point)
    {
        List<MapObject> mapObjectList = mapClass.GetMapObjectList(point);

        return mapObjectList;
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
        Debug.Log("TeleportBoss");
    }

    private void TeleportHero(MapObject hero, Vector2 targetPont)
    {
        GlobalEventSystem.SendPlayerStartMove();
        int heroID = hero.ID;
        GameObject heroObject = combatPlayerDataInStage.PlayersHeroes[heroID];
        mapClass.RemoveHero(combatPlayerDataInStage.HeroCoordinates[heroID], heroID);
        if (heroObject.GetComponent<NetworkObject>().IsOwner) heroObject.transform.position = targetPont + mapClass.tileZero;
        combatPlayerDataInStage.HeroCoordinates[heroID] = targetPont;
        mapClass.SetHero(combatPlayerDataInStage.HeroCoordinates[heroID], heroID);
        GlobalEventSystem.SendPlayerEndMove();
    }
}
