using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TeleportExampleSubSkill1 : SkillScript
{
    [SerializeField] GameObject ExampleSkillPrefab;

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);

        SpawnSkillPrefab(skillIndex);
                
        CastEnd();
    }

    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> areaList = GridAreaMethods.SquareAOE(characterCellCoordinate, selectedCellCoordinate);
        return areaList;
    }
    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> availableTilesList = GridAreaMethods.AllCardinalLines(characterCellCoordinate, selectedCellCoordinate);
        return availableTilesList;
    }

    void SpawnSkillPrefab(int skillIndex)
    {
        SpawnSkillObjects(GetArea(skillIndex), ExampleSkillPrefab);
    }
}
