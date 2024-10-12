using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;

[Serializable]
public class Recharge : SkillScript
{
    [Header("Quick Shot")]

    [Header("Stats")]
    [SerializeField] int bulletReload = 2;
    [SerializeField] int extraButtetForAllyNear = 1;

    [Header("Prefabs")]
    [SerializeField] GameObject reloadPrefab;

    [Header("Localize")]
    public new List<int> LocalizeVariablesList = new();
    public override List<int> GetLocalizeVariablesList()
    {
        return LocalizeVariablesList;
    }



    private ShotsManager shotsManager => GameObject.FindObjectOfType<ShotsManager>();

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);
        CastFX();

        CastRecharge(playerID);

        CastEnd();
    }
    protected override void CastFX()
    {
        CastVFX(new List<Vector2> { ActualHeroPosition }, CastVFXPrefab);
        if (castSFX != null) castSFX.Play(combatPlayerDataInStage.transform);
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
        if (IsPlayersNear()) shotsManager.SetShotsCount(playerID, shotsManager.GetShotsCount(playerID) + bulletReload);
        else shotsManager.SetShotsCount(playerID, shotsManager.GetShotsCount(playerID) + extraButtetForAllyNear);

        LocalizeVariablesList[0] = shotsManager.GetShotsCount(playerID);
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
