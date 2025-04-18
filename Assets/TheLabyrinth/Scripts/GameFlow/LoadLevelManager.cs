using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth.GameFlow
{
    using Abstractions;
    using Common.Logging;
    using Movement.Abstractions;
    using MazeGeneration;

    public class LoadLevelManager : MonoBehaviour, ILevelManager
    {
        public int TotalMazeCellsCount => _mazeBuilder.TotalCellsCount;

#nullable enable
        public event Action? LevelLoaded;
        public event Action? LevelUnloaded;
#nullable restore

        [Inject]
        public void Initialize(ILogger logger,
            IGameInfoProvider gameInfoProvider,
            IPlayerMoveController playerMoveController,
            IManualMoveStrategy manualMoveStrategy,
            IPhysicMoveStrategy physicMoveStrategy)
        {
            _logger = logger;
            _gameInfoProvider = gameInfoProvider;
            _playerMoveController = playerMoveController;
            _manualMoveStrategy = manualMoveStrategy;
            _physicMoveStrategy = physicMoveStrategy;
        }

        public void LoadLevel() => StartCoroutine(LoadLevelCoroutine());
        public void UnloadLevel() => StartCoroutine(FinishLevelCoroutine());

        private IEnumerator LoadLevelCoroutine()
        {
            _logger.Log($"{nameof(LoadLevelManager)}.{nameof(LoadLevelCoroutine)}");
            var previousMoveStrategy = _playerMoveController.MoveStrategy;
            _playerMoveController.MoveStrategy = _manualMoveStrategy;
            _mazeBuilder.Levels = _gameInfoProvider.CurrentLevel.MazeLevelsCount;
            _logger.LogWarning($"Load {_gameInfoProvider.CurrentLevel.MazeShape} level {_gameInfoProvider.CurrentLevel.Index} with {_gameInfoProvider.CurrentLevel.MazeLevelsCount} maze levels count");
            yield return _mazeBuilder.RebuildCoroutine();
            var playerSpawnPoint = _mazeBuilder.GetCellCenter(_mazeBuilder.Levels - 1, 0);
            _manualMoveStrategy.Move(new Vector3(playerSpawnPoint.x, 100, playerSpawnPoint.y));
            _playerMoveController.MoveStrategy = _physicMoveStrategy;
            if (_exitPointTransform != null)
            {
                var exitCellCenter = _mazeBuilder.GetCellCenter(0, 0);
                _exitPointTransform.position = new Vector3(exitCellCenter.x, _exitPointTransform.position.y, exitCellCenter.y);
            }
            yield return null; // wait frame for CharacterController.IsGrounded
            yield return new WaitUntil(() => _physicMoveStrategy.IsGrounded);// wait until player isn't grounded
            _playerMoveController.MoveStrategy = previousMoveStrategy;
            LevelLoaded?.Invoke();
        }
        private IEnumerator FinishLevelCoroutine()
        {
            var previousMoveStrategy = _playerMoveController.MoveStrategy;
            _playerMoveController.MoveStrategy = _manualMoveStrategy;
            _manualMoveStrategy.Move(new Vector3(0, 1, 60));
            yield return null;
            _playerMoveController.MoveStrategy = previousMoveStrategy;
            LevelUnloaded?.Invoke();
        }

        [SerializeReference]
        private ArcMazeBuilder _mazeBuilder;
        [SerializeField]
        private Transform _exitPointTransform;

        private ILogger _logger;
        private IGameInfoProvider _gameInfoProvider;
        private IPlayerMoveController _playerMoveController;
        private IManualMoveStrategy _manualMoveStrategy;
        private IPhysicMoveStrategy _physicMoveStrategy;
    }
}