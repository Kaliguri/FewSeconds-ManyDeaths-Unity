using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;

[Serializable]
public class Sacrifice : SkillScript
{
    [Header("Sacrifice")]

    [Header("Stats")]
    [SerializeField] float healParameter = 999f;

    
    [Header("Prefabs")]
    [SerializeField] GameObject SacrificePrefab;

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);
        CastFX();

        SpawnSkillSpawnSpritesPrefab();
        ApplayHeal(playerID);

        if (skillIndex == 0) CastEnd();

    }
    protected override void CastFX()
    {
        CastVFX(new List<Vector2> { ActualHeroPosition }, CastVFXPrefab);
        if (castSFX != null) castSFX.Play(combatPlayerDataInStage.transform);
    }

    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> areaList = new() { characterCellCoordinate };
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
            if (combatObject.ObjectID == playerID) 
            { 
                CombatMethods.ApplayDamage(healParameter, GetHeroCombatObject(playerID), combatObject);
            }
            else 
            { 
                SpawnSkillObjects(new List<Vector2> { combatObject.GetPosition() }, AreaVFXPrefab);
                CombatMethods.ApplayHeal(healParameter, GetHeroCombatObject(playerID), combatObject); 
            }
        }
    }
}
