#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    using Common.UI.Views.Abstractions;

    public interface ISettingsMenuView : IView
    {
        #region Graphics

        #endregion

        #region Diagnostic
        bool ShowFps { set; }
        #endregion

        event Action? Apply;
        event Action? Cancel;
        event Action<bool>? ShowFpsChanged;
    }
}