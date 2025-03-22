using UnityEngine;
using KarenKrill.Core;
using KarenKrill.Core.UI.Presenters;
using KarenKrill.UI.Views;
using KarenKrill.Core.Level;
using System.Globalization;
using KarenKrill.Core.Utilities;

namespace KarenKrill.UI.Presenters
{
    public class LevelInfoPresenter : IPresenter<IILevelInfoView>
    {
        public IILevelInfoView View { get; }
        ITimeLimitedLevelController _levelController;
        IGameController _gameController;
        public LevelInfoPresenter(IILevelInfoView view, ITimeLimitedLevelController levelController, IGameController gameController)
        {
            View = view;
            _levelController = levelController;
            _gameController = gameController;
        }
        void UpdateRemainingTimeTextColor()
        {
            float relativeRemainingTime = _levelController.RemainingTime / _levelController.MaxCompleteTime;
            if (relativeRemainingTime < _levelController.WarningTime)
            {
                if (relativeRemainingTime < _levelController.LastWarningTime)
                {
                    View.RemainingTimeTextColor = Color.red.ToSystemColor();
                }
                else
                {
                    View.RemainingTimeTextColor = Color.yellow.ToSystemColor();
                }
            }
            else
            {
                View.RemainingTimeTextColor = Color.white.ToSystemColor();
            }
        }
        private void OnWarningTimeChanged(float _) => UpdateRemainingTimeTextColor();
        private void OnMaxCompleteTimeChanged(float _) => UpdateRemainingTimeTextColor();
        void OnRemainingTimeChanged(float remainingTime)
        {
            string prefix = "Time left";
            View.RemainingTimeText = string.Format("{0}: {1} s", prefix, remainingTime.ToString("0.0", CultureInfo.InvariantCulture));
            UpdateRemainingTimeTextColor();
        }
        private void OnCurrentLevelChanged()
        {
            if (string.IsNullOrEmpty(_gameController.CurrentLevelName))
            {
                string prefix = "Level";
                View.Title = string.Format("{0}: {1}", prefix, _gameController.CurrentLevelNumber);
            }
            else
            {
                View.Title = _gameController.CurrentLevelName;
            }
        }
        public void Enable()
        {
            _levelController.RemainingTimeChanged += OnRemainingTimeChanged;
            _levelController.MaxCompleteTimeChanged += OnMaxCompleteTimeChanged;
            _levelController.WarningTimeChanged += OnWarningTimeChanged;
            _levelController.LastWarningTimeChanged += OnWarningTimeChanged;
            _gameController.CurrentLevelChanged += OnCurrentLevelChanged;
            OnMaxCompleteTimeChanged(_levelController.MaxCompleteTime);
            OnCurrentLevelChanged();
            View.Show();
        }
        public void Disable()
        {
            View.Close();
            _levelController.RemainingTimeChanged -= OnRemainingTimeChanged;
            _levelController.MaxCompleteTimeChanged -= OnMaxCompleteTimeChanged;
            _levelController.WarningTimeChanged -= OnWarningTimeChanged;
            _levelController.LastWarningTimeChanged -= OnWarningTimeChanged;
            _gameController.CurrentLevelChanged -= OnCurrentLevelChanged;
        }
    }
}