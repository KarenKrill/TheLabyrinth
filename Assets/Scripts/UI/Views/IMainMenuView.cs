#nullable enable
using System;

namespace KarenKrill.UI.Views
{
    using Core.UI.Views;
    public interface IMainMenuView : IUserInterfaceView
    {
        public event Action? NewGame;
        public event Action? Exit;
        public event Action? Settings;
    }
}