using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _wallPrefab;
    [SerializeField]
    private int _mazeWidth, _mazeDepth;
    private List<GameObject> _walls = new();
    
    private void Start()
    {
        LightweightMazeGenerator lightweightMazeGenerator = new();
        var cells = lightweightMazeGenerator.Generate(_mazeWidth, _mazeDepth);
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                if (cells[x, z].LeftWall)
                {
                    var wall = Instantiate(_wallPrefab, new Vector3(x - 1f, 0, z + .5f), Quaternion.identity, transform);
                    _walls.Add(wall);
                }
                if (cells[x, z].RightWall)
                {
                    var wall = Instantiate(_wallPrefab, new Vector3(x + 1f, 0, z + .5f), Quaternion.identity, transform);
                    _walls.Add(wall);
                }
                if (cells[x, z].BackWall)
                {
                    var wall = Instantiate(_wallPrefab, new Vector3(x - 1f, 0, z), Quaternion.Euler(0,90,0), transform);
                    _walls.Add(wall);
                }
                if (cells[x, z].FrontWall)
                {
                    var wall = Instantiate(_wallPrefab, new Vector3(x - 1f, 0, z + 1f), Quaternion.Euler(0, 90, 0), transform);
                    _walls.Add(wall);
                }
            }
        }
    }
}
