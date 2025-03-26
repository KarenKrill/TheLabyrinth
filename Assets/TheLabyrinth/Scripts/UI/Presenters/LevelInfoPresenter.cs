using System.Globalization;
using UnityEngine;
using KarenKrill.Common.GameLevel;
using KarenKrill.Common.Utilities;
using KarenKrill.Common.UI.Presenters.Abstractions;

namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using GameFlow.Abstractions;
    using Views.Abstractions;

    public class LevelInfoPresenter : IPresenter<IILevelInfoView>
    {
        public IILevelInfoView View { get; set; }
        private ITimeLimitedLevelController _levelController;
        private IGameController _gameController;
        public LevelInfoPresenter(ITimeLimitedLevelController levelController, IGameController gameController)
        {
            _levelController = levelController;
            _gameController = gameController;
        }
        private void UpdateRemainingTimeTextColor()
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
        private void OnRemainingTimeChanged(float remainingTime)
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