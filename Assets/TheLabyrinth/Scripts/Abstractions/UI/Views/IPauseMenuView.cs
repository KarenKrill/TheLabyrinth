#nullable enable

using System;

using KarenKrill.UI.Views.Abstractions;

namespace TheLabyrinth.UI.Views.Abstractions
{
    public interface IPauseMenuView : IView
    {
        public event Action? Resume;
        public event Action? Settings;
        public event Action? Restart;
        public event Action? MainMenuExit;
        public event Action? Exit;
    }
}