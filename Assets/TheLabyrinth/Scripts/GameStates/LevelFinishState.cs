﻿using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using GameFlow.Abstractions;

    public class LevelFinishState : IStateHandler<GameState>
    {
        public GameState State => GameState.LevelFinish;

        public LevelFinishState(ILogger logger, IGameFlow gameFlow, IGameController gameController)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _gameController = gameController;
        }

        public void Enter()
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _gameController.OnLevelFinish();
        }

        public void Exit()
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
        }

        private readonly ILogger _logger;
        IGameFlow _gameFlow;
        IGameController _gameController;
    }
}
