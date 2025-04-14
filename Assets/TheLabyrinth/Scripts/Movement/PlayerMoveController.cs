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
            get => _moveStrategy;
            set
            {
                if (_moveStrategy != value)
                {
                    if (_moveStrategy is MoveBehaviour prevMoveBehaviour)
                    {
                        prevMoveBehaviour.enabled = false;
                        _logger.Log($"{prevMoveBehaviour.gameObject.name} disabled");
                    }
                    _moveStrategy = value;
                    if (value is MoveBehaviour moveBehaviour)
                    {
                        moveBehaviour.enabled = true;
                        _logger.Log($"{moveBehaviour.gameObject.name} enabled");
                    }
                }
            }
        }
        public void SetStrategy(IMoveStrategy strategy) => MoveStrategy = strategy;


        private IMoveStrategy _moveStrategy;
        private ILogger _logger;
    }
}