using System;
using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth.GameFlow
{
    using Abstractions;
    using Common.GameFlow.Abstractions;
    using Common.GameInfo.Abstractions;

    public class LevelController : MonoBehaviour, ITimeLimitedLevelController, ITimeLimitedLevelInfoProvider
    {
        public float MaxCompleteTime
        {
            get => _timeOnCurrentLevel;
            set
            {
                if (_timeOnCurrentLevel != value)
                {
                    _timeOnCurrentLevel = value;
                }
            }
        }
        public float RemainingTime
        {
            get => _TimeLeft;
            set
            {
                if (_TimeLeft != value)
                {
                    _TimeLeft = value;
                }
            }
        }
        public float WarningTime
        {
            get => _warningLeftTime;
            set
            {
                if (_warningLeftTime != value)
                {
                    _warningLeftTime = value;
                }
            }
        }
        public float LastWarningTime
        {
            get => _lastWarningLeftTime;
            set
            {
                if (_lastWarningLeftTime != value)
                {
                    _lastWarningLeftTime = value;
                }
            }
        }

#nullable enable
        public event Action<float>? MaxCompleteTimeChanged;
        public event Action<float>? RemainingTimeChanged;
        public event Action<float>? WarningTimeChanged;
        public event Action<float>? LastWarningTimeChanged;
#nullable restore

        [Inject]
        public void Initialize(IGameFlow gameFlow)
        {
            _gameFlow = gameFlow;
        }

        public void Enable()
        {
            enabled = true;
            Debug.Log($"{nameof(LevelController)} enabled");
        }
        public void Disable()
        {
            enabled = false;
            Debug.Log($"{nameof(LevelController)} disabled");
        }

        public void OnGameStart()
        {
            _TimeLeft = 0;
        }
        public void OnLevelPlay()
        {
            _timeOnCurrentLevel = Mathf.Round(_levelTimeFactor * Mathf.Sqrt(_loadLevelManager.TotalMazeCellsCount) / 5) * 5;
            OnMaxCompleteTimeChanged();
            _TimeLeft = _timeOnCurrentLevel;
        }

        [SerializeField]
        private LoadLevelManager _loadLevelManager;
        [SerializeField]
        private float _levelTimeFactor = 5f;
        [SerializeField, Range(0, 1)]
        private float _warningLeftTime = 0.3f;
        [SerializeField, Range(0, 1)]
        private float _lastWarningLeftTime = 0.1f;

        private IGameFlow _gameFlow;

        private float _timeOnCurrentLevel;

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
        private void Update()
        {
            UpdateLeftLevelTime();
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
        
        private void OnMaxCompleteTimeChanged()
        {
            MaxCompleteTimeChanged?.Invoke(MaxCompleteTime);
        }
        private void OnRemainingTimeChanged()
        {
            RemainingTimeChanged?.Invoke(RemainingTime);
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