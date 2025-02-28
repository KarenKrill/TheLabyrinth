using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputController _inputController;
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
    private bool _useRootMotion = false;
    [Header("AI Navigation")]
    [SerializeField]
    private bool _useAiNavigation = true;
    [SerializeField]
    private NavMeshAgent _playerNavAgent;
    [SerializeField]
    private Transform _aiDestination;
    private float _fallSpeed;
    private bool _isJumping = false, _isSliding = false, _isGrounded = false;
    private Vector3 _slopeSlideVelocity;
    private float _characterControllerStepOffset;
    private float? _lastGroundedTime, _jumpButtonPressedTime;

    private void Awake()
    {
        _inputController.Moved += OnMoved;
        _inputController.Jumped += OnJumped;
    }
    private void OnJumped(bool isButtonClicked)
    {
        if (isButtonClicked)
        {
            _jumpButtonPressedTime = Time.time;
        }
    }
    private void OnMoved(Vector2 moveDelta) { }
    private void UpdateSlopeSlideVelocity()
    {
        if (Physics.Raycast(_characterController.transform.position, Vector3.down, out var hitInfo))
        {
            float slopeAngle = Vector3.Angle(hitInfo.normal, Vector3.up);
            if (slopeAngle >= _characterController.slopeLimit)
            {
                _slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, _fallSpeed, 0), hitInfo.normal);
                return;
            }
        }
        if (_isSliding)
        {
            _slopeSlideVelocity -= _slopeSlideVelocity * Time.deltaTime * 3;
            if (_slopeSlideVelocity.magnitude > 1)
            {
                return;
            }
        }
        _slopeSlideVelocity = Vector3.zero;
    }
    private void UpdateMovement()
    {
        _isGrounded = _characterController.isGrounded;
        bool isFalling = !_isGrounded;
        Vector3 direction = new(_inputController.MoveDelta.x, 0, _inputController.MoveDelta.y);
        var quat = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up);
        direction = quat * direction;
        float inputMagnitude = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 1 : 0.5f;
        // Only if InputAction mode is not DigitalNormalized
        //float inputMagnitude = Mathf.Clamp(direction.magnitude, 0, (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? 1 : 0.5f);
        //direction.Normalize();
        float gravity = Physics.gravity.y * _gravityMultiplier;
        if (_isJumping && _fallSpeed > 0 && !_inputController.IsJumpPressed) // если короткое нажатие
        {
            gravity *= 2; // ускоряем прыжок
        }
        _fallSpeed += gravity * Time.deltaTime;
        UpdateSlopeSlideVelocity();
        if (_slopeSlideVelocity == Vector3.zero)
        {
            _isSliding = false;
        }

        if (_isGrounded)
        {
            _lastGroundedTime = Time.time;
        }
        else
        {
            _characterControllerStepOffset = _characterController.stepOffset;
            _characterController.stepOffset = 0;
        }
        if (Time.time - _lastGroundedTime <= _jumpButtonGracePeriod)
        {
            if (_slopeSlideVelocity != Vector3.zero)
            {
                _isSliding = true;
            }
            _characterController.stepOffset = _characterControllerStepOffset;
            if (!_isSliding)
            {
                _fallSpeed = -0.5f;
            }
            _isGrounded = true;
            isFalling = false;
            _isJumping = false;
            if (Time.time - _jumpButtonPressedTime <= _jumpButtonGracePeriod && !_isSliding)
            {
                _isJumping = true;
                _fallSpeed = Mathf.Sqrt(_jumpHeight * -3 * gravity);
                _jumpButtonPressedTime = null;
                _lastGroundedTime = null;
            }
        }
        else
        {
            _isGrounded = false;
            if ((_isJumping && _fallSpeed < 0) || _fallSpeed < -2)
            {
                isFalling = true;
            }
        }
        if (!_useRootMotion)
        {
            float speed = inputMagnitude * _maximumSpeed;
            Vector3 velocity = speed * direction;
            velocity.y = _fallSpeed;
            _characterController.Move(velocity * Time.deltaTime);
        }
        if (!_isGrounded && !_isSliding)
        {
            float speed = inputMagnitude * _jumpHorizontalSpeed;
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
        bool isMoving = direction != Vector3.zero;
        if (isMoving)
        {
            Quaternion rotationQuaternion = Quaternion.LookRotation(direction, Vector3.up);
            _characterController.transform.rotation = Quaternion.RotateTowards(_characterController.transform.rotation, rotationQuaternion, _rotationDegreeSpeed * Time.deltaTime);
        }

        if (_animator != null)
        {
            _animator.SetFloat("Input Magnitude", inputMagnitude, 0.5f, Time.deltaTime);
            _animator.SetBool("IsGrounded", _isGrounded);
            _animator.SetBool("IsJumping", _isJumping);
            _animator.SetBool("IsFalling", isFalling);
            _animator.SetBool("IsMoving", isMoving);
        }
    }
    private void OnAnimatorMove()
    {
        if (_useRootMotion && _isGrounded && !_isSliding && _animator != null)
        {
            Vector3 velocity = _animator.deltaPosition;
            velocity.y = _fallSpeed * Time.deltaTime;
            _characterController.Move(velocity);
        }
    }
    private bool _isMoveLocked = false;
    private bool _IsMoveLocked
    {
        get => _isMoveLocked;
        set
        {
            if (value)
            {
                _playerNavAgent.enabled = false;
            }
            else
            {
            }
            _isMoveLocked = value;
        }
    }
    private void Update()
    {
        if (!_IsMoveLocked)
        {
            if (_useAiNavigation)
            {
                if (_playerNavAgent.destination != _aiDestination.position)
                {
                    _playerNavAgent.enabled = true;
                    _playerNavAgent.destination = _aiDestination.position;
                    _playerNavAgent.acceleration = _maximumSpeed;
                    _playerNavAgent.speed = _maximumSpeed;
                }
            }
            else
            {
                _playerNavAgent.enabled = false;
                UpdateMovement();
            }
        }
    }
    public IEnumerator Move(Vector3 dest)
    {
        _IsMoveLocked = true;
        yield return new WaitForSeconds(0.01f);
        _characterController.transform.position = dest;
        yield return new WaitForSeconds(0.01f);
        _IsMoveLocked = false;
    }
}