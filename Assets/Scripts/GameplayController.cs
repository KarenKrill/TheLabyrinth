using System.Collections;
using UnityEngine;
using TMPro;
using Zenject;

namespace KarenKrill
{
    using MazeGeneration;
    using Core;

    public class GameplayController : MonoBehaviour
    {
        [Inject]
        IGameFlow _gameFlow;
        [Inject]
        ILogger _logger;

        [SerializeField]
        private PlayerController _playerController;
        [SerializeField]
        private TextMeshProUGUI _levelInfoTextBox;
        [SerializeField]
        private TextMeshProUGUI _timesLeftTextBox;
        [SerializeField]
        private int _gameLevelsCount = 13;
        [SerializeField]
        private float _aiPlayerModeMaxSpeed = 50;
        [SerializeField]
        float _levelTimeFactor = 5f;
        float _timeOnCurrentLevel;
        [SerializeField, Range(0, 1)]
        float _redWarnAlertTimeLeft = 0.1f;
        [SerializeField, Range(0, 1)]
        float _warnAlertTimeLeft = 0.3f;
        [SerializeField]
        LoadLevelManager _loadLevelManager;

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
        private float _humanPlayerModePlayerSpeed = 0;

        public void Awake()
        {
            _gameFlow.GameStart += OnGameStart;
            _gameFlow.LevelFinish += OnLevelFinish;
            _gameFlow.LevelLoad += OnLevelLoad;
            _gameFlow.PlayerWin += OnPlayerWin;
            _gameFlow.PlayerLoose += OnPlayerLoose;
            _gameFlow.LevelPlay += OnLevelPlay;
            _gameFlow.LevelPause += OnLevelPause;
            _gameFlow.GameEnd += OnGameEnd;
            //_gameFlow.LoadMainMenu();
            //_gameFlow.StartGame();
        }
        void ResetToDefaults()
        {
            SetAiPlayingMode(false);
            _timesLeftTextBox.text = $"TimesLeft: 0.000 s";
            _timesLeftTextBox.color = Color.white;
            _PassedLevels = 1;
        }

        private void OnGameStart()
        {
            ResetToDefaults();
        }
        private void OnGameEnd()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        private void OnLevelLoad()
        {
        }
        private void OnLevelPlay()
        {
            _timeOnCurrentLevel = Mathf.Round(_levelTimeFactor * Mathf.Sqrt(_loadLevelManager.TotalMazeCellsCount) / 5) * 5;
            if (_PassedLevels < _gameLevelsCount) // game not ended
            {
                _TimeLeft = _timeOnCurrentLevel;
                _timesLeftTextBox.color = Color.white;
            }
            _playerController.UnlockMovement();
            //Time.timeScale = 1;
        }
        private void OnLevelPause()
        {
            _playerController.LockMovement();
            //Time.timeScale = 0;
        }
        private void OnLevelFinish()
        {
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
        private void OnPlayerLoose()
        {
            SetAiPlayingMode();
            //Time.timeScale = 0;
            _playerController.LockMovement(xAxis: true, yAxis: true, zAxis: true);
        }
        private void OnPlayerWin()
        {
            SetAiPlayingMode();
            //Time.timeScale = 0;
            _playerController.LockMovement(xAxis: true, yAxis: true, zAxis: true);
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