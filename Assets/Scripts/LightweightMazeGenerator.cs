using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightweightMazeCell
{
    public bool IsVisited { get; private set; } = false;
    public bool LeftWall { get; private set; } = true;
    public bool RightWall { get; private set; } = true;
    public bool BackWall { get; private set; } = true;
    public bool FrontWall { get; private set; } = true;
    public void ClearLeftWall() => LeftWall = false;
    public void ClearRightWall() => RightWall = false;
    public void ClearBackWall() => BackWall = false;
    public void ClearFrontWall() => FrontWall = false;
    public void Visit() => IsVisited = true;
    public int X { get; private set; }
    public int Y { get; private set; }
    public LightweightMazeCell(int x, int y)
    {
        X = x;
        Y = y;
    }
}
public class LightweightMazeGenerator
{
    private LightweightMazeCell[,] _mazeCells;
    private int _mazeWidth, _mazeDepth;
    private void ClearWalls(LightweightMazeCell prevCell, LightweightMazeCell currCell)
    {
        if (prevCell.X < currCell.X)
        {
            prevCell.ClearRightWall();
            currCell.ClearLeftWall();
        }
        else if (prevCell.X > currCell.X)
        {
            prevCell.ClearLeftWall();
            currCell.ClearRightWall();
        }
        else if (prevCell.Y < currCell.Y)
        {
            prevCell.ClearFrontWall();
            currCell.ClearBackWall();
        }
        else if (prevCell.Y > currCell.Y)
        {
            prevCell.ClearBackWall();
            currCell.ClearFrontWall();
        }
    }
    private IEnumerable<LightweightMazeCell> GetUnvisitedCells(LightweightMazeCell cell)
    {
        var x = cell.X;
        var z = cell.Y;
        if (x + 1 < _mazeWidth)
        {
            var neighbourCell = _mazeCells[x + 1, z];
            if (!neighbourCell.IsVisited)
            {
                yield return neighbourCell;
            }
        }
        if (x - 1 >= 0)
        {
            var neighbourCell = _mazeCells[x - 1, z];
            if (!neighbourCell.IsVisited)
            {
                yield return neighbourCell;
            }
        }
        if (z + 1 < _mazeDepth)
        {
            var neighbourCell = _mazeCells[x, z + 1];
            if (!neighbourCell.IsVisited)
            {
                yield return neighbourCell;
            }
        }
        if (z - 1 >= 0)
        {
            var neighbourCell = _mazeCells[x, z - 1];
            if (!neighbourCell.IsVisited)
            {
                yield return neighbourCell;
            }
        }
    }
    private LightweightMazeCell GetNextUnvisitedCell(LightweightMazeCell cell)
    {
        var unvisitedCells = GetUnvisitedCells(cell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }
    private void GenerateMaze(LightweightMazeCell prevCell, LightweightMazeCell currCell)
    {
        currCell.Visit();
        if (prevCell != null)
        {
            ClearWalls(prevCell, currCell);
        }
        LightweightMazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currCell);
            if (nextCell != null)
            {
                GenerateMaze(currCell, nextCell);
            }
        }
        while (nextCell != null);
    }
    public LightweightMazeCell[,] Generate(int width, int depth)
    {
        _mazeCells = new LightweightMazeCell[width, depth];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                _mazeCells[x, y] = new(x, y);
            }
        }
        _mazeWidth = width;
        _mazeDepth = depth;
        GenerateMaze(null, _mazeCells[0, 0]);
        return _mazeCells;
    }
}
