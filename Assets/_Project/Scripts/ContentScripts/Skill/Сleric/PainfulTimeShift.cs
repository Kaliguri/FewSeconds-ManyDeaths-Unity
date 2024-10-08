using Sirenix.OdinInspector.Demos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PainfulTimeShift : SkillScript
{
    [Header("PainfulTimeShift")]
    [SerializeField] GameObject PainfulTimeShiftPrefab;
    [SerializeField] float damageParameter = 10f;
    [SerializeField] float restoreEnergyParametr = 1f;

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);

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
                CombatMethods.ApplayDamage(damageParameter, GetHeroCombatObject(playerID), combatObject);
                NetworkInstance.instance.ChangePlayerEnergyRpc(combatPlayerDataInStage._TotalStatsList[combatObject.ObjectID].currentCombat.CurrentEnergy + 1, combatObject.ObjectID);
            }
        }
    }
}
