#nullable enable
using System;

namespace KarenKrill.UI.Views
{
    using Core.UI.Views;
    public interface IWinMenuView : IUserInterfaceView
    {
        public event Action? Restart;
        public event Action? Exit;
    }
}