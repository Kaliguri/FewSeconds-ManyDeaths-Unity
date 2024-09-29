using System.Collections.Generic;
using UnityEngine;

public class DamageExampleSkill : SkillScript
{
    [SerializeField] float Damage;
    [SerializeField] GameObject ExampleSkillPrefab;
    
    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] castPosition, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, castPosition);

        SpawnSkillPrefab();
        ApplayDamage(playerID);
                
        CastEnd();
        
    }
    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
 
        List<Vector2> areaList = GridAreaMethods.SquareAOE(characterCellCoordinate, selectedCellCoordinate, radius: 2);
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> areaList = GridAreaMethods.SquareAOE(characterCellCoordinate, selectedCellCoordinate, radius: 20);
        return areaList;
    }

    void SpawnSkillPrefab()
    {
        SpawnSkillObjects(GetArea(), ExampleSkillPrefab);
    }
    
    void ApplayDamage(int playerID)
    {
        foreach (CombatObject combatObject in GetAffectedCombatObjectList())
        {
            CombatMethods.ApplayDamage(Damage, GetHeroCombatObject(playerID), combatObject);
        }
    }
}
