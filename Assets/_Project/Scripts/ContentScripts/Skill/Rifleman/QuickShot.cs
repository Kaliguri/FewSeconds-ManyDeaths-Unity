using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;

[Serializable]
public class QuickShot : SkillScript
{
    [Title("Quick Shot")]

    [Title("Stats")]
    [SerializeField] float Damage = 20f;



    [Title("Visual")]
    [SerializeField] Vector2 spawnModification;
    [SerializeField] float shotSpeed = 5f;


    [Title("Prefabs")]
    [SerializeField] GameObject QuickShotPrefab;
    

    [Title("SFX")]
    [SerializeField] SoundEvent castSFX;
    [SerializeField] SoundEvent hitSFX;
    
    
    private ShotsManager shotsManager => GameObject.FindObjectOfType<ShotsManager>();

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);

        CastQuickShot(playerID);
    }

    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {

        List<Vector2> areaList = new List<Vector2> { selectedCellCoordinate };
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return mapClass.AllTiles;
    }

    private void CastQuickShot(int playerID)
    {
        int shotsCount = shotsManager.GetShotsCount(playerID);

        if (shotsCount > 0)
        {
            shotsManager.SetShotsCount(playerID, shotsCount - 1);
            MonoInstance.instance.StartCoroutine(ShotMovement(playerID));
        }
    }

    private IEnumerator ShotMovement(int playerID)
    {
        GameObject shot = MonoInstance.Instantiate(QuickShotPrefab, ActualHeroPosition + mapClass.tileZero + spawnModification, Quaternion.identity);

        while (Vector2.Distance(shot.transform.position, SelectedCellCoordinate[0] + mapClass.tileZero) > 0.01f)
        {
            shot.transform.position = Vector2.MoveTowards(shot.transform.position, SelectedCellCoordinate[0] + mapClass.tileZero, shotSpeed * Time.deltaTime);
            yield return null;
        }

        MonoInstance.Destroy(shot);

        float damage = Damage;
        if (IsPlayersNear()) damage *= 1.5f;
        ApplayDamage(playerID, damage);

        CastEnd();
    }

    private bool IsPlayersNear()
    {
        List<Vector2> areaAround = GridAreaMethods.SquareAOE(ActualHeroPosition, SelectedCellCoordinate[0], 1, true);

        for (int i = 0; i < areaAround.Count; i++)
        {
            List<MapObject> mapObjects = GetObjectsFromPoint(areaAround[i]);
            for (int j = 0; j < mapObjects.Count; j++)
            {
                if (mapObjects[j] is Hero) return true;
            }
        }

        return false;
    }

    private void ApplayDamage(int playerID, float damage)
    {
        foreach (CombatObject combatObject in GetAffectedCombatObjectList())
        {
            CombatMethods.ApplayDamage(damage, GetHeroCombatObject(playerID), combatObject);
        }
    }
}
