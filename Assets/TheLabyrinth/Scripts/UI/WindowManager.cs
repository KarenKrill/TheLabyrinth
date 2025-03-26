using KarenKrill.Common.UI.Views.Abstractions;
using KarenKrill.Common.UI.Presenters.Abstractions;
using KarenKrill.TheLabyrinth.UI.Views.Abstractions;
using KarenKrill.TheLabyrinth.GameFlow.Abstractions;

namespace KarenKrill.TheLabyrinth.UI
{
    public class WindowManager
    {
        IGameFlow _gameFlow;
        IUserInterfaceFactory _userInterfaceFactory;
        IPresenter<IMainMenuView> _mainMenuPresenter;
        IPresenter<IPauseMenuView> _pauseMenuPresenter;
        IPresenter<IWinMenuView> _winMenuPresenter;
        IPresenter<ILooseMenuView> _looseMenuPresenter;
        IPresenter<IILevelInfoView> _levelInfoPresenter;
        public WindowManager(IUserInterfaceFactory userInterfaceFactory,
            IPresenter<IMainMenuView> mainMenuPresenter,
            IPresenter<IPauseMenuView> pauseMenuPresenter,
            IPresenter<IWinMenuView> winMenuPresenter,
            IPresenter<ILooseMenuView> looseMenuPresenter,
            IPresenter<IILevelInfoView> levelInfoPresenter,
            IGameFlow gameFlow)
        {
            _userInterfaceFactory = userInterfaceFactory;
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

        private void OnLevelLoad()
        {
            _levelInfoPresenter.Enable();
        }
        private void OnMainMenuLoad()
        {
            _mainMenuPresenter.View ??= _userInterfaceFactory.Create<IMainMenuView>();
            _looseMenuPresenter.View ??= _userInterfaceFactory.Create<ILooseMenuView>();
            _levelInfoPresenter.View ??= _userInterfaceFactory.Create<IILevelInfoView>();
            _pauseMenuPresenter.View ??= _userInterfaceFactory.Create<IPauseMenuView>();
            _winMenuPresenter.View ??= _userInterfaceFactory.Create<IWinMenuView>();
            _mainMenuPresenter.Enable();
        }
        bool _isPaused = false;
        private void OnLevelPlay()
        {
            if (_isPaused)
            {
                _pauseMenuPresenter.Disable();
            }
        }
        private void OnLevelPause()
        {
            _isPaused = true;
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