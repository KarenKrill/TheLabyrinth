#nullable enable

namespace KarenKrill.UI.Views
{
    using Core.UI.Views;
    using System.Drawing;

    public interface IILevelInfoView : IUserInterfaceView
    {
        public string Title { set; }
        public string RemainingTimeText { set; }
        public Color RemainingTimeTextColor { set; }
    }
}