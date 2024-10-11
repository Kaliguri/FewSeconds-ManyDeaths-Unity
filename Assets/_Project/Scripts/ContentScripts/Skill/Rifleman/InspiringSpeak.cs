using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;

[Serializable]
public class InspiringSpeak : SkillScript
{
    [Header("Inspiring Speak")]
    
    [Header("Prefabs")]
    [SerializeField] GameObject InspiringSpeakPrefab;

    
    private ShotsManager shotsManager => GameObject.FindObjectOfType<ShotsManager>();

    public override void Cast(Vector2 heroPosition, Vector2 actualHeroPosition, Vector2[] selectedCellCoordinate, int playerID, int skillIndex = 0)
    {
        CastStart(heroPosition, actualHeroPosition, selectedCellCoordinate);
        CastFX();

        CastInspiringSpeak(playerID);

        CastEnd();
    }

    protected override void CastFX()
    {
        CastVFX(new List<Vector2> { ActualHeroPosition }, CastVFXPrefab);
        CastVFX(GetArea(), AreaVFXPrefab);
        if (castSFX != null) castSFX.Play(combatPlayerDataInStage.transform);
    }

    public override List<Vector2> Area(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        List<Vector2> areaList = GridAreaMethods.SquareAOE(characterCellCoordinate, characterCellCoordinate);
        return areaList;
    }

    public override List<Vector2> AvailableTiles(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int skillIndex = 0)
    {
        return mapClass.AllTiles;
    }

    private void CastInspiringSpeak(int playerID)
    {
        List<MapObject> mapObjectList = GetAffectedMapObjectList();

        List<int> playersIdList = new();
        foreach (MapObject mapObject in mapObjectList) if (mapObject is Hero) playersIdList.Add(mapObject.ID);

        for (int i = 0; i < playersIdList.Count; i++) combatPlayerDataInStage._TotalStatsList[playersIdList[i]].general.MaxEnergy += 1;

        GlobalEventSystem.SendEnergyChange();
    }
}
