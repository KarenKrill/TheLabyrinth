#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    using Common.UI.Views.Abstractions;

    public interface IPauseMenuView : IView
    {
        public event Action? Resume;
        public event Action? Settings;
        public event Action? Restart;
        public event Action? Exit;
    }
}