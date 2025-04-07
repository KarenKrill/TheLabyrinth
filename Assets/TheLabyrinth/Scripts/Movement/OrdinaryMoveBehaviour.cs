using UnityEngine;

namespace KarenKrill.TheLabyrinth.Movement
{
    using Abstractions;

    public class OrdinaryMoveBehaviour : MonoBehaviour, IMoveStrategy
    {
        public bool UsePhysics { get => _usePhysics; set => _usePhysics = value; }

        [SerializeField]
        private bool _usePhysics;
    }
}
