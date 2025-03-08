using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace KarenKrill.Core
{
    public class WindowManager : MonoBehaviour
    {
        [Inject]
        IGameFlow _gameFlow;
        [Inject]
        ILogger _logger;
        [SerializeField]
        GameObject _mainMenuWindow, _pauseWindow, _winWindow, _looseWindow;
        public void Awake()
        {
            _gameFlow.MainMenuLoad += OnMainMenuLoad;
            _gameFlow.GameStart += OnGameStart;
            _gameFlow.LevelPlay += OnLevelPlay;
            _gameFlow.LevelPause += OnLevelPause;
            _gameFlow.PlayerWin += OnPlayerWin;
            _gameFlow.PlayerLoose += OnPlayerLoose;
        }
        public void Start()
        {
            _gameFlow.LoadMainMenu();
        }

        private void OnMainMenuLoad()
        {
            //_mainMenuWindow.SetActive(true);
            _gameFlow.StartGame();
        }
        private void OnGameStart()
        {
            _winWindow.SetActive(false);
            _looseWindow.SetActive(false);
            _pauseWindow.SetActive(false);
        }
        private void OnLevelPlay()
        {
            _pauseWindow.SetActive(false);
        }
        private void OnLevelPause()
        {
            _pauseWindow.SetActive(true);
        }
        private void OnPlayerLoose()
        {
            _looseWindow.SetActive(true);
        }
        private void OnPlayerWin()
        {
            _winWindow.SetActive(true);
        }
    }
}
