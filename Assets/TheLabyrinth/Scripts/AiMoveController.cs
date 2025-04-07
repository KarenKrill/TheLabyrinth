using UnityEngine;
using UnityEngine.AI;

namespace KarenKrill.TheLabyrinth.Movement
{
    using Abstractions;
    using Common.Utilities;

    /// <summary>
    /// Makes an unit to chase a target
    /// </summary>
    /// <remarks>
    /// To start/stop <see cref="NavMeshAgent"/> moving use Enable/Disable
    /// </remarks>
    public class AiMoveController : OrdinaryMoveBehaviour, IAiMoveStrategy
    {
        public float MaximumSpeed { get => _maximumSpeed; set => _maximumSpeed = value; }
        public float MinimumSpeed { get => _minimumSpeed; set => _minimumSpeed = value; }

        [SerializeField]
        private float _maximumSpeed = 5f;
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

        private void Awake()
        {
            _navAgent.enabled = false;
        }
        private void OnEnable()
        {
            if (_navAgent.isOnNavMesh)
            {
                _navAgent.isStopped = false;
            }
            _navAgent.enabled = true;
        }
        private void OnDisable()
        {
            if (!_navAgent.IsNullOrDestroyed())
            {
                if (_navAgent.isOnNavMesh)
                {
                    _navAgent.isStopped = true;
                }
                _navAgent.enabled = false;
            }
        }
        private void Update()
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
            _navAgent.acceleration = Random.Range(_minimumSpeed, _maximumSpeed);
            _navAgent.speed = Random.Range(_minimumSpeed, _maximumSpeed);
            _navAgent.isStopped = false;
        }
        private void MovementUpdate()
        {
            _navAgent.acceleration = Random.Range(_minimumSpeed, _maximumSpeed);
            _navAgent.speed = Random.Range(_minimumSpeed, _maximumSpeed);
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