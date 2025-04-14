using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth.GameFlow
{
    using Abstractions;
    using Movement.Abstractions;
    using MazeGeneration;
    using Movement;

    public class LoadLevelManager : MonoBehaviour, ILevelManager
    {
        public int TotalMazeCellsCount => _mazeBuilder.TotalCellsCount;

#nullable enable
        public event Action? LevelLoaded;
        public event Action? LevelUnloaded;
#nullable restore

        [Inject]
        public void Initialize(ILogger logger,
            IPlayerMoveController playerMoveController,
            IManualMoveStrategy manualMoveStrategy,
            IPhysicMoveStrategy physicMoveStrategy,
            IPlayerInputMoveStrategy playerInputMoveStrategy)
        {
            _logger = logger;
            _playerMoveController = playerMoveController;
            _manualMoveStrategy = manualMoveStrategy;
            _physicMoveStrategy = physicMoveStrategy;
            _playerInputMoveStrategy = playerInputMoveStrategy;
        }

        public void Reset()
        {
            ResetToDefaults();
        }
        public void OnLevelLoad() => StartCoroutine(LoadLevelCoroutine());
        public void OnLevelEnd() => StartCoroutine(FinishLevelCoroutine());

        private void ResetToDefaults()
        {
            _mazeLevelsCount = _mazeMinLevelsCount;
            _mazeBuilder.Levels = _mazeLevelsCount;
        }
        private IEnumerator LoadLevelCoroutine()
        {
            _logger.Log($"{nameof(LoadLevelManager)}.{nameof(LoadLevelCoroutine)}");
            var previousMoveStrategy = _playerMoveController.MoveStrategy;
            //Time.timeScale = 0;
            _playerMoveController.MoveStrategy = _manualMoveStrategy;
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
            yield return new WaitUntil(() => _playerController.IsGrounded);// wait until player isn't grounded
            _playerMoveController.MoveStrategy = previousMoveStrategy;
            LevelLoaded?.Invoke();
        }
        private IEnumerator FinishLevelCoroutine()
        {
            var previousMoveStrategy = _playerMoveController.MoveStrategy;
            //Time.timeScale = 0;
            _playerMoveController.MoveStrategy = _manualMoveStrategy;
            _manualMoveStrategy.Move(new Vector3(0, 1, 60));
            yield return null;
            _mazeBuilder.Levels = _mazeLevelsCount < _mazeMaxLevelsCount ? ++_mazeLevelsCount : _mazeMaxLevelsCount;
            _playerMoveController.MoveStrategy = previousMoveStrategy;
            LevelUnloaded?.Invoke();
        }

        [SerializeField]
        private CharacterMoveController _playerController;
        [SerializeReference]
        private ArcMazeBuilder _mazeBuilder;
        [SerializeField]
        private Transform _exitPointTransform;
        [SerializeField]
        private int _mazeMinLevelsCount = 4;
        [SerializeField]
        private int _mazeMaxLevelsCount = 13;

        private ILogger _logger;
        private IPlayerMoveController _playerMoveController;
        private IManualMoveStrategy _manualMoveStrategy;
        private IPhysicMoveStrategy _physicMoveStrategy;
        private IPlayerInputMoveStrategy _playerInputMoveStrategy;
        private int _mazeLevelsCount = 0;
    }
}