using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;

[Serializable]
public class Recharge : SkillScript
{
    [Title("Quick Shot")]

    [Title("Stats")]
    [SerializeField] float bulletReload;
    [SerializeField] float extraButtetForAllyNear;

    [Title("Prefabs")]
    [SerializeField] GameObject reloadPrefab;


    [Title("SFX")]
    [SerializeField] SoundEvent castSFX;


    private ShotsManager shotsManager => GameObject.FindObjectOfType<ShotsManager>();

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);

        CastRecharge(playerID);

        CastEnd();
    }

    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {

        List<Vector2> areaList = new List<Vector2> { selectedCellCoordinate };
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return mapClass.AllTiles;
    }

    private void CastRecharge(int playerID)
    {
        if (IsPlayersNear()) shotsManager.SetShotsCount(playerID, shotsManager.GetShotsCount(playerID) + 2);
        else shotsManager.SetShotsCount(playerID, shotsManager.GetShotsCount(playerID) + 1);
    }

    private bool IsPlayersNear()
    {
        List<Vector2> areaAround = GridAreaMethods.SquareAOE(ActualHeroPosition, SelectedCellCoordinate[0], 1, true);

        for (int i = 0; i < areaAround.Count; i++)
        {
            List<MapObject> mapObjects = GetObjectsFromPoint(areaAround[i]);
            for (int j = 0; j < mapObjects.Count; j++)
            {
                if (mapObjects[j] is Hero) return true;
            }
        }

        return false;
    }
}
