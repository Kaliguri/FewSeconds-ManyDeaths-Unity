using System.Collections.Generic;
using UnityEngine;

public class ShieldExampleSkill : SkillScript
{
    [SerializeField] float Heal;
    [SerializeField] GameObject ExampleSkillPrefab;
    
    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] castPosition, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, castPosition);

        SpawnSkillPrefab(skillIndex);
                
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

    void SpawnSkillPrefab(int skillIndex)
    {
        SpawnSkillObjects(GetArea(skillIndex), ExampleSkillPrefab);
    }
}
