using Sirenix.OdinInspector.Demos;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ExampleSkill1 : SkillScript
{
    [SerializeField] GameObject ExampleSkill1Prefab;

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] castPosition, int skillIndex = 0)
    {
        ActualHeroPosition = actualHeroPosition;
        HeroPosition = heroPosition;
        CastPosition = castPosition;

        SpawnSkillPrefab(skillIndex);
                
        MonoInstance.instance.Invoke(nameof(EndCast), SkillDuration);
    }

    protected override void SpawnSkillPrefab(int skillIndex)
    {
        //Debug.Log("Cast ExampleSkill1 to " + CastPosition[skillIndex]);
        List<Vector2> area = GetArea(skillIndex);
        for (int i = 0; i < area.Count; i++)
        {
            Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(area[i] + mapClass.tileZero);
            if (mapClass.gameplayTilemap.HasTile(tile))
            {
                GameObject affectedObject = MonoInstance.Instantiate(ExampleSkill1Prefab, area[i] + mapClass.tileZero, Quaternion.identity);
                MonoInstance.Destroy(affectedObject, SkillPrefabDuration);
            }
        }
    }

    protected override List<Vector2> GetArea(int skillIndex)
    {
        List<Vector2> area = Area(HeroPosition, CastPosition[skillIndex]);
        return area;
    }

    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> areaList = CombatGridMethods.SquareAOE(characterCellCoordinate, selectedCellCoordinate);
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> availableTilesList = CombatGridMethods.AllCardinalLines(characterCellCoordinate);
        return availableTilesList;
    }
}
