using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ProBuilder;
using Zenject;

using KarenKrill.MazeGeneration;

namespace TheLabyrinth.MazeBuilding
{
    public class ArcMazeBuilder : MonoBehaviour
    {
        public int Levels = 3;
        public int TotalCellsCount
        {
            get
            {
                if (LastBuildedMaze != null)
                {
                    return LastBuildedMaze.Cells.Sum(level => level == null ? 0 : level.Length);
                }
                else
                {
                    return 0;
                }
            }
        }
        public CircularMaze LastBuildedMaze { get; private set; }

        public UnityEvent<CircularMaze> MazeGenerationFinished = new();

        [Inject]
        public void Initialize(ILogger logger)
        {
            _logger = logger;
        }
        public IEnumerator DestroyCoroutine()
        {
            if (_buildSemaphore.Wait(1))
            {
                if (LastBuildedMaze != null)
                {
                    LastBuildedMaze = null;
                    foreach (var level in _levelWalls)
                    {
                        foreach (var wall in level.AsEnumerable().Reverse())
                        {
                            Destroy(wall);
                            yield return null;
                        }
                        level.Clear();
                    }
                    _levelWalls.Clear();
                }
                _ = _buildSemaphore.Release();
            }
        }
        public Vector2 GetCellCenter(int level, int cell)
        {
            if (level == 0 && cell == 0)
            {
                return Vector2.zero;
            }
            float radius = Levels > 1 ? Mathf.Lerp(_internalRadius, _radius, level / (float)(Levels - 1)) : _radius;
            var cellsOnLevel = CircularMazeGenerator.CellsOnLevel(level);
            var nextLevelRadius = Levels > 1 ? Mathf.Lerp(_internalRadius, _radius, (level - 1) / (float)(Levels - 1)) : _internalRadius;
            var levelsDistance = radius - nextLevelRadius;
            var betweenLevelRadius = radius - levelsDistance / 2 - _wallWidth;
            var angle = 360 / (float)cellsOnLevel;
            var wallAngle = Mathf.Lerp(-180, 180, (cellsOnLevel - cell) / (float)(cellsOnLevel));
            wallAngle -= angle / 2;
            var wallAngleRadians = Mathf.Deg2Rad * wallAngle;
            return new Vector2(betweenLevelRadius * Mathf.Cos(wallAngleRadians), betweenLevelRadius * Mathf.Sin(wallAngleRadians));
        }
        public IEnumerator BuildCoroutine()
        {
            if (_buildSemaphore.Wait(1))
            {
                CircularMazeGenerator circuitMazeGenerator = new(_logger);
                LastBuildedMaze = circuitMazeGenerator.Generate(Levels);
                yield return InstantiateMaze();
                MazeGenerationFinished.Invoke(LastBuildedMaze);
                _ = _buildSemaphore.Release();
            }
        }
        public IEnumerator RebuildCoroutine()
        {
            yield return DestroyCoroutine();
            yield return BuildCoroutine();
        }

        [SerializeField]
        private Material _material;
        [SerializeField]
        private float _radius = 10, _internalRadius = 2;
        [SerializeField]
        private float _height = 2;
        [SerializeField]
        private float _minArcSegmentLength = 10f;
        [SerializeField]
        private float _wallWidth = 0.2f;

        private ILogger _logger;
        private readonly List<List<GameObject>> _levelWalls = new();
        private readonly SemaphoreSlim _buildSemaphore = new(1, 1);

        private IEnumerator InstantiateMaze()
        {
            _levelWalls.AddRange(LastBuildedMaze.Cells.Select(_ => new List<GameObject>()));
            foreach (var level in LastBuildedMaze.Cells.Reverse())
            {
                foreach (var cell in level)
                {
                    InstantiateCell(cell);
                    yield return null;
                }
            }
        }
        private void InstantiateCell(CircularMazeCell cell)
        {
            float radius = Levels > 1 ? Mathf.Lerp(_internalRadius, _radius, cell.Level / (float)(Levels - 1)) : _radius;
            var cellsOnNextLevel = CircularMazeGenerator.CellsOnLevel(cell.Level + 1 >= Levels ? cell.Level : cell.Level + 1);
            var cellsOnLevel = CircularMazeGenerator.CellsOnLevel(cell.Level);
            float angle = 360 / (float)cellsOnNextLevel;
            float width = _wallWidth;
            float depth = _height;
            float arcLength = angle * radius;
            int arcSegments = Mathf.RoundToInt(arcLength / _minArcSegmentLength);
            if (arcSegments < 2)
            {
                arcSegments = 2;
            }
            int radialCuts = arcSegments + 1; // think about arch, if you don't understand +1

            var nextLevelRadius = Levels > 1 ? Mathf.Lerp(_internalRadius, _radius, (cell.Level - 1) / (float)(Levels - 1)) : _internalRadius;
            var levelsDistance = radius - nextLevelRadius;
            var betweenLevelRadius = radius - levelsDistance / 2 - width;

            for (int i = 0; i < cell.FrontWalls.Count; i++)
            {
                if (cell.FrontWalls[i])
                {
                    var wallAngle = Mathf.Lerp(-180, 180, (cellsOnNextLevel - (cell.Cell * cell.FrontWalls.Count + i)) / (float)cellsOnNextLevel);
                    InstantiateArcWall(radius, angle, width, depth, radialCuts, wallAngle, cell, i);
                }
            }
            if (cell.LeftWall)
            {
                var wallAngle = Mathf.Lerp(-180, 180, (cellsOnLevel - cell.Cell) / (float)(cellsOnLevel));
                var wallAngleRadians = Mathf.Deg2Rad * wallAngle;
                InstantiateBoxWall(levelsDistance, betweenLevelRadius, wallAngleRadians, wallAngle, cell, true);
            }
            if (cell.RightWall)
            {
                var lerp = (cellsOnLevel - (cell.Cell + 1) % cellsOnLevel) / (float)(cellsOnLevel);
                var wallAngle = Mathf.Lerp(-180, 180, lerp);
                var wallAngleRadians = Mathf.Deg2Rad * wallAngle;
                InstantiateBoxWall(levelsDistance, betweenLevelRadius, wallAngleRadians, wallAngle, cell, false);
            }
        }
        private void InstantiateArcWall(float radius, float angle, float width, float depth, int radialCuts, float angleRotation, CircularMazeCell cell, int frontWallIndex)
        {
            var insideFaces = true;
            var outsideFaces = true;
            var frontFaces = true;
            var backFaces = true;
            var endCaps = true;
            var proBuilderMesh = ShapeGenerator.GenerateArch(PivotLocation.Center, angle, radius, width, depth, radialCuts, insideFaces, outsideFaces, frontFaces, backFaces, endCaps);
            var gameObject = proBuilderMesh.gameObject;
            gameObject.transform.parent = transform;
            gameObject.transform.Rotate(new Vector3(90, angle, angleRotation));
            var cos = Mathf.Cos(Mathf.Deg2Rad * (angleRotation - angle / 2f));
            var sin = Mathf.Sin(Mathf.Deg2Rad * (angleRotation - angle / 2f));
            //var archCenterDistance = angle == 180 ? radius/2 : radius * Mathf.Sqrt(0.5f); // not works for angle > 180
            //gameObject.transform.localPosition = new Vector3(archCenterDistance * cos, _height / 2, archCenterDistance * sin);
            //var h = (radius - width) * (1 - Mathf.Cos(Mathf.Deg2Rad * angle / 2f));
            //var h = Mathf.Tan(Mathf.Deg2Rad * angle / 2f) * radius / Mathf.Sqrt(2);
            var newRadius = (radius - width / 2) * Mathf.Cos(Mathf.Deg2Rad * angle / 2f);
            gameObject.transform.localPosition = new Vector3(newRadius * cos, _height / 2, newRadius * sin);
            gameObject.name = $"ArcL{cell.Level}:C{cell.Cell}:R{radius:#.##}A{angleRotation:#.##}(FrontWall{frontWallIndex})";
            var meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.material = _material;
            gameObject.AddComponent<MeshCollider>();
            _levelWalls[cell.Level].Add(gameObject);
        }
        private void InstantiateBoxWall(float levelsDistance, float betweenLevelRadius, float wallAngleRadians, float wallAngle, CircularMazeCell cell, bool isLeft)
        {
            var wallMesh = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(levelsDistance, _height, _wallWidth));
            var wallGameObject = wallMesh.gameObject;
            wallGameObject.transform.parent = transform;
            wallGameObject.transform.localPosition = new Vector3(betweenLevelRadius * Mathf.Cos(wallAngleRadians), _height / 2, betweenLevelRadius * Mathf.Sin(wallAngleRadians));
            wallGameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, -wallAngle, 0));
            wallGameObject.name = $"WallL{cell.Level}C{cell.Cell}A{wallAngle:#.##}({(isLeft ? "Left" : "Right")})";
            var wallMeshRenderer = wallGameObject.GetComponent<MeshRenderer>();
            wallMeshRenderer.material = _material;
            _ = wallGameObject.AddComponent<MeshCollider>();
            _levelWalls[cell.Level].Add(wallGameObject);
        }
    }
}