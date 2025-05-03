#nullable enable

using System;

using KarenKrill.UI.Views.Abstractions;

namespace TheLabyrinth.UI.Views.Abstractions
{
    public interface IWinMenuView : IView
    {
        public event Action? Restart;
        public event Action? MainMenuExit;
        public event Action? Exit;
    }
}