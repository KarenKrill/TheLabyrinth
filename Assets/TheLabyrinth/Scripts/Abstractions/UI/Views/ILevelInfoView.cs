#nullable enable

using System.Drawing;

using KarenKrill.UI.Views.Abstractions;

namespace TheLabyrinth.UI.Views.Abstractions
{
    public interface IILevelInfoView : IView
    {
        public string Title { set; }
        public string RemainingTimeText { set; }
        public Color RemainingTimeTextColor { set; }
    }
}