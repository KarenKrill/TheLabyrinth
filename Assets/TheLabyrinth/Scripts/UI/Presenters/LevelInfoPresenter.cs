using System.Globalization;
using UnityEngine;

namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using Common.GameInfo.Abstractions;
    using Common.UI.Presenters.Abstractions;
    using Common.Utilities;
    using GameFlow.Abstractions;
    using Views.Abstractions;

    public class LevelInfoPresenter : IPresenter<IILevelInfoView>
    {
        public IILevelInfoView View { get; set; }

        public LevelInfoPresenter(ITimeLimitedLevelInfoProvider levelInfoProvider, IGameInfoProvider gameInfoProvider)
        {
            _levelInfoProvider = levelInfoProvider;
            _gameInfoProvider = gameInfoProvider;
        }
        public void Enable()
        {
            _levelInfoProvider.RemainingTimeChanged += OnRemainingTimeChanged;
            _levelInfoProvider.MaxCompleteTimeChanged += OnMaxCompleteTimeChanged;
            _levelInfoProvider.WarningTimeChanged += OnWarningTimeChanged;
            _levelInfoProvider.LastWarningTimeChanged += OnWarningTimeChanged;
            _gameInfoProvider.CurrentLevelChanged += OnCurrentLevelChanged;
            OnMaxCompleteTimeChanged(_levelInfoProvider.MaxCompleteTime);
            OnCurrentLevelChanged();
            View.Show();
        }
        public void Disable()
        {
            View.Close();
            _levelInfoProvider.RemainingTimeChanged -= OnRemainingTimeChanged;
            _levelInfoProvider.MaxCompleteTimeChanged -= OnMaxCompleteTimeChanged;
            _levelInfoProvider.WarningTimeChanged -= OnWarningTimeChanged;
            _levelInfoProvider.LastWarningTimeChanged -= OnWarningTimeChanged;
            _gameInfoProvider.CurrentLevelChanged -= OnCurrentLevelChanged;
        }

        private readonly ITimeLimitedLevelInfoProvider _levelInfoProvider;
        private readonly IGameInfoProvider _gameInfoProvider;

        private void UpdateRemainingTimeTextColor()
        {
            float relativeRemainingTime = _levelInfoProvider.RemainingTime / _levelInfoProvider.MaxCompleteTime;
            if (relativeRemainingTime < _levelInfoProvider.WarningTime)
            {
                if (relativeRemainingTime < _levelInfoProvider.LastWarningTime)
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
            if (string.IsNullOrEmpty(_gameInfoProvider.CurrentLevelName))
            {
                string prefix = "Level";
                View.Title = string.Format("{0}: {1}", prefix, _gameInfoProvider.CurrentLevelNumber);
            }
            else
            {
                View.Title = _gameInfoProvider.CurrentLevelName;
            }
        }
    }
}