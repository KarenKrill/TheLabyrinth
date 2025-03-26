using System;
using UnityEngine;
using UnityEngine.UI;
using KarenKrill.Common.UI.Views;

namespace KarenKrill.TheLabyrinth.UI.Views
{
    using Abstractions;

    public class MainMenuView : ViewBehaviour, IMainMenuView
    {
        [SerializeField]
        private Button _newGameButton;
        [SerializeField]
        private Button _settingsButton;
        [SerializeField]
        private Button _exitButton;
#nullable enable
        public event Action? NewGame;
        public event Action? Settings;
        public event Action? Exit;
        private void OnNewGameButtonClicked() => NewGame?.Invoke();
        private void OnSettingsButtonClicked() => Settings?.Invoke();
        private void OnExitButtonClicked() => Exit?.Invoke();
#nullable restore
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
    }
}