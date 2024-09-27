using System.Collections.Generic;
using UnityEngine;

public class DamageExampleSkill : SkillScript
{
    public float Damage;
    
    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] castPosition, int skillIndex = 0)
    {
        
    }

    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
 
        return null;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, int skillIndex = 0)
    {

        return null;
    }
}
