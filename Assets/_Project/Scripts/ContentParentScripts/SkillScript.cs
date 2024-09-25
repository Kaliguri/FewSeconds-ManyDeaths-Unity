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
    public bool HasConditionsForSelectedCell = false;
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

    public virtual void Cast(Vector2 HeroPosition, Vector2 ActualHeroPosition, Vector2[] CastPosition, int skillIndex = 0)
    {
        Debug.Log("Cast");
        GlobalEventSystem.SendPlayerActionEnd();
    }

    public virtual List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return new List<Vector2>();
    }

    public virtual List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, int skillIndex = 0)
    {
        return new List<Vector2>();
    }

    protected virtual void EndCast() 
    {
        Debug.Log("EndCast");
        GlobalEventSystem.SendPlayerActionEnd();
    }

    protected virtual void SpawnSkillPrefab(int skillIndex)
    {

    }

    protected virtual List<Vector2> GetArea(int skillIndex)
    {
        return new List<Vector2>();
    }
}
