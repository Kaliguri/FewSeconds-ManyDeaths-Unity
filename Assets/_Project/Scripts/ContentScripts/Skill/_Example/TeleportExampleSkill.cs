using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TeleportExampleSkill : SkillScript
{
    [SerializeReference, SubclassSelector]
    public List<SkillScript> skillList;
    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] castPosition, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, castPosition);

        skillList[skillIndex].Cast(heroPosition, actualHeroPosition, castPosition, skillIndex);
        
        CastEnd(true);
    }

    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> areaList = skillList[skillIndex].Area(characterCellCoordinate, selectedCellCoordinate);
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> availableTilesList = skillList[skillIndex].AvailableTiles(characterCellCoordinate, selectedCellCoordinate);
        return availableTilesList;
    }
}
