using UnityEngine;
using Zenject;
using KarenKrill.Common.GameLevel;
using KarenKrill.Common.UI.Views.Abstractions;
using KarenKrill.TheLabyrinth.UI.Presenters;
using KarenKrill.TheLabyrinth.UI.Views.Abstractions;
using KarenKrill.TheLabyrinth.GameFlow.Abstractions;

namespace KarenKrill.TheLabyrinth.UI
{
    public class WindowManager : MonoBehaviour
    {
        [Inject]
        IGameFlow _gameFlow;
        [Inject]
        ILogger _logger;
        [Inject]
        IUserInterfaceFactory _userInterfaceFactory;
        [Inject]
        ITimeLimitedLevelController _timeLimitedLevelController;
        [Inject]
        IGameController _gameController;
        
        MainMenuPresenter _mainMenuPresenter;
        PauseMenuPresenter _pauseMenuPresenter;
        WinMenuPresenter _winMenuPresenter;
        LooseMenuPresenter _looseMenuPresenter;
        LevelInfoPresenter _levelInfoPresenter;

        public void Awake()
        {
            _gameFlow.MainMenuLoad += OnMainMenuLoad;
            _gameFlow.LevelPlay += OnLevelPlay;
            _gameFlow.LevelPause += OnLevelPause;
            _gameFlow.PlayerWin += OnPlayerWin;
            _gameFlow.PlayerLoose += OnPlayerLoose;
            _gameFlow.LevelLoad += OnLevelLoad;

            _mainMenuPresenter = new(_logger, _gameFlow);
            _pauseMenuPresenter = new(_logger, _gameFlow);
            _winMenuPresenter = new(_gameFlow);
            _looseMenuPresenter = new(_gameFlow);
            _levelInfoPresenter = new(_timeLimitedLevelController, _gameController);
            _mainMenuPresenter.View = _viewFactory.Create<IMainMenuView>();
            _looseMenuPresenter.View = _viewFactory.Create<ILooseMenuView>();
            _levelInfoPresenter.View = _viewFactory.Create<IILevelInfoView>();
            _pauseMenuPresenter.View = _viewFactory.Create<IPauseMenuView>();
            _winMenuPresenter.View = _viewFactory.Create<IWinMenuView>();
        }

        private void OnLevelLoad()
        {
            _levelInfoPresenter.Enable();
        }

        public void Start()
        {
            _gameFlow.LoadMainMenu();
        }

        private void OnMainMenuLoad()
        {
            _mainMenuPresenter.Enable();
            //_gameFlow.StartGame();
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