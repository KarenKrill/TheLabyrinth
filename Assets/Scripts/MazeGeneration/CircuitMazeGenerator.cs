using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable

namespace KarenKrill.MazeGeneration
{
    public class TreeNode<T>
    {
        public T Data { get; private set; }
        public bool IsVisited { get; private set; } = false;
        public List<TreeNode<T>> Neighbours { get; private set; } = new();
        public TreeNode(T data)
        {
            Data = data;
        }
        public void BindWith(TreeNode<T> neighbour)
        {
            CircuitMazeCell cell = (Data as CircuitMazeCell)!;
            CircuitMazeCell neighbourCell = (neighbour.Data as CircuitMazeCell)!;
            var cellNeighbours = Neighbours.Select(node => (node.Data as CircuitMazeCell)!);
            var neighbourNeighbours = neighbour.Neighbours.Select(node => (node.Data as CircuitMazeCell)!);
            if (neighbourNeighbours.Where(c => c.Level == cell.Level && c.Cell == cell.Cell).Any())
            {
                //Debug.LogWarning($"BindWith:L{cell.Level}C{cell.Cell} already neigh of L{neighbourCell.Level}C{neighbourCell.Cell}");
            }
            if (cellNeighbours.Where(c => c.Level == neighbourCell.Level && c.Cell == neighbourCell.Cell).Any())
            {
                Debug.LogWarning($"BindWith:L{neighbourCell.Level}C{neighbourCell.Cell} already neigh of L{cell.Level}C{cell.Cell}");
            }
            Neighbours.Add(neighbour);
            neighbour.Neighbours.Add(this);
        }
        public void Visit() => IsVisited = true;
    }
    public class Tree<T>
    {
        public TreeNode<T> RootNode { get; private set; }
        public Tree(T data)
        {
            RootNode = new(data);
        }
        public Tree(TreeNode<T> rootNode)
        {
            RootNode = rootNode;
        }

        private IEnumerable<TreeNode<T>> GetUnvisitedCells(TreeNode<T> cell)
        {
            foreach (var neighbour in cell.Neighbours)
            {
                if (!neighbour.IsVisited)
                {
                    yield return neighbour;
                }
            }
        }
        private TreeNode<T> GetNextUnvisitedCell(TreeNode<T> node)
        {
            var unvisitedCells = GetUnvisitedCells(node);
            return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
        }
        public void DepthFirstSearch(TreeNode<T> currCell, ref List<T> values)
        {
            currCell.Visit();
            if (currCell.Data is CircuitMazeCell cell)
            {
                var neighbours = string.Join(',', currCell.Neighbours.Select(node =>
                {
                    if (node.Data is CircuitMazeCell cellNode)
                    {
                        return $"L{cellNode.Level}C{cellNode.Cell}({(node.IsVisited ? "V" : "U")})";
                    }
                    else return "NoMazeCell";
                }));
                Debug.Log($"Added L{cell.Level}C{cell.Cell} Neighbours: {neighbours}");
            }
            TreeNode<T> nextCell;
            do
            {
                values.Add(currCell.Data);
                nextCell = GetNextUnvisitedCell(currCell);
                if (nextCell != null)
                {
                    DepthFirstSearch(nextCell, ref values);
                }
            }
            while (nextCell != null);
        }
        public List<T> DepthFirstSearch()
        {
            List<T> path = new();
            DepthFirstSearch(RootNode, ref path);
            return path;
        }
    }

    public class CircuitMazeCell
    {
        private List<bool> _frontWalls;
        public IReadOnlyList<bool> FrontWalls => _frontWalls;
        public bool LeftWall { get; private set; } = true;
        public bool RightWall { get; private set; } = true;
        public bool BackWall { get; private set; } = true;
        public int Level { get; private set; }
        public int Cell { get; private set; }
        public CircuitMazeCell(int level, int cell, int frontWallsCount)
        {
            Level = level;
            Cell = cell;
            _frontWalls = new List<bool>(frontWallsCount);
            for (int i = 0; i < frontWallsCount; i++)
            {
                _frontWalls.Add(true);
            }
        }
        public void BreakFrontWall(int wallIndex) => _frontWalls[wallIndex] = false;
        public void BreakLeftWall() => LeftWall = false;
        public void BreakRightWall() => RightWall = false;
        public void BreakBackWall() => BackWall = false;
    }
    public class CircuitMaze
    {
        public CircuitMazeCell[][] Cells { get; set; }
        public CircuitMazeCell ExitCell { get; set; }
        public CircuitMaze(CircuitMazeCell[][] cells, CircuitMazeCell exitCell)
        {
            Cells = cells;
            ExitCell = exitCell;
        }
    }
    public class CircuitMazeGenerator
    {
        public static int CellsOnLevel(int level) => level == 0 ? 1 : (int)Mathf.Pow(2, Mathf.Floor(Mathf.Log(level + 1, 2)) + 2);
        public CircuitMaze Generate(int levels, int startCells)
        {
            // Caching
            int[] cellsOnLevelCache = new int[levels];
            for(int level = 0; level < levels; level++)
            {
                cellsOnLevelCache[level] = CellsOnLevel(level);
            }

            // Nodes & Cells creation:
            CircuitMazeCell[][] cells = new CircuitMazeCell[levels][];
            TreeNode<CircuitMazeCell>[][] cellNodes = new TreeNode<CircuitMazeCell>[levels][];
            for (int level = 0; level < levels; level++)
            {
                int cellsOnLevel = cellsOnLevelCache[level];
                int cellsOnNextLevel = level + 1 < levels ? cellsOnLevelCache[level + 1] : cellsOnLevel;
                int nextLevelCellsPerCell = cellsOnNextLevel / cellsOnLevel;
                int wallsOnCell = nextLevelCellsPerCell;
                cellNodes[level] = new TreeNode<CircuitMazeCell>[cellsOnLevel];
                cells[level] = new CircuitMazeCell[cellsOnLevel];
                for (int cellIndex = 0; cellIndex < cellsOnLevel; cellIndex++)
                {
                    var cell = new CircuitMazeCell(level, cellIndex, wallsOnCell);
                    cellNodes[level][cellIndex] = new(cell);
                    cells[level][cellIndex] = cell;
                }
            }

            // Center cell inner walls destroing (center cell only have front walls):
            var centerCell = cells[0][0];
            centerCell.BreakLeftWall();
            centerCell.BreakRightWall();
            centerCell.BreakBackWall();

            // Tree building:
            Tree<CircuitMazeCell> tree = new(cellNodes[0][0]);
            var rootNode = tree.RootNode;
            for (int cell = 0; cell < cellsOnLevelCache[1]; cell++)
            {
                rootNode.BindWith(cellNodes[1][cell]);
            }
            for (int level = 1; level < levels; level++)
            {
                int cellsOnLevel = cellNodes[level].Length;
                bool isNotLastLevel = level + 1 < levels;
                int cellsOnNextLevel = isNotLastLevel ? cellNodes[level + 1].Length : 0;
                bool isNextLevelExtended = isNotLastLevel && cellsOnNextLevel > cellsOnLevel;
                int cellsPerPrevLevelCell = cellsOnNextLevel / cellsOnLevel;
                for (int cell = 0; cell < cellsOnLevel; cell++)
                {
                    var cellNode = cellNodes[level][cell];
                    var nextCellNode = cellNodes[level][(cell + 1) % cellsOnLevel];
                    //Debug.Log($"Bind1 L{cellNode.Data.Level}C{cellNode.Data.Cell} with L{nextCellNode.Data.Level}C{nextCellNode.Data.Cell}");
                    cellNode.BindWith(nextCellNode);
                    if (isNotLastLevel)
                    {
                        if (isNextLevelExtended)
                        {
                            var nextLevelCellsOffset = cell * cellsPerPrevLevelCell;
                            for (int i = 0; i < cellsPerPrevLevelCell; i++)
                            {
                                //Debug.Log($"Bind2 L{cellNode.Data.Level}C{cellNode.Data.Cell} with L{level + 1}C{nextLevelCellsOffset + i}");
                                cellNode.BindWith(cellNodes[level + 1][nextLevelCellsOffset + i]);
                            }
                        }
                        else
                        {
                            //Debug.Log($"Bind3 L{cellNode.Data.Level}C{cellNode.Data.Cell} with L{level + 1}C{cell}");
                            cellNode.BindWith(cellNodes[level + 1][cell]);
                        }
                    }
                }
            }

            var dfsPath = tree.DepthFirstSearch();

            Debug.Log(string.Join("->", dfsPath.Select(cell => $"L{cell.Level}C{cell.Cell}")));

            // Walls destroing:
            for (int i = 1; i < dfsPath.Count; i++)
            {
                var prevCell = dfsPath[i - 1];
                var cell = dfsPath[i];
                int cellsOnPrevLevel = cellsOnLevelCache[prevCell.Level];
                int cellsOnLevel = cellsOnLevelCache[cell.Level];
                int levelCellsPerCell = cellsOnLevel > cellsOnPrevLevel ? cellsOnLevel / cellsOnPrevLevel : cellsOnPrevLevel / cellsOnLevel;
                if (cell.Level != prevCell.Level)
                {
                    if (cell.Level > prevCell.Level)
                    {
                        cell.BreakBackWall();
                        prevCell.BreakFrontWall(cell.Cell % levelCellsPerCell);
                    }
                    else
                    {
                        cell.BreakFrontWall(prevCell.Cell % levelCellsPerCell);
                        prevCell.BreakBackWall();
                    }
                }
                else if (cell.Cell == prevCell.Cell + 1 || (cell.Cell == 0 && prevCell.Cell > 1))
                {
                    cell.BreakLeftWall();
                    prevCell.BreakRightWall();
                }
                else
                {
                    cell.BreakRightWall();
                    prevCell.BreakLeftWall();
                }
            }

            var uniquePath = new List<CircuitMazeCell>();
            foreach (var node in dfsPath)
            {
                if (!uniquePath.Contains(node))
                {
                    uniquePath.Add(node);
                }
            }

            return new(cells, uniquePath[^1]);
        }
    }
}
