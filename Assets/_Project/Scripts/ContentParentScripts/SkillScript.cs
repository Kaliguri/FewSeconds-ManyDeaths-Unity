using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [Title("Prefabs")]
    [SerializeField] GameObject TilePrefab;
    [SerializeField] protected GameObject VFXPrefab;

    protected MapClass mapClass => GameObject.FindObjectOfType<MapClass>(); 
    protected BossManager bossManager => GameObject.FindObjectOfType<BossManager>();
    protected CombatPlayerDataInStage combatPlayerDataInStage => GameObject.FindObjectOfType<CombatPlayerDataInStage>();


    protected Vector2 HeroPosition;
    protected Vector2 ActualHeroPosition;
    protected Vector2[] SelectedCellCoordinate;
    protected float SkillPrefabDuration = 1f;
    protected float SkillDuration = 1f;

    public virtual SkillScript Select()
    {
        Debug.Log("Select this Skill!");
        return this;
    }

    public virtual void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);

        Debug.Log("Cast");

        CastEnd();
    }

    protected void CastStart(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate)
    {
        SetPosition(heroPosition, actualHeroPosition, selectedCellCoordinate);
        if (IsMovable && heroPosition != actualHeroPosition) ChangeSelectedCellCoordinate();
    }

    protected void SetPosition(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate)
    {
        ActualHeroPosition = actualHeroPosition;
        HeroPosition = heroPosition;
        SelectedCellCoordinate = selectedCellCoordinate;
    }

    private void ChangeSelectedCellCoordinate()
    {
        for (int i = 0; i < SelectedCellCoordinate.Length; i++)
        {
            SelectedCellCoordinate[i] += ActualHeroPosition - HeroPosition;
        }
    }

    protected void CastEnd(bool isContainerSkill = false)
    {
        if (!isContainerSkill)
        { MonoInstance.instance.Invoke(nameof(CastEndPart2), SkillDuration); }
    }

    protected void CastEndPart2() 
    {
        GlobalEventSystem.SendPlayerSkillEnd();
    }

    public virtual List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return new List<Vector2>();
    }

    protected virtual List<Vector2> GetArea(int skillIndex = 0)
    {
        List<Vector2> area = Area(HeroPosition, SelectedCellCoordinate[skillIndex]);
        return area;
    }

    public virtual List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
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

    protected List<MapObject> GetAffectedMapObjectList(int skillIndex = 0)
    {
        return mapClass.MapObjectCheck(GetArea(skillIndex)); 
    }

    protected List<MapObject> GetObjectsFromPoint(Vector2 point)
    {
        return mapClass.GetMapObjectList(point).ToList();
    }

    protected List<CombatObject> GetAffectedCombatObjectList(int skillIndex = 0)
    {
        List<MapObject> MapObjectList = GetAffectedMapObjectList(skillIndex);
        List<CombatObject> combatObjectList = new List<CombatObject>();

        foreach (MapObject mapObject in MapObjectList)
        {
            if (mapObject is Hero)
            {
                combatObjectList.Add(new HeroCombatObject(mapObject.ID, combatPlayerDataInStage));
            }
            if (mapObject is Boss)
            {
                combatObjectList.Add(new BossCombatObject(bossManager));
            }
        }

        return combatObjectList;
    }

    protected HeroCombatObject GetHeroCombatObject(int playerID)
    {
        return new HeroCombatObject(playerID, combatPlayerDataInStage);
    }
}
