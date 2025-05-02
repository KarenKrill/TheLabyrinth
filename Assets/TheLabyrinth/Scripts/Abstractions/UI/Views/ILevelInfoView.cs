#nullable enable

using System.Drawing;

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    using UI.Views.Abstractions;

    public interface IILevelInfoView : IView
    {
        public string Title { set; }
        public string RemainingTimeText { set; }
        public Color RemainingTimeTextColor { set; }
    }
}