#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    using Common.UI.Views.Abstractions;

    public interface ILooseMenuView : IView
    {
        public event Action? Restart;
        public event Action? Exit;
    }
}