using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Demos;
using Sonity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class HorizontalSign : SkillScript
{
    [Header("Horizontal Sign")]

    [Header("Stats")]
    [SerializeField] float shieldValue = 40f;


    [Header("Prefabs")]
    [SerializeField] GameObject HorizontalSignPrefab;

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);
        CastFX();

        SpawnSkillSpawnSpritesPrefab();
        ApplayShield(playerID);

        CastEnd();

    }
    protected override void CastFX()
    {
        CastVFX(new List<Vector2> { ActualHeroPosition }, CastVFXPrefab);
        CastVFX(GetArea(), AreaVFXPrefab);
        if (castSFX != null) castSFX.Play(combatPlayerDataInStage.transform);
    }

    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        Vector2 leftTile = new Vector2(-1, characterCellCoordinate.y);
        Vector2 rightTile = new Vector2(mapClass.Max_A, characterCellCoordinate.y);
        List<Vector2> areaList = GridAreaMethods.CoordinateLine(leftTile, rightTile, 1, mapClass.Max_A);
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return mapClass.AllTiles;
    }


    void SpawnSkillSpawnSpritesPrefab()
    {
        SpawnSkillObjects(GetArea(), HorizontalSignPrefab);
    }

    void ApplayShield(int playerID)
    {
        foreach (CombatObject combatObject in GetAffectedCombatObjectList())
        {
            CombatMethods.ApplayShield(shieldValue, GetHeroCombatObject(playerID), combatObject);
        }
    }
}
