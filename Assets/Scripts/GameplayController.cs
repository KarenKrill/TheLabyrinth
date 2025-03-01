using Assets.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameplayController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _levelInfoTextBox;
    [SerializeField]
    private TextMeshProUGUI _timesLeftTextBox;
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
                _levelInfoTextBox.text = $"Level: {_PassedLevels}";
            }
        }
    }
    [SerializeField]
    private int _mazeMinLevelsCount = 4;
    [SerializeField]
    private int _mazeMaxLevelsCount = 13;
    [SerializeField]
    private int _gameLevelsCount = 13;
    private int _mazeLevelsCount = 0;
    private float _playerSpeed;
    [SerializeField]
    private float _menuAiPlayerSpeed = 50;
    [SerializeField]
    private GameObject _winWindow;
    [SerializeField]
    private GameObject _looseWindow;
    float _startTime;
    [SerializeField]
    float _timeOnCell = 5f;
    private IEnumerator AfterLevelGeneratedCoroutine(CircuitMaze maze)
    {
        var playerSpawnPoint = _mazeGenerator.GetCellCenter(new(_mazeGenerator.Levels - 1, 0, 0));
        yield return _playerController.Move(new Vector3(playerSpawnPoint.x, 100, playerSpawnPoint.y));
        if (_exitPointTransform != null)
        {
            var exitCellCenter = _mazeGenerator.GetCellCenter(maze.Cells[0][0]);
            _exitPointTransform.position = new Vector3(exitCellCenter.x, _exitPointTransform.position.y, exitCellCenter.y);
        }
        _startTime = Time.time;
        _levelStarted = true;
    }
    private void OnMazeGenerationFinished(CircuitMaze maze)
    {
        StartCoroutine(AfterLevelGeneratedCoroutine(maze));
    }
    private bool _levelStarted = false;
    public IEnumerator Start()
    {
        _timesLeftTextBox.text = $"TimesLeft: 0.000 s";
        _timesLeftTextBox.color = Color.white;
        _winWindow.SetActive(false);
        _looseWindow.SetActive(false);
        _PassedLevels = 1;
        _mazeLevelsCount = _mazeMinLevelsCount;
        _mazeGenerator.Levels = _mazeMinLevelsCount;
        _mazeGenerator.MazeGenerationFinished.AddListener(OnMazeGenerationFinished);
        _playerController.LockMovement(xAxis: true, yAxis: false, zAxis: true);
        yield return _mazeGenerator.GenerateCoroutine();
        _playerController.UnlockMovement();
    }
    private IEnumerator FinishLevelCoroutine()
    {
        _levelStarted = false;
        bool isGameEnd = true;
        if (_PassedLevels < _gameLevelsCount)
        {
            _PassedLevels++;
            isGameEnd = false;
        }
        else if (!_winWindow.activeInHierarchy)
        {
            _playerController.UseAiNavigation = true;
            _playerSpeed = _playerController.MaximumSpeed;
            _playerController.MaximumSpeed = _menuAiPlayerSpeed;
            _winWindow.SetActive(true);
        }
        yield return _playerController.Move(new Vector3(0, 1, 60));
        _mazeGenerator.Levels = _mazeLevelsCount < _mazeMaxLevelsCount ? ++_mazeLevelsCount : _mazeMaxLevelsCount;
        _playerController.LockMovement(xAxis: true, yAxis: false, zAxis: true);
        yield return _mazeGenerator.GenerateCoroutine();
        _playerController.UnlockMovement();
        if (!isGameEnd)
        {
            _startTime = Time.time;
            _levelStarted = true;
            _timesLeftTextBox.color = Color.white;
        }
    }
    public void FinishLevel() => StartCoroutine(FinishLevelCoroutine());
    public IEnumerator RestartCoroutine()
    {
        _playerController.MaximumSpeed = _playerSpeed;
        _playerController.UseAiNavigation = false;
        _mazeGenerator.MazeGenerationFinished.RemoveListener(OnMazeGenerationFinished);
        yield return Start();
        _winWindow.SetActive(false);
        _looseWindow.SetActive(false);
    }
    public void Restart() => StartCoroutine(RestartCoroutine());
    [SerializeField, Range(0,1)]
    float _redWarnAlertTimeLeft = 0.1f;
    [SerializeField, Range(0, 1)]
    float _warnAlertTimeLeft = 0.3f;
    public void Update()
    {
        if (_levelStarted)
        {
            if (!_winWindow.activeInHierarchy)
            {
                var timeSinceLevelStarted = Time.time - _startTime;
                var timeOnCurrentLevel = _timeOnCell * _mazeGenerator.TotalCellsCount;//_mazeGenerator.Levels;
                var timeLeft = timeOnCurrentLevel > timeSinceLevelStarted ? timeOnCurrentLevel - timeSinceLevelStarted : 0;
                _timesLeftTextBox.text = $"TimesLeft: {timeLeft:0.000} s";
                if (timeLeft / timeOnCurrentLevel < _warnAlertTimeLeft)
                {
                    if (timeLeft / timeOnCurrentLevel < _redWarnAlertTimeLeft)
                    {
                        _timesLeftTextBox.color = Color.red;
                    }
                    else
                    {
                        _timesLeftTextBox.color = Color.yellow;
                    }
                }
                if (timeLeft == 0)
                {
                    _levelStarted = false;
                    _playerController.LockMovement(xAxis: true, yAxis: true, zAxis: true);
                    _looseWindow.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                _playerController.UseAiNavigation = !_playerController.UseAiNavigation;
                if (_playerController.UseAiNavigation)
                {
                    _playerController.MaximumSpeed = _menuAiPlayerSpeed;
                }
                else
                {
                    _playerController.MaximumSpeed = _playerSpeed;
                }
            }
        }
    }
}
