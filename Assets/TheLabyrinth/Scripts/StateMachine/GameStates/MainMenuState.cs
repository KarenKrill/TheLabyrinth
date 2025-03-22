using UnityEngine;
using Zenject;
using KarenKrill.TheLabyrinth.StateMachine;
using KarenKrill.Common.UI.Views.Abstractions;
using KarenKrill.TheLabyrinth.UI.Views.Abstractions;
using KarenKrill.TheLabyrinth.StateMachine.Abstractions;
using KarenKrill.TheLabyrinth.GameFlow.Abstractions;

namespace KarenKrill.TheLabyrinth.GameStates
{
    public class MainMenuState : IGameState
    {
        [Inject]
        IUserInterfaceFactory _userInterfaceFactory;
        [Inject]
        IGameApp _application;
        [Inject]
        ILogger _logger;
        IMainMenuView _mainMenuView;
        public void Enter()
        {
            _mainMenuView = _userInterfaceFactory.Create<IMainMenuView>();
            _mainMenuView.Exit += OnExit;
            _mainMenuView.Settings += OnSettings;
            _mainMenuView.NewGame += OnNewGame;
            _mainMenuView.Show();
        }
        private void OnNewGame()
        {
            _logger.Log("OnNewGame");
        }
        private void OnSettings()
        {
            _logger.Log("OnNewGame");
        }
        private void OnExit()
        {
            _application.Quit();
        }
    }
}