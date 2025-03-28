using System;
using UnityEngine;
using UnityEngine.UI;

namespace KarenKrill.TheLabyrinth.UI.Views
{
    using Abstractions;
    using Common.UI.Views;

    public class WinMenuView : ViewBehaviour, IWinMenuView
    {
#nullable enable
        public event Action? Restart;
        public event Action? Exit;
#nullable restore

        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _exitButton;

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(OnRestartButtonClicked);
            _exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _exitButton.onClick.RemoveListener(OnExitButtonClicked);
        }

        private void OnRestartButtonClicked() => Restart?.Invoke();
        private void OnExitButtonClicked() => Exit?.Invoke();
    }
}