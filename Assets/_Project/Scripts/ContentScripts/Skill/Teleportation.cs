using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Teleportation : SkillScript
{
    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] castPosition, int skillIndex = 0)
    {
        ActualHeroPosition = actualHeroPosition;
        HeroPosition = heroPosition;
        CastPosition = castPosition;

        MonoInstance.instance.Invoke(nameof(EndCast), SkillDuration);
    }

    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> area = new List<Vector2>();
        area.Add(characterCellCoordinate);
        return area;
    }
}
