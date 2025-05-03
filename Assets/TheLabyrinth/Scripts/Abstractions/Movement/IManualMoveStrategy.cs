using UnityEngine;

namespace TheLabyrinth.Movement.Abstractions
{
    public interface IManualMoveStrategy : IMoveStrategy
    {
        public void Move(Vector3 destination);
    }
}
