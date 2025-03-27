using System;
using UnityEngine;
using UnityEngine.UI;

namespace KarenKrill.TheLabyrinth.UI.Views
{
    using Abstractions;
    using Common.UI.Views;

    public class WinMenuView : ViewBehaviour, IWinMenuView
    {
        [SerializeField]
        Button _restartButton;
        [SerializeField]
        Button _exitButton;
#nullable enable
        public event Action? Restart;
        public event Action? Exit;
        private void OnRestartButtonClicked() => Restart?.Invoke();
        private void OnExitButtonClicked() => Exit?.Invoke();
#nullable restore
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
    }
}