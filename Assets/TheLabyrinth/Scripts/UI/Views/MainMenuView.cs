using System;
using UnityEngine;
using UnityEngine.UI;

namespace KarenKrill.TheLabyrinth.UI.Views
{
    using Abstractions;
    using Common.UI.Views;

    public class MainMenuView : ViewBehaviour, IMainMenuView
    {
#nullable enable
        public event Action? NewGame;
        public event Action? Settings;
        public event Action? Exit;
#nullable restore

        [SerializeField]
        private Button _newGameButton;
        [SerializeField]
        private Button _settingsButton;
        [SerializeField]
        private Button _exitButton;

        private void OnEnable()
        {
            _newGameButton.onClick.AddListener(OnNewGameButtonClicked);
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        private void OnDisable()
        {
            _newGameButton.onClick.RemoveListener(OnNewGameButtonClicked);
            _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnNewGameButtonClicked() => NewGame?.Invoke();
        private void OnSettingsButtonClicked() => Settings?.Invoke();
        private void OnExitButtonClicked() => Exit?.Invoke();
    }
}