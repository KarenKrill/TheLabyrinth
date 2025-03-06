using System.Collections;
using UnityEngine;
using TMPro;
using Zenject;

namespace KarenKrill
{
    using MazeGeneration;

    public class GameplayController : MonoBehaviour
    {
        [Inject]
        IGameFlow _gameFlow;
        [Inject]
        ILogger _logger;
        [SerializeField]
        private TextMeshProUGUI _levelInfoTextBox;
        [SerializeField]
        private TextMeshProUGUI _timesLeftTextBox;
        [SerializeField]
        private PlayerController _playerController;
        [SerializeReference]
        private ArcMazeBuilder _mazeBuilder;
        [SerializeField]
        private Transform _exitPointTransform;
        [SerializeField]
        private int _mazeMinLevelsCount = 4;
        [SerializeField]
        private int _mazeMaxLevelsCount = 13;
        [SerializeField]
        private int _gameLevelsCount = 13;
        [SerializeField]
        private float _aiPlayerModeMaxSpeed = 50;
        [SerializeField]
        private GameObject _winWindow;
        [SerializeField]
        private GameObject _looseWindow;
        [SerializeField]
        float _levelTimeFactor = 5f;
        float _timeOnCurrentLevel;
        [SerializeField, Range(0, 1)]
        float _redWarnAlertTimeLeft = 0.1f;
        [SerializeField, Range(0, 1)]
        float _warnAlertTimeLeft = 0.3f;

        private float _timeLeft;
        private float _TimeLeft
        {
            get => _timeLeft;
            set
            {
                if (_timeLeft != value)
                {
                    if (System.MathF.Round(value, 1) != System.MathF.Round(_timeLeft, 1))
                    {
                        _timeLeft = value;
                        _timesLeftTextBox.text = $"TimesLeft: {_timeLeft:0.0} s";
                        if (_timeLeft / _timeOnCurrentLevel < _warnAlertTimeLeft)
                        {
                            if (_timeLeft / _timeOnCurrentLevel < _redWarnAlertTimeLeft)
                            {
                                _timesLeftTextBox.color = Color.red;
                            }
                            else
                            {
                                _timesLeftTextBox.color = Color.yellow;
                            }
                        }
                    }
                    else
                    {
                        _timeLeft = value;
                    }
                }
            }
        }

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
        private int _mazeLevelsCount = 0;
        private float _humanPlayerModePlayerSpeed = 0;

        public void Start()
        {
            _gameFlow.GameStart += OnGameStart;
            _gameFlow.LevelFinish += OnLevelFinish;
            _gameFlow.LevelLoad += OnLevelLoad;
            _gameFlow.PlayerWin += OnPlayerWin;
            _gameFlow.PlayerLoose += OnPlayerLoose;
            _gameFlow.LevelPlay += OnLevelPlay;

            //_gameFlow.LoadMainMenu();
            _gameFlow.StartGame();
        }
        void ResetToDefaults()
        {
            SetAiPlayingMode(false);
            _timesLeftTextBox.text = $"TimesLeft: 0.000 s";
            _timesLeftTextBox.color = Color.white;
            _winWindow.SetActive(false);
            _looseWindow.SetActive(false);
            _PassedLevels = 1;
            _mazeLevelsCount = _mazeMinLevelsCount;
            _mazeBuilder.Levels = _mazeLevelsCount;
        }
        private IEnumerator LoadLevelCoroutine()
        {
            _playerController.LockMovement(xAxis: true, yAxis: true, zAxis: true);
            yield return _mazeBuilder.BuildCoroutine();
            _timeOnCurrentLevel = Mathf.Round(_levelTimeFactor * Mathf.Sqrt(_mazeBuilder.TotalCellsCount) / 5) * 5;
            if (_PassedLevels < _gameLevelsCount) // game not ended
            {
                _TimeLeft = _timeOnCurrentLevel;
                _timesLeftTextBox.color = Color.white;
            }
            var playerSpawnPoint = _mazeBuilder.GetCellCenter(_mazeBuilder.Levels - 1, 0);
            _playerController.LockMovement(xAxis: true, yAxis: false, zAxis: true);
            yield return _playerController.Move(new Vector3(playerSpawnPoint.x, 100, playerSpawnPoint.y));
            if (_exitPointTransform != null)
            {
                var exitCellCenter = _mazeBuilder.GetCellCenter(0, 0);
                _exitPointTransform.position = new Vector3(exitCellCenter.x, _exitPointTransform.position.y, exitCellCenter.y);
            }
            yield return new WaitForSeconds(0.5f); // wait while player isn't fall (stucks in air)
            yield return new WaitUntil(() => _playerController.IsGrounded);// wait until player isn't grounded
            _gameFlow.PlayLevel();
        }
        private IEnumerator FinishLevelCoroutine()
        {
            _playerController.LockMovement();
            yield return _playerController.Move(new Vector3(0, 1, 60));
            _mazeBuilder.Levels = _mazeLevelsCount < _mazeMaxLevelsCount ? ++_mazeLevelsCount : _mazeMaxLevelsCount;
            if (_PassedLevels < _gameLevelsCount)
            {
                _PassedLevels++;
                _gameFlow.LoadLevel();
            }
            else
            {
                _gameFlow.WinGame();
            }
        }

        private void OnGameStart()
        {
            ResetToDefaults();
            _gameFlow.LoadLevel();
        }
        private void OnLevelLoad() => StartCoroutine(LoadLevelCoroutine());
        private void OnLevelPlay() => _playerController.UnlockMovement();
        private void OnLevelFinish() => StartCoroutine(FinishLevelCoroutine());
        private void OnPlayerLoose()
        {
            SetAiPlayingMode();
            _playerController.LockMovement(xAxis: true, yAxis: true, zAxis: true);
            _looseWindow.SetActive(true);
        }
        private void OnPlayerWin()
        {
            SetAiPlayingMode();
            _playerController.LockMovement(xAxis: true, yAxis: true, zAxis: true);
            _winWindow.SetActive(true);
        }
        private void SetAiPlayingMode(bool turnOn = true)
        {
            if (_playerController.UseAiNavigation != turnOn)
            {
                _playerController.UseAiNavigation = turnOn;
                if (turnOn)
                {
                    _humanPlayerModePlayerSpeed = _playerController.MaximumSpeed;
                    _playerController.MaximumSpeed = _aiPlayerModeMaxSpeed;
                    _playerController.AiMinSpeed = _aiPlayerModeMaxSpeed / 10;
                }
                else
                {
                    if (_humanPlayerModePlayerSpeed == 0) // first time
                    {
                        _humanPlayerModePlayerSpeed = _playerController.MaximumSpeed;
                    }
                    else
                    {
                        _playerController.MaximumSpeed = _humanPlayerModePlayerSpeed;
                    }
                }
            }
        }
        private void UpdateLeftLevelTime()
        {
            _TimeLeft = _TimeLeft > Time.deltaTime ? _TimeLeft - Time.deltaTime : 0;
            if (_TimeLeft == 0)
            {
                _gameFlow.LooseGame();
            }
        }
        public void Update()
        {
            if (_gameFlow.State == GameState.LevelPlay)
            {
                if (Input.GetKeyDown(KeyCode.F)) // cheat
                {
                    SetAiPlayingMode(!_playerController.UseAiNavigation);
                }
                UpdateLeftLevelTime();
            }
        }
    }
}