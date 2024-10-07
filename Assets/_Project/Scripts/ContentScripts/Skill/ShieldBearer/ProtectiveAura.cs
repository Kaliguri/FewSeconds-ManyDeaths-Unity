using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ProtectiveAura: SkillScript
{
    [Header("ProtectiveAura")]
    [SerializeField] GameObject AuraSkillPrefab;
    [SerializeField] float shieldParameter = 20f;
    
    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);

        SpawnSkillSpawnSpritesPrefab();
        ApplayShield(playerID);
                
        CastEnd();
        
    }
    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> areaList = new() { selectedCellCoordinate };
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return mapClass.AllTiles;
    }


    void SpawnSkillSpawnSpritesPrefab()
    {
        SpawnSkillObjects(GetArea(), AuraSkillPrefab);
    }
    
    void ApplayShield(int playerID)
    {
        foreach (CombatObject combatObject in GetAffectedCombatObjectList())
        {
            if (combatObject is HeroCombatObject && combatObject.ObjectID == playerID) CombatMethods.ApplayShield(shieldParameter * 2, GetHeroCombatObject(playerID), combatObject);
            else CombatMethods.ApplayShield(shieldParameter, GetHeroCombatObject(playerID), combatObject);
        }
    }
}
