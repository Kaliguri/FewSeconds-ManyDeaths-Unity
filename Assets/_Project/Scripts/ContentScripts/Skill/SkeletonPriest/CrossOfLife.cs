using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;

[Serializable]
public class CrossOfLife : SkillScript
{
    [Header("Cross Of Life")]

    [Header("Stats")]
    [SerializeField] float healValue = 80f;


    [Header("Prefabs")]
    [SerializeField] GameObject CrossOfLifePrefab;
    

    [Header("SFX")]
    [SerializeField] SoundEvent castSFX;

    
    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);
        castSFX.Play(combatPlayerDataInStage.transform);

        SpawnSkillSpawnSpritesPrefab();
        ApplayHeal(playerID);

        CastEnd();

    }
    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> areaList = GridAreaMethods.AllCardinalLines(selectedCellCoordinate, selectedCellCoordinate, 1, 1);
        areaList.Add(selectedCellCoordinate);
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return mapClass.AllTiles;
    }


    void SpawnSkillSpawnSpritesPrefab()
    {
        SpawnSkillObjects(GetArea(), CrossOfLifePrefab);
    }

    void ApplayHeal(int playerID)
    {
        foreach (CombatObject combatObject in GetAffectedCombatObjectList())
        {
            CombatMethods.ApplayHeal(healValue, GetHeroCombatObject(playerID), combatObject);
        }
    }
}
