using System;
using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth.GameFlow
{
    using Abstractions;
    using Common.StateSystem.Abstractions;
    using Common.GameLevel.Abstractions;
    using Input.Abstractions;
    using Movement;

    public class GameplayController : MonoBehaviour, ITimeLimitedLevelController, IGameController
    {
#nullable enable
        public int CurrentLevelNumber => _PassedLevels;
        public string? CurrentLevelName => null;
        public float MaxCompleteTime => _timeOnCurrentLevel;
        public float RemainingTime => _TimeLeft;
        public float WarningTime => _warningLeftTime;
        public float LastWarningTime => _lastWarningLeftTime;

        public event Action? CurrentLevelChanged;
        public event Action<float>? MaxCompleteTimeChanged;
        public event Action<float>? RemainingTimeChanged;
        public event Action<float>? WarningTimeChanged;
        public event Action<float>? LastWarningTimeChanged;
#nullable restore

        [Inject]
        public void Initialize(IGameFlow gameFlow, IInputActionService inputActionService, IManagedStateMachine<GameState> managedStateMachine)
        {
            _gameFlow = gameFlow;
            _inputActionService = inputActionService;
            _managedStateMachine = managedStateMachine;
        }
        public void OnGameStart()
        {
            ResetToDefaults();
            _inputActionService.Disable();
        }
        public void OnGameEnd()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        public void OnLevelLoad()
        {
            _isLevelWasPaused = false;
            _inputActionService.Disable();
        }
        public void OnLevelPlay()
        {
            if (_isLevelWasPaused)
            {
                _isLevelWasPaused = false;
            }
            else
            {
                _timeOnCurrentLevel = Mathf.Round(_levelTimeFactor * Mathf.Sqrt(_loadLevelManager.TotalMazeCellsCount) / 5) * 5;
                OnMaxCompleteTimeChanged();
                if (_PassedLevels <= _gameLevelsCount) // game not ended
                {
                    _TimeLeft = _timeOnCurrentLevel;
                }
            }
            _inputActionService.SetActionMap(ActionMap.InGame);
            //Time.timeScale = 1;
        }
        public void OnLevelPause()
        {
            _isLevelWasPaused = true;
            _inputActionService.SetActionMap(ActionMap.UI);
            //Time.timeScale = 0;
        }
        public void OnLevelFinish()
        {
            if (_PassedLevels < _gameLevelsCount)
            {
                _PassedLevels++;
                OnCurrentLevelChanged();
                _gameFlow.LoadLevel();
            }
            else
            {
                _gameFlow.WinGame();
            }
        }
        public void OnPlayerLoose()
        {
            //Time.timeScale = 0;
            _inputActionService.SetActionMap(ActionMap.UI);// Disable();
        }
        public void OnPlayerWin()
        {
            //Time.timeScale = 0;
            _inputActionService.SetActionMap(ActionMap.UI); //_inputActionService.Disable();
        }

        [SerializeField]
        private CharacterMoveController _playerController;
        [SerializeField]
        private LoadLevelManager _loadLevelManager;
        [SerializeField]
        private int _gameLevelsCount = 13;
        [SerializeField]
        private float _levelTimeFactor = 5f;
        [SerializeField, Range(0, 1)]
        private float _warningLeftTime = 0.3f;
        [SerializeField, Range(0, 1)]
        private float _lastWarningLeftTime = 0.1f;

        private IGameFlow _gameFlow;
        private IInputActionService _inputActionService;
        private IManagedStateMachine<GameState> _managedStateMachine;

        private float _timeOnCurrentLevel;
        private bool _isLevelWasPaused = false;

        private float _timeLeft;
        private float _TimeLeft
        {
            get => _timeLeft;
            set
            {
                if (_timeLeft != value)
                {
                    if (MathF.Round(value, 1) != MathF.Round(_timeLeft, 1))
                    {
                        _timeLeft = value;
                        OnRemainingTimeChanged();
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
                }
            }
        }

        private void Awake()
        {
            _managedStateMachine.Start();
            _inputActionService.Pause += OnPaused;
            _inputActionService.Back += OnResumed;
        }
        private void Update()
        {
            if (_gameFlow.State == GameState.LevelPlay)
            {
                UpdateLeftLevelTime();
            }
        }
        private void OnValidate()
        {
            if (_warningLeftTime < _lastWarningLeftTime)
            {
                _warningLeftTime = _lastWarningLeftTime;
            }
            OnWarningTimeChanged();
            OnLastWarningTimeChanged();
        }

        private void UpdateLeftLevelTime()
        {
            _TimeLeft = _TimeLeft > Time.deltaTime ? _TimeLeft - Time.deltaTime : 0;
            if (_TimeLeft == 0)
            {
                _gameFlow.LooseGame();
            }
        }
        private void ResetToDefaults()
        {
            _TimeLeft = 0;
            _PassedLevels = 1;
            OnCurrentLevelChanged();
        }
        private void OnPaused()
        {
            _gameFlow.PauseLevel();
        }
        private void OnResumed()
        {
            _gameFlow.PlayLevel();
        }
        private void OnMaxCompleteTimeChanged()
        {
            MaxCompleteTimeChanged?.Invoke(MaxCompleteTime);
        }
        private void OnRemainingTimeChanged()
        {
            RemainingTimeChanged?.Invoke(RemainingTime);
        }
        private void OnCurrentLevelChanged()
        {
            CurrentLevelChanged?.Invoke();
        }
        private void OnWarningTimeChanged()
        {
            WarningTimeChanged?.Invoke(_warningLeftTime);
        }
        private void OnLastWarningTimeChanged()
        {
            LastWarningTimeChanged?.Invoke(_lastWarningLeftTime);
        }
    }
}