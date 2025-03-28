using System;
using UnityEngine;
using UnityEngine.UI;

namespace KarenKrill.TheLabyrinth.UI.Views
{
    using Abstractions;
    using Common.UI.Views;

    public class PauseMenuView : ViewBehaviour, IPauseMenuView
    {
#nullable enable
        public event Action? Resume;
        public event Action? Restart;
        public event Action? Settings;
        public event Action? Exit;
#nullable restore

        [SerializeField]
        private Button _resumeButton;
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _settingsButton;
        [SerializeField]
        private Button _exitButton;

        private void OnEnable()
        {
            _resumeButton.onClick.AddListener(OnResumeButtonClicked);
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        private void OnDisable()
        {
            _resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnResumeButtonClicked() => Resume?.Invoke();
        private void OnRestartButtonClicked() => Restart?.Invoke();
        private void OnSettingsButtonClicked() => Settings?.Invoke();
        private void OnExitButtonClicked() => Exit?.Invoke();
    }
}