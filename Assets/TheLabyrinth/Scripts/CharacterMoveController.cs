using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace KarenKrill.TheLabyrinth.Movement
{
    using Abstractions;
    using Common.Logging;
    using Input.Abstractions;

    /// <summary>
    /// Controls player movement (with CharacterController):
    /// <list type="bullet">
    /// <item>Controls player ai move (with NavMeshAgent)</item>
    /// <item>Controls manual move</item>
    /// <item>Controls player animation move</item>
    /// <item>Controls player run</item>
    /// <item>Controls player sloping</item>
    /// <item>Controls player jumping</item>
    /// </list>
    /// </summary>
    public class CharacterMoveController : OrdinaryMoveBehaviour, IPlayerInputMoveStrategy
    {
        public float MaximumSpeed { get => _maximumSpeed; set => _maximumSpeed = value; }
        public bool IsGrounded => _characterController.isGrounded;
        public bool UseAiNavigation { get => _useAiNavigation; set => _useAiNavigation = value; }
        public float AiMinSpeed { get => _aiMinSpeed; set => _aiMinSpeed = value; }

        [Inject]
        public void Initialize(ILogger logger, IInputActionService inputActionService)
        {
            _logger = logger;
            _inputActionService = inputActionService;
        }
        public IEnumerator ForcedMove(Vector3 dest)
        {
            _IsForcedMoveActive = true;
            yield return new WaitForSeconds(0.1f);
            _characterController.transform.position = dest;
            yield return new WaitForSeconds(0.1f);
            _IsForcedMoveActive = false;
        }
        public void LockMovement(bool xAxis = true, bool yAxis = true, bool zAxis = true)
        {
            _xAxisLocked = xAxis;
            _yAxisLocked = yAxis;
            _zAxisLocked = zAxis;
            _logger.LogWarning($"Move locked: ({xAxis},{yAxis},{zAxis})");
        }
        public void UnlockMovement()
        {
            _xAxisLocked = false;
            _yAxisLocked = false;
            _zAxisLocked = false;
            _logger.LogWarning("Move unlocked");
        }

        [SerializeField]
        private CharacterController _characterController;
        [SerializeField]
        private Animator _animator = null;
        [SerializeField]
        private float _maximumSpeed = 5f, _rotationDegreeSpeed = 360.0f;
        [SerializeField]
        private float _jumpHeight = 2.0f, _jumpHorizontalSpeed = 3.0f, _gravityMultiplier = 1.5f;
        [SerializeField]
        private float _jumpButtonGracePeriod = 0.2f;
        [SerializeField]
        private float _slidingDecelerationFactor = 3f;
        [SerializeField]
        private bool _useRootMotion = false;
        [Header("AI Navigation")]
        [SerializeField]
        private bool _useAiNavigation = true;
        [SerializeField]
        private float _aiMinSpeed = 5f;
        [SerializeField]
        private NavMeshAgent _playerNavAgent;
        [SerializeField]
        private Transform _aiDestination;

        private bool _isForcedMoveActive = false;
        private bool _IsForcedMoveActive
        {
            get => _isForcedMoveActive;
            set
            {
                if (value)
                {
                    _playerNavAgent.enabled = false;
                }
                else
                {
                }
                _isForcedMoveActive = value;
            }
        }
        private bool _IsRealyGrounded => (_characterController.isGrounded && _characterController.transform.position.y <= 2);
        private bool _IsAiMovementAvailable => _IsRealyGrounded; // NavMeshAgent can move only if agents placed on navmesh (i.e. is grounded)
        private bool _IsAiMovementActive => _useAiNavigation && _IsAiMovementAvailable;


        private ILogger _logger;
        private IInputActionService _inputActionService;
        private bool _xAxisLocked = false, _yAxisLocked = false, _zAxisLocked = false;
        private bool _isJumping = false, _isSliding = false, _isGrounded = false;
        private bool _isRunModeEnabled = false;
        private bool _isJumpPressed = false;
        private float _fallSpeed;
        private Vector3 _slopeSlideVelocity;
        private float _characterControllerStepOffset;
        private float? _lastGroundedTime, _jumpButtonPressedTime;

        private void OnEnable()
        {
            _inputActionService.Run += OnRun;
            _inputActionService.RunCancel += OnRunCancel;
            _inputActionService.Jump += OnJump;
            _inputActionService.JumpCancel += OnJumpCancel;
        }
        private void OnDisable()
        {
            _inputActionService.Run -= OnRun;
            _inputActionService.RunCancel -= OnRunCancel;
            _inputActionService.Jump -= OnJump;
            _inputActionService.JumpCancel -= OnJumpCancel;
        }
        private void Awake()
        {
            _characterControllerStepOffset = _characterController.stepOffset;
        }
        private void Update()
        {
            if (!_IsForcedMoveActive)
            {
                if (_IsAiMovementActive)
                {
                    UpdateAiMovement();
                }
                else
                {
                    UpdateMovement();
                }
            }
        }
        private void OnAnimatorMove()
        {
            if (_useRootMotion && _animator != null && _isGrounded && !_isSliding)
            {
                Vector3 velocity = _animator.deltaPosition;
                velocity.y = _fallSpeed * Time.deltaTime;
                _characterController.Move(velocity);
            }
        }

        private void UpdateSlopeSlideVelocity()
        {
            if (Physics.Raycast(_characterController.transform.position, Vector3.down, out var hitInfo))
            {
                float slopeAngle = Vector3.Angle(hitInfo.normal, Vector3.up);
                if (slopeAngle >= _characterController.slopeLimit)
                {
                    _slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, _fallSpeed, 0), hitInfo.normal);
                    _isSliding = true;
                    return;
                }
            }
            if (_isSliding)
            {
                _slopeSlideVelocity -= _slidingDecelerationFactor * Time.deltaTime * _slopeSlideVelocity;
                if (_slopeSlideVelocity.magnitude > 1)
                {
                    return;
                }
            }
            _slopeSlideVelocity = Vector3.zero;
            _isSliding = false;
        }
        private void UpdateMovement()
        {
            _playerNavAgent.enabled = false;

            float gravity = Physics.gravity.y * _gravityMultiplier;
            if (_isJumping && !_isJumpPressed && _fallSpeed > 0) // если короткое нажатие
            {
                gravity *= 2; // ускоряем прыжок
            }
            _fallSpeed += gravity * Time.deltaTime;

            UpdateSlopeSlideVelocity();

            _isGrounded = _characterController.isGrounded;
            if (_isGrounded)
            {
                _lastGroundedTime = Time.time;
            }

            // Check on falling
            bool isFalling = !_isGrounded;
            if (Time.time - _lastGroundedTime <= _jumpButtonGracePeriod) // grounded recently
            {
                _characterController.stepOffset = _characterControllerStepOffset;
                _isGrounded = true;
                isFalling = false;
                _isJumping = false;
                if (!_isSliding)
                {
                    if (Time.time - _jumpButtonPressedTime <= _jumpButtonGracePeriod) // jump pressed recently
                    {
                        _isJumping = true;
                        _jumpButtonPressedTime = null;
                        _lastGroundedTime = null;
                        _fallSpeed = Mathf.Sqrt(_jumpHeight * -3 * gravity);
                    }
                    else
                    {
                        _fallSpeed = -0.5f; // to prevent isGrounded false positives
                    }
                }
            }
            else
            {
                _characterController.stepOffset = 0; // fix stuck in the wall while jumping
                if ((_isJumping && _fallSpeed < 0) || _fallSpeed < -2)
                {
                    isFalling = true;
                }
            }

            // Direction & DirectionInputMagnitude usings
            var cameraRelativeQuaternion = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up);
            var moveDelta = _inputActionService.LastMoveDelta;
            var direction = cameraRelativeQuaternion * new Vector3(_xAxisLocked ? 0 : moveDelta.x, 0, _zAxisLocked ? 0 : moveDelta.y);
            var directionMagnitudeMax = _isRunModeEnabled ? 1 : 0.5f;
            if (direction.magnitude > 1)
            {
                direction.Normalize();
            }
            var directionMagnitude = Mathf.Clamp(direction.magnitude, 0, directionMagnitudeMax);
            if (!_useRootMotion)
            {
                float speed = directionMagnitude * _maximumSpeed;
                Vector3 velocity = speed * direction;
                velocity.y = _fallSpeed;
                _characterController.Move(velocity * Time.deltaTime);
            }
            if (_isSliding)
            {
                Vector3 velocity = _slopeSlideVelocity;
                velocity.y = _fallSpeed;
                _characterController.Move(velocity * Time.deltaTime);
            }
            else if (!_isGrounded) // jumping
            {
                float speed = directionMagnitude * _jumpHorizontalSpeed;
                Vector3 velocity = speed * direction;
                velocity.y = _fallSpeed;
                _characterController.Move(velocity * Time.deltaTime);
            }

            // Direction usings
            bool isMoving = direction != Vector3.zero;
            if (isMoving)
            {
                Quaternion rotationQuaternion = Quaternion.LookRotation(direction, Vector3.up);
                _characterController.transform.rotation = Quaternion.RotateTowards(_characterController.transform.rotation, rotationQuaternion, _rotationDegreeSpeed * Time.deltaTime);
            }

            if (_animator != null)
            {
                _animator.SetFloat("Input Magnitude", directionMagnitude, 0.5f, Time.deltaTime);
                _animator.SetBool("IsGrounded", _isGrounded);
                _animator.SetBool("IsJumping", _isJumping);
                _animator.SetBool("IsFalling", isFalling);
                _animator.SetBool("IsMoving", isMoving);
            }
        }
        private void UpdateAiMovement()
        {
            if (_playerNavAgent.destination != _aiDestination.position && !(_xAxisLocked && _yAxisLocked && _zAxisLocked))
            {
                var dest = _aiDestination.position;
                var prevDest = _playerNavAgent.destination;
                var newDest = new Vector3(_xAxisLocked ? prevDest.x : dest.x, _yAxisLocked ? prevDest.y : dest.y, _zAxisLocked ? prevDest.z : dest.z);
                _playerNavAgent.enabled = true;
                _playerNavAgent.destination = newDest;
                _playerNavAgent.acceleration = Random.Range(_aiMinSpeed, _maximumSpeed);
                _playerNavAgent.speed = Random.Range(_aiMinSpeed, _maximumSpeed);
            }
        }
        private void OnRun()
        {
            _isRunModeEnabled = true;
        }
        private void OnRunCancel()
        {
            _isRunModeEnabled = false;
        }
        private void OnJump()
        {
            _jumpButtonPressedTime = Time.time;
            _isJumpPressed = true;
        }
        private void OnJumpCancel()
        {
            _isJumpPressed = false;
        }
    }
}