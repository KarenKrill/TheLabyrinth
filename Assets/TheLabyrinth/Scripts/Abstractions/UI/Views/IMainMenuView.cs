#nullable enable

using System;

using KarenKrill.UI.Views.Abstractions;

namespace TheLabyrinth.UI.Views.Abstractions
{
    public interface IMainMenuView : IView
    {
        public event Action? NewGame;
        public event Action? Exit;
        public event Action? Settings;
    }
}