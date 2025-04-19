namespace KarenKrill.TheLabyrinth.Abstractions
{
    public enum QualityLevel
    {
        Low,
        Middle,
        High
    }

    public class GameSettings
    {
        #region Graphics

        public QualityLevel QualityLevel;

        #endregion

        #region Diagnostic

        public bool ShowFps;

        #endregion

        public GameSettings(QualityLevel qualityLevel = QualityLevel.High, bool showFps = false)
        {
            QualityLevel = qualityLevel;
            ShowFps = showFps;
        }
    }
}
