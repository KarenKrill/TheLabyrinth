using System;
using UnityEngine;
using UnityEngine.UI;

namespace KarenKrill.UI.Views
{
    using Core.UI.Views;

    public class LooseMenuView : UiViewBehaviour, ILooseMenuView
    {
        [SerializeField]
        private Button _restartButton;
        [SerializeField]
        private Button _exitButton;
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