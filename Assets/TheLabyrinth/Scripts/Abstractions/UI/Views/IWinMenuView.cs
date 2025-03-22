#nullable enable

using System;
using KarenKrill.Common.UI.Views.Abstractions;

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    public interface IWinMenuView : IUserInterfaceView
    {
        public event Action? Restart;
        public event Action? Exit;
    }
}