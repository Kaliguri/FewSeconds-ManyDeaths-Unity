using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;

[Serializable]
public class ProtectiveAura: SkillScript
{
    [Header("Protective Aura")]

    [Title("Stats")]
    [SerializeField] float shieldValue = 20f;


    [Title("Prefabs")]
    [SerializeField] GameObject AuraSkillPrefab;
    
    
    [Title("SFX")]
    [SerializeField] SoundEvent castSFX;
    
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
            if (combatObject is HeroCombatObject && combatObject.ObjectID == playerID) CombatMethods.ApplayShield(shieldValue * 2, GetHeroCombatObject(playerID), combatObject);
            else CombatMethods.ApplayShield(shieldValue, GetHeroCombatObject(playerID), combatObject);
        }
    }
}