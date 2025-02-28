using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;
using Zenject;
using static Cinemachine.DocumentationSortingAttribute;

public class ArcMazeGenerator : MonoBehaviour
{
    [SerializeField]
    private float _radius = 10, _internalRadius = 2;
    [SerializeField]
    private float _height = 2;
    [SerializeField]
    private int _levels = 3;
    [SerializeField]
    private int _startCellsCount = 4;
    [SerializeField]
    private float _wallWidth = 0.2f;
    [SerializeField]
    private Material _material;
    private List<GameObject> _walls = new();
    private void InstantiateArcWall(float radius, float angle, float width, float depth, int radialCuts, float angleRotation, CircuitMazeCell cell, int frontWallIndex)
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
        var archCenterDistance = angle == 180 ? radius/2 : radius * Mathf.Sqrt(0.5f); // not works for angle > 180
        //gameObject.transform.localPosition = new Vector3(archCenterDistance * cos, _height / 2, archCenterDistance * sin);
        //var h = (radius - width) * (1 - Mathf.Cos(Mathf.Deg2Rad * angle / 2f));
        //var h = Mathf.Tan(Mathf.Deg2Rad * angle / 2f) * radius / Mathf.Sqrt(2);
        var newRadius = (radius - width / 2) * Mathf.Cos(Mathf.Deg2Rad * angle / 2f);
        gameObject.transform.localPosition = new Vector3(newRadius * cos, _height / 2, newRadius * sin);
        gameObject.name = $"ArcL{cell.Level}:C{cell.Cell}:R{radius:#.##}A{angleRotation:#.##}(FrontWall{frontWallIndex})";
        var meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = _material;
        gameObject.AddComponent<MeshCollider>();
        _walls.Add(gameObject);
    }
    private void InstantiateBoxWall(float levelsDistance, float betweenLevelRadius, float wallAngleRadians, float wallAngle, CircuitMazeCell cell, bool isLeft)
    {
        var wallMesh = ShapeGenerator.GenerateCube(PivotLocation.Center, new Vector3(levelsDistance, _height, _wallWidth));
        var wallGameObject = wallMesh.gameObject;
        wallGameObject.transform.parent = transform;
        wallGameObject.transform.localPosition = new Vector3(betweenLevelRadius * Mathf.Cos(wallAngleRadians), _height / 2, betweenLevelRadius * Mathf.Sin(wallAngleRadians));
        wallGameObject.transform.localRotation = Quaternion.Euler(new Vector3(0, -wallAngle, 0));
        wallGameObject.name = $"WallL{cell.Level}C{cell.Cell}A{wallAngle:#.##}({(isLeft ? "Left" : "Right")})";
        var wallMeshRenderer = wallGameObject.GetComponent<MeshRenderer>();
        wallMeshRenderer.material = _material;
        wallGameObject.AddComponent<MeshCollider>();
        _walls.Add(wallGameObject);
    }
    private void InstantiateCell(CircuitMazeCell cell)
    {
        float radius = _levels > 1 ? Mathf.Lerp(_internalRadius, _radius, cell.Level / (float)(_levels - 1)) : _radius;
        var cellsOnNextLevel = CircuitMazeGenerator.CellsOnLevel(cell.Level + 1 >= _levels ? cell.Level : cell.Level + 1);
        var cellsOnLevel = CircuitMazeGenerator.CellsOnLevel(cell.Level);
        float angle = 360 / (float)cellsOnNextLevel;
        float width = _wallWidth;
        float depth = _height;
        int levelCellsCount = 10;
        int radialCuts = 2 * levelCellsCount + 1; // think about arch, if you don't understand +1

        var nextLevelRadius = _levels > 1 ? Mathf.Lerp(_internalRadius, _radius, (cell.Level - 1) / (float)(_levels - 1)) : _internalRadius;
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
        else
        {
            Debug.Log($"L{cell.Level}C{cell.Cell}: doesn't have left wall");
        }
        if (cell.RightWall)
        {
            var lerp = (cellsOnLevel - (cell.Cell + 1) % cellsOnLevel) / (float)(cellsOnLevel);
            var wallAngle = Mathf.Lerp(-180, 180, lerp);
            var wallAngleRadians = Mathf.Deg2Rad * wallAngle;
            InstantiateBoxWall(levelsDistance, betweenLevelRadius, wallAngleRadians, wallAngle, cell, false);
        }
        else
        {
            Debug.Log($"L{cell.Level}C{cell.Cell}: doesn't have right wall");
        }
        /*if (cell.BackWall)
        {
            var lerp = i / (float)levelCellsCount;
            var wallAngle = Mathf.Lerp(-180, 180, lerp);
            var wallAngleRadians = Mathf.Deg2Rad * wallAngle;
            InstantiateBoxWall(levelsDistance, betweenLevelRadius, wallAngleRadians, wallAngle);
        }*/
    }
    private void Generate()
    {
        Debug.Log("New generating");
        CircuitMazeGenerator circuitMazeGenerator = new();
        var cells = circuitMazeGenerator.Generate(_levels, _startCellsCount);
        foreach(var level in cells)
        {
            foreach (var cell in level)
            {
                InstantiateCell(cell);
            }
        }
    }
    private void OnEnable()
    {
        Generate();
    }
    private void OnDisable()
    {
        foreach (var wall in _walls)
        {
            DestroyImmediate(wall);
        }
    }
}