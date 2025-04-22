#nullable enable

namespace KarenKrill.TheLabyrinth.UI.Views.Abstractions
{
    using Common.UI.Views.Abstractions;

    public interface IDiagnosticInfoView : IView
    {
        public string FpsText { set; }
    }
}