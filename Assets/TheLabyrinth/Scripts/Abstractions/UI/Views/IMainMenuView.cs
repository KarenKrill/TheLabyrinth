#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    using UI.Views.Abstractions;

    public interface IMainMenuView : IView
    {
        public event Action? NewGame;
        public event Action? Exit;
        public event Action? Settings;
    }
}