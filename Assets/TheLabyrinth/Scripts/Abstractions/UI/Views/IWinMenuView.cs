#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    using Common.UI.Views.Abstractions;

    public interface IWinMenuView : IView
    {
        public event Action? Restart;
        public event Action? Exit;
    }
}