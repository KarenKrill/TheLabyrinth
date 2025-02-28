using Assets.Scripts;
using ModestTree;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _winTextBox;
    [SerializeField]
    private Vector3 _playerSpawnPoint = Vector3.one;
    [SerializeField]
    private PlayerController _playerController;
    [SerializeReference]
    private ArcMazeGenerator _mazeGenerator;
    [SerializeField]
    private Transform _exitPointTransform;

    private int _passedLevels = 0;
    private int _PassedLevels
    {
        get => _passedLevels;
        set
        {
            if (value == 0 || _passedLevels != value)
            {
                _passedLevels = value;
                _winTextBox.text = $"Пройдено уровней: {_PassedLevels}";
            }
        }
    }
    [SerializeField]
    private int _mazeStartLevelsCount = 4;
    [SerializeField]
    private int _mazeMaxLevelsCount = 13;
    [SerializeField]
    private int _mazeEndLevelsCount = 13;
    private int _mazeLevelsCount = 4;
    [SerializeField]
    private GameObject _winWindow;
    private void OnMazeGenerationFinished(CircuitMaze maze)
    {
        if (_exitPointTransform != null)
        {
            var exitCellCenter = _mazeGenerator.GetCellCenter(maze.ExitCell);
            exitCellCenter = _mazeGenerator.GetCellCenter(maze.Cells[0][0]);
            _exitPointTransform.position = new Vector3(exitCellCenter.x, _exitPointTransform.position.y, exitCellCenter.y);
        }
    }
    public IEnumerator Start()
    {
        _PassedLevels = 0;
        _mazeLevelsCount = 0;
        _mazeGenerator.Levels = _mazeStartLevelsCount;
        _mazeGenerator.MazeGenerationFinished.AddListener(OnMazeGenerationFinished);
        var playerSpawnPoint = _mazeGenerator.GetCellCenter(new(_mazeGenerator.Levels - 1, 0, 0));
        yield return _playerController.Move(new Vector3(playerSpawnPoint.x, 10, playerSpawnPoint.y));
        _mazeGenerator.Generate();
    }
    private IEnumerator FinishLevelCoroutine()
    {
        if (_mazeLevelsCount < _mazeEndLevelsCount - 1)
        {
            _mazeLevelsCount++;
            if (_mazeLevelsCount < _mazeStartLevelsCount)
            {
                _mazeGenerator.Levels = _mazeStartLevelsCount;
            }
            else
            {
                _mazeGenerator.Levels = _mazeLevelsCount < _mazeMaxLevelsCount ? _mazeLevelsCount : _mazeMaxLevelsCount;
            }
            var playerSpawnPoint = _mazeGenerator.GetCellCenter(new(_mazeGenerator.Levels - 1, 0, 0));
            yield return _playerController.Move(new Vector3(playerSpawnPoint.x, 10, playerSpawnPoint.y));
            _mazeGenerator.Generate();
        }
        else
        {
            _winWindow.SetActive(true);
        }
    }
    public void FinishLevel()
    {
        _PassedLevels++;
        StartCoroutine(FinishLevelCoroutine());
    }
    public IEnumerator RestartCoroutine()
    {
        _mazeGenerator.MazeGenerationFinished.RemoveListener(OnMazeGenerationFinished);
        yield return Start();
        _winWindow.SetActive(false);
    }
    public void Restart()
    {
        StartCoroutine(RestartCoroutine());
    }
}
