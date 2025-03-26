#nullable enable

using System;
using KarenKrill.Common.UI.Views.Abstractions;

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    public interface IMainMenuView : IView
    {
        public event Action? NewGame;
        public event Action? Exit;
        public event Action? Settings;
    }
}