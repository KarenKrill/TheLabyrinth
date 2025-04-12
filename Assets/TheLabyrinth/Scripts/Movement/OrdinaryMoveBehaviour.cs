using UnityEngine;

namespace KarenKrill.TheLabyrinth.Movement
{
    using Abstractions;

    public class OrdinaryMoveBehaviour : MoveBehaviour, IMoveStrategy
    {
        public bool UsePhysics { get => _usePhysics; set => _usePhysics = value; }

        [SerializeField]
        private bool _usePhysics;
    }
}
