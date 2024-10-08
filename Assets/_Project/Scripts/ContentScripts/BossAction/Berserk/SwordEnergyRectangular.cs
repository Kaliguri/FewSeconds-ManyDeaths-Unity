using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class SwordEnergyRectangular : BossActionScript
{
    [SerializeField] private int damage = 25;
    [SerializeField] private GameObject slicePrefab;
    [SerializeField] private float sliceSpeed = 1f;
    private int sliceCount = 0;
    private enum Direction { Up, Down, Left, Right }

    public override void Cast(List<Vector2> targetPoints, int act)
    {
        CastStart(targetPoints, act);

        Debug.Log("Cast Berserk Sword Energy Rectangular!");
        CastSwordEnergyRectangulare();
    }

    public override List<Vector2> GetCastPoint(int act)
    {
        return new List<Vector2>() { new Vector2(0, 0) };
    }

    private void CastSwordEnergyRectangulare()
    {
        List<Direction> directions = GenerateDirections();
        sliceCount = directions.Count;

        for (int i = 0; i < directions.Count; i++)
        {
            Vector2 fromTile = bossManager.CurrentCoordinates;
            Vector2 toTile = GetBoundaryTile(fromTile, directions[i]);
            List<Vector2> targetLine = GridAreaMethods.CoordinateLine(fromTile, toTile);
            MonoInstance.instance.StartCoroutine(SliceMovement(targetLine));
        }
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

            DamageEveryOneInTiles(new List<Vector2> { targetPosition - mapClass.tileZero }, damage);
        }

        MonoInstance.Destroy(slice);

        Debug.Log("DestroySlice");

        sliceCount--;

        if (sliceCount <= 0) CastEnd();
    }


    private Vector2 GetBoundaryTile(Vector2 fromTile, Direction direction)
    {
        Vector2 boundaryTile = new();

        switch (direction)
        {
            case Direction.Up:
                // ��������� ����� �� ������� �������
                boundaryTile = new Vector2(fromTile.x, 0);
                break;

            case Direction.Down:
                // ��������� ���� �� ������� �������
                boundaryTile = new Vector2(fromTile.x, mapClass.Max_B);
                break;

            case Direction.Left:
                // ��������� ����� �� ������� �������
                boundaryTile = new Vector2(0, fromTile.y);
                break;

            case Direction.Right:
                // ��������� ������ �� ������� �������
                boundaryTile = new Vector2(mapClass.Max_A, fromTile.y);
                break;
        }

        return boundaryTile;
    }

    #region Direction

    private List<Direction> GenerateDirections()
    {
        List<Direction> directions =new();

        switch (act)
        {
            case 0:
                directions.Add(GetRandomDirection());
                break;

            case 1:
                Direction randomDirection = GetRandomDirection();
                directions.Add(randomDirection);
                directions.Add(GetOppositeDirection(randomDirection));
                break;

            case 2:
                directions.AddRange(new Direction[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right });
                break;

            default:
                directions.AddRange(new Direction[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right });
                break;
        }
        return directions;
    }

    private Direction GetRandomDirection()
    {
        return (Direction)UnityEngine.Random.Range(0, 4);
    }

    private Direction GetOppositeDirection(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
            default: return Direction.Up;
        }
    }

    #endregion
}
