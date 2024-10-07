using Sirenix.OdinInspector.Demos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class VerticalSign : SkillScript
{
    [Header("VerticalSign")]
    [SerializeField] GameObject VerticalSignPrefab;
    [SerializeField] float shieldParameter = 40f;

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);

        SpawnSkillSpawnSpritesPrefab();
        ApplayShield(playerID);

        CastEnd();

    }
    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        Vector2 upTile = new Vector2(characterCellCoordinate.x, mapClass.Max_B);
        Vector2 downTile = new Vector2(characterCellCoordinate.x, 0);
        List<Vector2> areaList = GridAreaMethods.CoordinateLine(upTile, downTile, 1, mapClass.Max_B);
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return mapClass.AllTiles;
    }


    void SpawnSkillSpawnSpritesPrefab()
    {
        SpawnSkillObjects(GetArea(), VerticalSignPrefab);
    }

    void ApplayShield(int playerID)
    {
        foreach (CombatObject combatObject in GetAffectedCombatObjectList())
        {
            CombatMethods.ApplayShield(shieldParameter, GetHeroCombatObject(playerID), combatObject);
        }
    }
}
