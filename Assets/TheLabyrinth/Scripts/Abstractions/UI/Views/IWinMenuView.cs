#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    using UI.Views.Abstractions;

    public interface IWinMenuView : IView
    {
        public event Action? Restart;
        public event Action? MainMenuExit;
        public event Action? Exit;
    }
}