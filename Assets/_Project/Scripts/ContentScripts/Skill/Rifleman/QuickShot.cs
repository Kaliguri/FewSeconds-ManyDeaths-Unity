using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;

[Serializable]
public class QuickShot : SkillScript
{
    [Header("Quick Shot")]

    [Header("Stats")]
    [SerializeField] float Damage = 20f;
    [SerializeField] float timeBeforeShot = 1f;


    [Header("Visual")]
    [SerializeField] Vector2 spawnModification;
    [SerializeField] float shotSpeed = 5f;


    [Header("Prefabs")]
    [SerializeField] GameObject QuickShotPrefab;
    

    [Header("SFX")]
    [SerializeField] SoundEvent hitSFX;

    [Header("Prefabs")]
    [SerializeField] GameObject HitVFXPrefab;


    private ShotsManager shotsManager => GameObject.FindObjectOfType<ShotsManager>();

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);
        CastFX();

        CastQuickShot(playerID);
    }

    protected override void CastFX()
    {
        CastVFX(new List<Vector2> { ActualHeroPosition }, CastVFXPrefab);
        if (castSFX != null) castSFX.Play(combatPlayerDataInStage.transform);
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
            SpawnSkillObjects(SelectedCellCoordinate.ToList(), AreaVFXPrefab);
            MonoInstance.instance.StartCoroutine(ShotMovement(playerID));
        }
        else CastEnd();
    }

    private IEnumerator ShotMovement(int playerID)
    {
        yield return new WaitForSeconds(timeBeforeShot);
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
        List<Vector2> areaAround = GridAreaMethods.SquareAOE(ActualHeroPosition, ActualHeroPosition, 1, true);

        for (int i = 0; i < areaAround.Count; i++)
        {
            Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(areaAround[i] + mapClass.tileZero);
            if (mapClass.gameplayTilemap.HasTile(tile))
            {
                List<MapObject> mapObjects = GetObjectsFromPoint(areaAround[i]);
                for (int j = 0; j < mapObjects.Count; j++)
                {
                    if (mapObjects[j] is Hero) return true;
                }
            }
        }

        return false;
    }

    private void ApplayDamage(int playerID, float damage)
    {
        foreach (CombatObject combatObject in GetAffectedCombatObjectList())
        {
            CombatMethods.ApplayDamage(damage, GetHeroCombatObject(playerID), combatObject);
            if (hitSFX != null) hitSFX.Play(combatPlayerDataInStage.transform);
        }

        CastVFX(SelectedCellCoordinate.ToList(), HitVFXPrefab);
    }
}
