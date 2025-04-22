namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using Common.Diagnostics.Abstractions;
    using Common.UI.Presenters.Abstractions;
    using Views.Abstractions;

    public class DiagnosticInfoPresenter : IPresenter<IDiagnosticInfoView>
    {
        public IDiagnosticInfoView View { get; set; }

        public DiagnosticInfoPresenter(IDiagnosticsProvider diagnosticsProvider)
        {
            _diagnosticsProvider = diagnosticsProvider;
        }
        public void Enable()
        {
            OnPerfomanceInfoChanged(_diagnosticsProvider.PerfomanceInfo);
            _diagnosticsProvider.PerfomanceInfoChanged += OnPerfomanceInfoChanged;
            View.Show();
        }
        public void Disable()
        {
            View.Close();
            _diagnosticsProvider.PerfomanceInfoChanged -= OnPerfomanceInfoChanged;
        }

        private readonly IDiagnosticsProvider _diagnosticsProvider;
        private void OnPerfomanceInfoChanged(PerfomanceInfo perfomanceInfo)
        {
            View.FpsText = $"FpsAvg: {perfomanceInfo.FpsAverage:0.0}";
        }
    }
}