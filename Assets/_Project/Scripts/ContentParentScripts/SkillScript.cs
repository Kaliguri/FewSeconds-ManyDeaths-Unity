using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SkillScript 
{
    [Header("Base")]
    public int EnergyCost;
    public int SkillCooldown;

    [Header("Other")]
    public bool IsMovable = false;
    public int TargetCount = 1;

    protected MapClass mapClass => GameObject.FindGameObjectWithTag("MapController").GetComponent<MapClass>();
    protected Vector2 HeroPosition;
    protected Vector2 ActualHeroPosition;
    protected Vector2[] CastPosition;
    protected float SkillPrefabDuration = 1f;
    protected float SkillDuration = 1f;

    public virtual SkillScript Select()
    {
        Debug.Log("Select this Skill!");
        return this;
    }

    public virtual void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] castPosition, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, castPosition);

        Debug.Log("Cast");

        CastEnd();
    }

    protected void CastStart(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] castPosition)
    {
        SetPosition(heroPosition, actualHeroPosition, castPosition);
    }

    protected void SetPosition(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] castPosition)
    {
        ActualHeroPosition = actualHeroPosition;
        HeroPosition = heroPosition;
        CastPosition = castPosition;
    }

    protected void CastEnd(bool isContainerSkill = false)
    {
        if (!isContainerSkill)
        { MonoInstance.instance.Invoke(nameof(CastEndPart2), SkillDuration); }
    }

    protected void CastEndPart2() 
    {
        GlobalEventSystem.SendPlayerActionEnd();
    }

    public virtual List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return new List<Vector2>();
    }

    public virtual List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return new List<Vector2>();
    }
    protected virtual List<Vector2> GetArea(int skillIndex)
    {
        return new List<Vector2>();
    }

    protected void SpawnSkillObjects(List<Vector2> area, GameObject Skillobject)
    {
        for (int i = 0; i < area.Count; i++)
            {
                Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(area[i] + mapClass.tileZero);
                if (mapClass.gameplayTilemap.HasTile(tile))
                {
                    GameObject affectedObject = MonoInstance.Instantiate(Skillobject, area[i] + mapClass.tileZero, Quaternion.identity);
                    MonoInstance.Destroy(affectedObject, SkillPrefabDuration);
                }
            }
    }
}
