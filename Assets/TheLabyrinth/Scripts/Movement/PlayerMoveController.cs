using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth.Movement
{
    using Abstractions;

    public class PlayerMoveController : MonoBehaviour, IPlayerMoveController
    {
        [Inject]
        public void Initialize(ILogger logger)
        {
            _logger = logger;
        }
        public IMoveStrategy MoveStrategy
        {
            get => _moveBehaviour;
            set
            {
                if (value is MoveBehaviour playerMoveBehaviour)
                {
                    if (_moveBehaviour != null)
                    {
                        _moveBehaviour.enabled = false;
                        _logger.Log($"{_moveBehaviour.gameObject.name} disabled");
                    }
                    _moveBehaviour = playerMoveBehaviour;
                    _moveBehaviour.enabled = true;
                    _logger.Log($"{_moveBehaviour.gameObject.name} enabled");
                }
            }
        }
        public void SetStrategy(IMoveStrategy strategy) => MoveStrategy = strategy;


        private MoveBehaviour _moveBehaviour;
        private ILogger _logger;
    }
}