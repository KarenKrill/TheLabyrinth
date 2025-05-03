using System;
using UnityEngine;
using UnityEngine.UI;

using KarenKrill.UI.Views;

namespace TheLabyrinth.UI.Views
{
    using Abstractions;

    public class SettingsMenuView : ViewBehaviour, ISettingsMenuView
    {
        #region Graphics

        #endregion

        #region Diagnostic
        public bool ShowFps { get => _showFpsToggle.isOn; set => _showFpsToggle.isOn = value; }
        #endregion

#nullable enable
        public event Action? Apply;
        public event Action? Cancel;
#nullable restore

        [SerializeField]
        private Toggle _showFpsToggle;
        [SerializeField]
        private Button _applyButton;
        [SerializeField]
        private Button _cancelButton;

        private void OnEnable()
        {
            _applyButton.onClick.AddListener(OnApplyButtonClicked);
            _cancelButton.onClick.AddListener(OnCancelButtonClicked);
        }
        private void OnDisable()
        {
            _applyButton.onClick.RemoveListener(OnApplyButtonClicked);
            _cancelButton.onClick.RemoveListener(OnCancelButtonClicked);
        }

        private void OnApplyButtonClicked() => Apply?.Invoke();
        private void OnCancelButtonClicked() => Cancel?.Invoke();
    }
}