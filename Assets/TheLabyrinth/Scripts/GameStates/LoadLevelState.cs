namespace KarenKrill.TheLabyrinth.GameStates
{
    using StateMachine.Abstractions;
    using GameFlow.Abstractions;
    using KarenKrill.TheLabyrinth.UI.Presenters;
    using KarenKrill.TheLabyrinth.UI.Views.Abstractions;
    using KarenKrill.Common.UI.Presenters.Abstractions;
    using KarenKrill.Common.UI.Views.Abstractions;
    using UnityEngine;

    public class LoadLevelState : IStateHandler<GameState>
    {
        public GameState State => GameState.LevelLoad;

        public LoadLevelState(ILogger logger,
            IGameFlow gameFlow,
            ILevelManager levelManager,
            IViewFactory viewFactory,
            IPresenter<IILevelInfoView> levelInfoPresenter,
            IGameController gameController)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _levelManager = levelManager;
            _viewFactory = viewFactory;
            _levelInfoPresenter = levelInfoPresenter;
            _gameController = gameController;
        }

        public void Enter()
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _levelManager.LevelLoaded += OnLevelLoaded;
            _levelManager.OnLevelLoad();
            _levelInfoPresenter.View ??= _viewFactory.Create<IILevelInfoView>();
            _levelInfoPresenter.Enable();
            _gameController.OnLevelLoad();
        }

        public void Exit()
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _levelManager.LevelLoaded -= OnLevelLoaded;
        }

        private readonly ILogger _logger;
        IGameFlow _gameFlow;
        ILevelManager _levelManager;
        IViewFactory _viewFactory;
        IPresenter<IILevelInfoView> _levelInfoPresenter;
        IGameController _gameController;

        private void OnLevelLoaded()
        {
            _gameFlow.PlayLevel();
        }
    }
}
