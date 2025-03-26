using KarenKrill.Common.UI.Views.Abstractions;
using KarenKrill.Common.UI.Presenters.Abstractions;
using KarenKrill.TheLabyrinth.UI.Views.Abstractions;
using KarenKrill.TheLabyrinth.GameFlow.Abstractions;

namespace KarenKrill.TheLabyrinth.UI
{
    public class PresenterManager
    {
        IGameFlow _gameFlow;
        IViewFactory _viewFactory;
        IPresenter<IMainMenuView> _mainMenuPresenter;
        IPresenter<IPauseMenuView> _pauseMenuPresenter;
        IPresenter<IWinMenuView> _winMenuPresenter;
        IPresenter<ILooseMenuView> _looseMenuPresenter;
        IPresenter<IILevelInfoView> _levelInfoPresenter;
        public PresenterManager(IViewFactory viewFactory,
            IPresenter<IMainMenuView> mainMenuPresenter,
            IPresenter<IPauseMenuView> pauseMenuPresenter,
            IPresenter<IWinMenuView> winMenuPresenter,
            IPresenter<ILooseMenuView> looseMenuPresenter,
            IPresenter<IILevelInfoView> levelInfoPresenter,
            IGameFlow gameFlow)
        {
            _viewFactory = viewFactory;
            _mainMenuPresenter = mainMenuPresenter;
            _pauseMenuPresenter = pauseMenuPresenter;
            _winMenuPresenter = winMenuPresenter;
            _looseMenuPresenter = looseMenuPresenter;
            _levelInfoPresenter = levelInfoPresenter;
            _gameFlow = gameFlow;

            _gameFlow.MainMenuLoad += OnMainMenuLoad;
            _gameFlow.LevelPlay += OnLevelPlay;
            _gameFlow.LevelPause += OnLevelPause;
            _gameFlow.PlayerWin += OnPlayerWin;
            _gameFlow.PlayerLoose += OnPlayerLoose;
            _gameFlow.LevelLoad += OnLevelLoad;
        }
        private void OnMainMenuLoad()
        {
            _mainMenuPresenter.View ??= _viewFactory.Create<IMainMenuView>();
            _looseMenuPresenter.View ??= _viewFactory.Create<ILooseMenuView>();
            _levelInfoPresenter.View ??= _viewFactory.Create<IILevelInfoView>();
            _pauseMenuPresenter.View ??= _viewFactory.Create<IPauseMenuView>();
            _winMenuPresenter.View ??= _viewFactory.Create<IWinMenuView>();
            _mainMenuPresenter.Enable();
        }

        private void OnLevelLoad()
        {
            _levelInfoPresenter.Enable();
        }

        bool _itWasPaused = false;
        private void OnLevelPlay()
        {
            if (_itWasPaused)
            {
                _pauseMenuPresenter.Disable();
            }
        }
        private void OnLevelPause()
        {
            _itWasPaused = true;
            _pauseMenuPresenter.Enable();
        }
        private void OnPlayerLoose()
        {
            _looseMenuPresenter.Enable();
            _levelInfoPresenter.Disable();
        }
        private void OnPlayerWin()
        {
            _winMenuPresenter.Enable();
            _levelInfoPresenter.Disable();
        }
    }
}