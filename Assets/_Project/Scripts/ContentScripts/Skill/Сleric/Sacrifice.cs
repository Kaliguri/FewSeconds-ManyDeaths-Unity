using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Sacrifice : SkillScript
{
    [Header("Sacrifice")]
    [SerializeField] GameObject SacrificePrefab;
    [SerializeField] float healParameter = 999f;

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);

        SpawnSkillSpawnSpritesPrefab();
        ApplayHeal(playerID);

        CastEnd();

    }
    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> areaList = mapClass.AllTiles;
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return mapClass.AllTiles;
    }


    void SpawnSkillSpawnSpritesPrefab()
    {
        SpawnSkillObjects(GetArea(), SacrificePrefab);
    }

    void ApplayHeal(int playerID)
    {
        List<MapObject> MapObjectList = mapClass.MapObjectCheck(combatPlayerDataInStage.HeroCoordinates.ToList());

        List<CombatObject> combatObjectList = new List<CombatObject>();

        foreach (MapObject mapObject in MapObjectList) if (mapObject is Hero) combatObjectList.Add(new HeroCombatObject(mapObject.ID, combatPlayerDataInStage));

        foreach (CombatObject combatObject in combatObjectList)
        {
            if (combatObject.ObjectID == playerID) CombatMethods.ApplayDamage(healParameter, GetHeroCombatObject(playerID), combatObject);
            else CombatMethods.ApplayHeal(healParameter, GetHeroCombatObject(playerID), combatObject);
        }
    }
}
