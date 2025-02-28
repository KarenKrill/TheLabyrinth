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
                _winTextBox.text = $"Пройдено уровней: {_passedLevels}";
            }
        }
    }
    public void Start()
    {
        _PassedLevels = 0;
        _mazeGenerator.MazeGenerationFinished.AddListener((maze) =>
        {
            if (_exitPointTransform != null)
            {
                var level = maze.ExitCell.Level;
                var cell = maze.ExitCell.Cell;
                _exitPointTransform.position = new Vector3(level, _exitPointTransform.position.y, cell);
            }
        });
        _mazeGenerator.Generate();
    }
    private IEnumerator FinishLevelCoroutine()
    {
        yield return _playerController.Move(_playerSpawnPoint);
        _mazeGenerator.Generate();
    }
    public void FinishLevel()
    {
        _PassedLevels++;
        StartCoroutine(FinishLevelCoroutine());
    }
}
