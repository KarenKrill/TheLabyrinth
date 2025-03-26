#nullable enable

using System;
using KarenKrill.Common.UI.Views.Abstractions;

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    public interface IPauseMenuView : IView
    {
        public event Action? Resume;
        public event Action? Settings;
        public event Action? Restart;
        public event Action? Exit;
    }
}