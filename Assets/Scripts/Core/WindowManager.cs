using UnityEngine;
using Zenject;
using KarenKrill.Core.Level;
using KarenKrill.Core.UI;
using KarenKrill.UI.Presenters;
using KarenKrill.UI.Views;

namespace KarenKrill.Core
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

            var mainMenuView = _userInterfaceFactory.Create<IMainMenuView>();
            _mainMenuPresenter = new(mainMenuView, _logger, _gameFlow);
            var pauseMenuView = _userInterfaceFactory.Create<IPauseMenuView>();
            _pauseMenuPresenter = new(pauseMenuView, _logger, _gameFlow);
            var winMenuView = _userInterfaceFactory.Create<IWinMenuView>();
            _winMenuPresenter = new(winMenuView, _gameFlow);
            var looseMenuView = _userInterfaceFactory.Create<ILooseMenuView>();
            _looseMenuPresenter = new(looseMenuView, _gameFlow);
            var levelInfoView = _userInterfaceFactory.Create<IILevelInfoView>();
            _levelInfoPresenter = new(levelInfoView, _timeLimitedLevelController, _gameController);
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