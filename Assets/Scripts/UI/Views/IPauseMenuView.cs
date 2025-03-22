#nullable enable
using System;

namespace KarenKrill.UI.Views
{
    using Core.UI.Views;
    public interface IPauseMenuView : IUserInterfaceView
    {
        public event Action? Resume;
        public event Action? Settings;
        public event Action? Restart;
        public event Action? Exit;
    }
}