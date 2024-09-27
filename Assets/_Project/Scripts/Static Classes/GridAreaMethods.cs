using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridAreaMethods
{
    public enum figs
    {
        Line,
        Diagonal,
        AllCardinalLines,
        AllDiagonalLines,
        Square,
        HorseCell,
        CircleAOE
    }
    private static List<Vector2> IncorrectlyCell()
    {
        Debug.Log("Incorrectly Cell");
        return null;
    }
    public static List<Vector2> CombineLists(List<List<Vector2>> Lists)
    {
        List<Vector2> result = new();

        foreach (List<Vector2> list in Lists)
            result.AddRange(list);

        return result.Distinct().ToList();
    }
    public static List<Vector2> CoordinateLine(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int width = 1, int maxDistance = 100)
    {
        if(characterCellCoordinate == selectedCellCoordinate) return IncorrectlyCell();
        List<Vector2> line = new();

        bool horisontal = characterCellCoordinate.y == selectedCellCoordinate.y;
        bool vertical = characterCellCoordinate.x == selectedCellCoordinate.x;

        if (!horisontal && !vertical) return IncorrectlyCell();

        int t = (horisontal) ? (int)characterCellCoordinate.x : (int)characterCellCoordinate.y;
        int target = (horisontal) ? (int)selectedCellCoordinate.x : (int)selectedCellCoordinate.y;
        bool right = t < target;

        while (line.Count < maxDistance)
        {
            t += (right) ? 1 : -1;
            line.Add(new Vector2((horisontal) ? t : characterCellCoordinate.x, (horisontal) ? characterCellCoordinate.y : t));
            if(line.Count >= maxDistance) break;
        }

        List<Vector2> result = new();
        for (int i = 0; i < width; i++)
        {
            result.AddRange(line.Select(v => {
                if (horisontal) v.y += Mathf.Floor(1 - (width / 2f)) + i;
                else v.x += Mathf.Floor(1 - (width / 2f)) + i;
                return v; }).ToArray());
        }

        return result;
    }

    public static List<Vector2> AllCardinalLines(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int width = 1, int maxDistance = 100)
    {
        List<List<Vector2>> result = new();

        Vector2 target = new(characterCellCoordinate.x + maxDistance, characterCellCoordinate.y);
        result.Add(CoordinateLine(characterCellCoordinate, target, width, maxDistance));

        target = new(characterCellCoordinate.x - maxDistance, characterCellCoordinate.y);
        result.Add(CoordinateLine(characterCellCoordinate, target, width, maxDistance));
        
        target = new(characterCellCoordinate.x, characterCellCoordinate.y - maxDistance);
        result.Add(CoordinateLine(characterCellCoordinate, target, width, maxDistance));
        
        target = new(characterCellCoordinate.x, characterCellCoordinate.y + maxDistance);
        result.Add(CoordinateLine(characterCellCoordinate, target, width, maxDistance));

        return CombineLists(result);
    }

    public static List<Vector2> DiagonalLine(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int width = 1, int maxDistance = 100)
    {
        if (characterCellCoordinate == selectedCellCoordinate) return IncorrectlyCell();
        List<Vector2> line = new();
        bool east = characterCellCoordinate.x < selectedCellCoordinate.x;
        bool north = characterCellCoordinate.y < selectedCellCoordinate.y;

        Vector2 d = selectedCellCoordinate - characterCellCoordinate;
        if (Mathf.Abs(d.x) != Mathf.Abs(d.y)) return IncorrectlyCell();

        Vector2 t = characterCellCoordinate;

        while (line.Count < maxDistance)
        {
            t.x += (east) ? 1 : -1;
            t.y += (north) ? 1 : -1;
            line.Add(t);
            if (line.Count >= maxDistance) break;
        }

        List<Vector2> result = new();
        for (int i = 0; i < width; i++)
        {
            result.AddRange(line.Select(v => {
                v.y += Mathf.Floor(1 - (width / 2f)) + i;
                return v;
            }).ToArray());
        }

        return result;
    }

    public static List<Vector2> AllDiagonalLines(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int width = 1, int maxDistance = 100)
    {
        List<List<Vector2>> result = new();

        Vector2 target = new(characterCellCoordinate.x + maxDistance, characterCellCoordinate.y+maxDistance);
        result.Add(DiagonalLine(characterCellCoordinate, target, width));

        target = new(characterCellCoordinate.x + maxDistance, characterCellCoordinate.y-maxDistance);
        result.Add(DiagonalLine(characterCellCoordinate, target, width));

        target = new(characterCellCoordinate.x-maxDistance, characterCellCoordinate.y - maxDistance);
        result.Add(DiagonalLine(characterCellCoordinate, target, width));

        target = new(characterCellCoordinate.x - maxDistance, characterCellCoordinate.y + maxDistance);
        result.Add(DiagonalLine(characterCellCoordinate, target, width));

        return CombineLists(result);
    }

    public static List<Vector2> SquareAOE(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int radius = 1, bool noCharacterCell = false)
    {
        List<Vector2> result = new();

        int y = (int)selectedCellCoordinate.y - radius;
        while (y <= (int)selectedCellCoordinate.y + radius)
        {
            int x = (int)selectedCellCoordinate.x - radius;
            while (x <= (int)selectedCellCoordinate.x + radius)
            {
                result.Add(new Vector2(x, y));
                x++;
            }
            y++;
        }
        if (noCharacterCell) result.Remove(characterCellCoordinate);
        return result;
    }
    public static List<Vector2> Perforation(List<Vector2> basis, List<Vector2> perforate)
    { 
        List<Vector2> result = basis;
        foreach (var v in perforate) result.Remove(v);

        return result;
    }
    public static List<Vector2> HorseCell(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, bool allLines = false)
    {
        List<Vector2> result = AllHorseCells(characterCellCoordinate);

        if (allLines) return result;
        else if (!result.Contains(selectedCellCoordinate)) return IncorrectlyCell();
        else return new List <Vector2> { selectedCellCoordinate }; 
    }
    public static List<Vector2> AllHorseCells(Vector2 characterCellCoordinate)
    {
        List<Vector2> result = new() {
            new Vector2(characterCellCoordinate.x + 1, characterCellCoordinate.y + 2),
            new Vector2(characterCellCoordinate.x - 1, characterCellCoordinate.y + 2),

            new Vector2(characterCellCoordinate.x + 1, characterCellCoordinate.y - 2),
            new Vector2(characterCellCoordinate.x - 1, characterCellCoordinate.y - 2),
            
            new Vector2(characterCellCoordinate.x + 2, characterCellCoordinate.y - 1),
            new Vector2(characterCellCoordinate.x + 2, characterCellCoordinate.y + 1),
            
            new Vector2(characterCellCoordinate.x - 2, characterCellCoordinate.y - 1),
            new Vector2(characterCellCoordinate.x - 2, characterCellCoordinate.y + 1) };

        return result;
    }

    public static List<Vector2> CircleAOE(Vector2 characterCellCoordinate, Vector2 selectedCellCoordinate, int radius = 1, bool noCharacterCell = false)
    {
        List<List<Vector2>> t = new() { new() { selectedCellCoordinate } };

        if (radius % 2 == 0) t.Add(SquareAOE(characterCellCoordinate, selectedCellCoordinate, radius/2));

        for (int i = 0; i < radius; i += 2)
            t.Add(AllCardinalLines(selectedCellCoordinate, selectedCellCoordinate, i + 1, radius - i/2));

        List<Vector2> result = CombineLists(t);

        if(noCharacterCell) result.Remove(characterCellCoordinate);

        return result;
    }
}
