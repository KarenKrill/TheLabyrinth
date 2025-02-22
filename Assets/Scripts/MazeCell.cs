using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MazeCellWallType
{
    Left,
    Right,
    Front,
    Back
}

public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;
    [SerializeField]
    private GameObject _rightWall;
    [SerializeField]
    private GameObject _backWall;
    [SerializeField]
    private GameObject _frontWall;
    [SerializeField]
    private GameObject _fillWall;
    public bool IsVisited { get; private set; }
    public void Visit()
    {
        _fillWall.SetActive(false);
        IsVisited = true;
    }
    public void ClearWall(MazeCellWallType wallType)
    {
        switch (wallType)
        {
            case MazeCellWallType.Left:
                _leftWall.SetActive(false);
                break;
            case MazeCellWallType.Right:
                _rightWall.SetActive(false);
                break;
            case MazeCellWallType.Front:
                _frontWall.SetActive(false);
                break;
            case MazeCellWallType.Back:
                _backWall.SetActive(false);
                break;
        }
    }
}
