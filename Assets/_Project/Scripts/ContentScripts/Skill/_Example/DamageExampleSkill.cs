using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class DamageExampleSkill : SkillScript
{
    [Header("DamageExampleSkill")]
    [SerializeField] float Damage = 20f;
    [SerializeField] GameObject ExampleSkillPrefab;
    
    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);

        SpawnSkillSpawnSpritesPrefab();
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
        return mapClass.AllTiles;
    }


    void SpawnSkillSpawnSpritesPrefab()
    {
        SpawnSkillObjects(GetArea(), ExampleSkillPrefab);
        //Debug.Log("SpawnSprites:" + GetArea().Count);
    }
    
    void ApplayDamage(int playerID)
    {
        foreach (CombatObject combatObject in GetAffectedCombatObjectList())
        {
            CombatMethods.ApplayDamage(Damage, GetHeroCombatObject(playerID), combatObject);
        }
    }
}
