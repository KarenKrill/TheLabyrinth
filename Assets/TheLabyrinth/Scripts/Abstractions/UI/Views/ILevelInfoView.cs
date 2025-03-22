#nullable enable

using System.Drawing;
using KarenKrill.Common.UI.Views.Abstractions;

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    public interface IILevelInfoView : IUserInterfaceView
    {
        public string Title { set; }
        public string RemainingTimeText { set; }
        public Color RemainingTimeTextColor { set; }
    }
}