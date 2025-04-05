using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth.GameFlow
{
    using Abstractions;
    using MazeGeneration;

    public class LoadLevelManager : MonoBehaviour, ILevelManager
    {
        public int TotalMazeCellsCount => _mazeBuilder.TotalCellsCount;

#nullable enable
        public event Action? LevelLoaded;
        public event Action? LevelUnloaded;
#nullable restore

        [Inject]
        public void Initialize(ILogger logger)
        {
            _logger = logger;
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
            //Time.timeScale = 0;
            _playerController.LockMovement(xAxis: true, yAxis: true, zAxis: true);
            yield return _mazeBuilder.RebuildCoroutine();
            var playerSpawnPoint = _mazeBuilder.GetCellCenter(_mazeBuilder.Levels - 1, 0);
            _playerController.LockMovement(xAxis: true, yAxis: false, zAxis: true);
            yield return _playerController.ForcedMove(new Vector3(playerSpawnPoint.x, 100, playerSpawnPoint.y));
            if (_exitPointTransform != null)
            {
                var exitCellCenter = _mazeBuilder.GetCellCenter(0, 0);
                _exitPointTransform.position = new Vector3(exitCellCenter.x, _exitPointTransform.position.y, exitCellCenter.y);
            }
            yield return new WaitForSeconds(0.5f); // wait while player isn't fall (stucks in air)
            yield return new WaitUntil(() => _playerController.IsGrounded);// wait until player isn't grounded
            _logger.Log($"{nameof(LoadLevelManager)}.{nameof(LoadLevelCoroutine)} ends, now level plays");
            LevelLoaded?.Invoke();
        }
        private IEnumerator FinishLevelCoroutine()
        {
            //Time.timeScale = 0;
            _playerController.LockMovement();
            yield return _playerController.ForcedMove(new Vector3(0, 1, 60));
            _mazeBuilder.Levels = _mazeLevelsCount < _mazeMaxLevelsCount ? ++_mazeLevelsCount : _mazeMaxLevelsCount;
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
        private int _mazeLevelsCount = 0;
    }
}