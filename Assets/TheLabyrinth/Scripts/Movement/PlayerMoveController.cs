using UnityEngine;

namespace KarenKrill.TheLabyrinth.Movement
{
    using Abstractions;

    public class PlayerMoveController : MonoBehaviour, IPlayerMoveController
    {
        public IMoveStrategy MoveStrategy
        {
            get => _moveBehaviour;
            set
            {
                if (value is OrdinaryMoveBehaviour playerMoveBehaviour)
                {
                    if (_moveBehaviour != null)
                    {
                        _moveBehaviour.enabled = false;
                    }
                    _moveBehaviour = playerMoveBehaviour;
                    _moveBehaviour.enabled = true;
                }
            }
        }
        public void SetStrategy(IMoveStrategy strategy) => MoveStrategy = strategy;


        private OrdinaryMoveBehaviour _moveBehaviour;
    }
}