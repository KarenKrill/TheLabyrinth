using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;
    [SerializeField]
    private int _mazeWidth, _mazeDepth;
    private MazeCell[,] _mazeCells;
    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell cell)
    {
        var x = (int)cell.transform.position.x;
        var z = (int)cell.transform.position.z;
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
    private MazeCell GetNextUnvisitedCell(MazeCell cell)
    {
        var unvisitedCells = GetUnvisitedCells(cell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }
    private void ClearWalls(MazeCell prevCell, MazeCell currCell)
    {
        if (prevCell == null)
        {
            return;
        }
        if (prevCell.transform.position.x < currCell.transform.position.x)
        {
            prevCell.ClearWall(MazeCellWallType.Right);
            currCell.ClearWall(MazeCellWallType.Left);
        }
        else if(prevCell.transform.position.x > currCell.transform.position.x)
        {
            prevCell.ClearWall(MazeCellWallType.Left);
            currCell.ClearWall(MazeCellWallType.Right);
        }
        else if (prevCell.transform.position.z < currCell.transform.position.z)
        {
            prevCell.ClearWall(MazeCellWallType.Front);
            currCell.ClearWall(MazeCellWallType.Back);
        }
        else if (prevCell.transform.position.z > currCell.transform.position.z)
        {
            prevCell.ClearWall(MazeCellWallType.Back);
            currCell.ClearWall(MazeCellWallType.Front);
        }
    }
    private IEnumerator GenerateMaze(MazeCell prevCell, MazeCell currCell)
    {
        yield return new WaitForSeconds(0.1f);
        currCell.Visit();
        ClearWalls(prevCell, currCell);
        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currCell);
            if (nextCell != null)
            {
                yield return GenerateMaze(currCell, nextCell);
            }
        }
        while (nextCell != null);
        yield return new WaitForSeconds(0.2f);
    }
    private IEnumerator Start()
    {
        _mazeCells = new MazeCell[_mazeWidth, _mazeDepth];
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeCells[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
            }
        }
        yield return GenerateMaze(null, _mazeCells[0, 0]);
    }
}
