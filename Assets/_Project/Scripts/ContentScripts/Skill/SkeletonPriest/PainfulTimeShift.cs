using Sonity;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PainfulTimeShift : SkillScript
{
    [Header("Painful Time Shift")]

    [Header("Stats")]
    [SerializeField] float damageValue = 10f;
    [SerializeField] int restoreEnergyValue = 1;
    

    [Header("Prefabs")]
    [SerializeField] GameObject PainfulTimeShiftPrefab;


    [Header("SFX")]
    [SerializeField] SoundEvent hitSFX;


    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);
        CastFX();

        SpawnSkillSpawnSpritesPrefab();
        CastPainfulTimeShift(playerID);

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
        SpawnSkillObjects(GetArea(), PainfulTimeShiftPrefab);
    }

    void CastPainfulTimeShift(int playerID)
    {
        foreach (CombatObject combatObject in GetAffectedCombatObjectList())
        {
            if (combatObject is HeroCombatObject)
            {
                CombatMethods.ApplayDamage(damageValue, GetHeroCombatObject(playerID), combatObject);
                NetworkInstance.instance.ChangePlayerEnergyRpc(combatPlayerDataInStage._TotalStatsList[combatObject.ObjectID].currentCombat.CurrentEnergy + restoreEnergyValue, combatObject.ObjectID);
            }
        }
    }
}
