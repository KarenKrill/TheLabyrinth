using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth.Movement
{
    using Abstractions;
    using Common.Logging;

    public class ManualMoveController : MoveBehaviour, IManualMoveStrategy
    {
        public void Move(Vector3 dest)
        {
            _transform.position = dest;
        }

        [SerializeField]
        private Transform _transform;
    }
}