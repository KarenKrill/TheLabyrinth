#nullable enable

using System;

namespace KarenKrill.UI.Views
{
    using Core.UI.Views;
    public interface ILooseMenuView : IUserInterfaceView
    {
        public event Action? Restart;
        public event Action? Exit;
    }
}