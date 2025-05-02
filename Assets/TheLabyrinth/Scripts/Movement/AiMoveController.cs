using UnityEngine;
using UnityEngine.AI;

using KarenKrill.Utilities;

namespace TheLabyrinth.Movement
{
    using Abstractions;
    
    /// <summary>
    /// Makes an unit to chase a target
    /// </summary>
    /// <remarks>
    /// To start/stop <see cref="NavMeshAgent"/> moving use Enable/Disable
    /// </remarks>
    public class AiMoveController : OrdinaryMoveBehaviour, IAiMoveStrategy
    {
        public float MinimumSpeed { get => _minimumSpeed; set => _minimumSpeed = value; }

        [SerializeField]
        private float _minimumSpeed = 5f;
        [SerializeField]
        private NavMeshAgent _navAgent;
        [SerializeField]
        private Transform _destination;
#if DEBUG
        [SerializeField]
        private bool _showPath;

        private NavMeshPath _activePath;
#endif

        protected override void Awake()
        {
            base.Awake();
            _navAgent.enabled = false;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            if (_navAgent.isOnNavMesh)
            {
                _navAgent.isStopped = false;
            }
            _navAgent.enabled = true;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            if (!_navAgent.IsNullOrDestroyed())
            {
                if (_navAgent.isOnNavMesh)
                {
                    _navAgent.isStopped = true;
                }
                _navAgent.enabled = false;
            }
        }
        protected override void Update()
        {
            if (_navAgent.isOnNavMesh)
            {
                if (_navAgent.destination != _destination.position)
                {
                    OnTargetChanged();
                }
                else if (_navAgent.hasPath)
                {
                    MovementUpdate();
                }
#if DEBUG
                DrawActivePath();
#endif
            }
        }

        private void OnTargetChanged()
        {
#if DEBUG
            _activePath = new();
            _ = _navAgent.CalculatePath(_destination.position, _activePath);
#endif
            _navAgent.destination = _destination.position;
            _navAgent.acceleration = Random.Range(_minimumSpeed, MaximumSpeed);
            _navAgent.speed = Random.Range(_minimumSpeed, MaximumSpeed);
            _navAgent.isStopped = false;
        }
        private void MovementUpdate()
        {
            _navAgent.acceleration = Random.Range(_minimumSpeed, MaximumSpeed);
            _navAgent.speed = Random.Range(_minimumSpeed, MaximumSpeed);
            if (_navAgent.isStopped)
            {
                _navAgent.isStopped = false;
            }
        }
#if DEBUG
        private void DrawActivePath()
        {
            if (_showPath && _activePath.corners.Length > 0)
            {
                for (int i = 1; i < _activePath.corners.Length; i++)
                {
                    Debug.DrawLine(_activePath.corners[i - 1], _activePath.corners[i], Color.red);
                }
            }
        }
#endif
    }
}