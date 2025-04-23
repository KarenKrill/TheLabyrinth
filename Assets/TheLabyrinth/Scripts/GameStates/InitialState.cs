using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views.Abstractions;
    using GameFlow.Abstractions;
    using TheLabyrinth.Abstractions;
    using UI.Views.Abstractions;

    public class InitialState : IStateHandler<GameState>
    {
        public GameState State => GameState.Initial;

        public InitialState(ILogger logger,
            IGameFlow gameFlow,
            IPresenter<IDiagnosticInfoView> diagnosticInfoPresenter,
            IViewFactory viewFactory,
            GameSettings gameSettings)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _diagnosticInfoPresenter = diagnosticInfoPresenter;
            _viewFactory = viewFactory;
            gameSettings.ShowFpsChanged += OnShowFpsChanged;
        }
        public void Enter(GameState prevState)
        {
            _logger.Log($"{nameof(InitialState)}.{nameof(Enter)}()");
            _gameFlow.LoadMainMenu();
        }
        public void Exit(GameState nextState)
        {
            _logger.Log($"{nameof(InitialState)}.{nameof(Exit)}()");
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly IPresenter<IDiagnosticInfoView> _diagnosticInfoPresenter;
        private readonly IViewFactory _viewFactory;

        private void OnShowFpsChanged(bool state)
        {
            if (state)
            {
                _diagnosticInfoPresenter.View ??= _viewFactory.Create<IDiagnosticInfoView>();
                _diagnosticInfoPresenter.Enable();
            }
            else if (_diagnosticInfoPresenter.View is not null)
            {
                _diagnosticInfoPresenter.Disable();
            }
        }
    }
}