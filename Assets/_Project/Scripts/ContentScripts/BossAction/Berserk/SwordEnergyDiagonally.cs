using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sonity;
using UnityEngine;

[Serializable]
public class SwordEnergyDiagonally : BossActionScript
{
    [Title("Stats")]
    [SerializeField] int damage = 25;
    

    [Title("Visual")]
    [SerializeField] float sliceSpeed = 1f;


    [Title("Prefabs")]
    [SerializeField] GameObject slicePrefab;


    [Title("SFX")]
    [SerializeField] SoundEvent castSFX;
    [SerializeField] SoundEvent hitSFX;
    
    private int sliceCount = 0;
    private enum DiagonalDirection { UpRight, UpLeft, DownRight, DownLeft }

    public override void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints, act);
        castSFX.Play(bossManager.transform);

        Debug.Log("Cast Berserk Sword Energy Diagonally!");
        CastSwordEnergyRectangulare();
    }

    public override List<Vector2> GetCastPoint(int act)
    {
        List<DiagonalDirection> directions = GenerateDirections();
        List<Vector2> castPoints = new() { bossManager.CurrentCoordinates };
        for (int i = 0; i < directions.Count; i++)
        {
            castPoints.Add(GetDiagonalBoundaryTile(bossManager.CurrentCoordinates, directions[i]));
        }
        return castPoints;
    }

    private void CastSwordEnergyRectangulare()
    {
        sliceCount = TargetPoints.Count - 1;
        Vector2 castBossPosition = TargetPoints[0];
        if (castBossPosition != bossManager.CurrentCoordinates) CorrectTargetPoints(); 

        for (int i = 1; i < TargetPoints.Count; i++)
        {
            List<Vector2> targetLine = GridAreaMethods.DiagonalLine(bossManager.CurrentCoordinates, TargetPoints[i]);
            MonoInstance.instance.StartCoroutine(SliceMovement(targetLine));
        }
    }

    private void CorrectTargetPoints()
    {
        Vector2 correctingVector = bossManager.CurrentCoordinates - TargetPoints[0];
        for (int i = 1; i < TargetPoints.Count; i++) TargetPoints[i] += correctingVector;
    }

    private IEnumerator SliceMovement(List<Vector2> targetLine)
    {
        if (targetLine == null || targetLine.Count == 0) yield break;

        Vector2 startPosition = targetLine[0] + mapClass.tileZero;
        GameObject slice = MonoInstance.Instantiate(slicePrefab, startPosition, Quaternion.identity);

        for (int i = 0; i < targetLine.Count; i++)
        {
            Vector3Int tile = mapClass.gameplayTilemap.WorldToCell(targetLine[i] + mapClass.tileZero);
            if (!mapClass.gameplayTilemap.HasTile(tile)) break;
            Vector2 targetPosition = targetLine[i] + mapClass.tileZero;

            while (Vector2.Distance(slice.transform.position, targetPosition) > 0.01f)
            {
                slice.transform.position = Vector2.MoveTowards(slice.transform.position, targetPosition, sliceSpeed * Time.deltaTime);
                yield return null;
            }

            slice.transform.position = targetPosition;

            DamageEveryOneInTiles(new List<Vector2> { targetPosition - mapClass.tileZero }, damage, hitSFX);
        }

        MonoInstance.Destroy(slice);

        sliceCount--;

        if (sliceCount <= 0) CastEnd();
    }


    private Vector2 GetDiagonalBoundaryTile(Vector2 fromTile, DiagonalDirection direction)
    {
        switch (direction)
        {
            case DiagonalDirection.UpRight:
                return fromTile + new Vector2(1, 1);

            case DiagonalDirection.UpLeft:
                return fromTile + new Vector2(-1, 1);

            case DiagonalDirection.DownRight:
                return fromTile + new Vector2(1, -1);

            case DiagonalDirection.DownLeft:
                return fromTile + new Vector2(-1, -1);

            default:
                return fromTile;
        }
    }


    #region DiagonalDirection

    private List<DiagonalDirection> GenerateDirections()
    {
        List<DiagonalDirection> directions = new();

        switch (act)
        {
            case 0:
                directions.Add(GetRandomDirection());
                break;

            case 1:
                DiagonalDirection randomDirection = GetRandomDirection();
                directions.Add(randomDirection);
                directions.Add(GetOppositeDiagonalDirection(randomDirection));
                break;

            case 2:
                directions.AddRange(new DiagonalDirection[] { DiagonalDirection.UpRight, DiagonalDirection.UpLeft, DiagonalDirection.DownRight, DiagonalDirection.DownLeft });
                break;

            default:
                directions.AddRange(new DiagonalDirection[] { DiagonalDirection.UpRight, DiagonalDirection.UpLeft, DiagonalDirection.DownRight, DiagonalDirection.DownLeft });
                break;
        }
        return directions;
    }

    private DiagonalDirection GetRandomDirection()
    {
        return (DiagonalDirection)UnityEngine.Random.Range(0, 4);
    }

    private DiagonalDirection GetOppositeDiagonalDirection(DiagonalDirection dir)
    {
        switch (dir)
        {
            case DiagonalDirection.UpRight: return DiagonalDirection.DownLeft;
            case DiagonalDirection.UpLeft: return DiagonalDirection.DownRight;
            case DiagonalDirection.DownRight: return DiagonalDirection.UpLeft;
            case DiagonalDirection.DownLeft: return DiagonalDirection.UpRight;
            default: return DiagonalDirection.UpRight;
        }
    }

    #endregion
}
