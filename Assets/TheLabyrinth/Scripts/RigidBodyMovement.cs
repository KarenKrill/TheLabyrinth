using System;
using UnityEngine;
using Zenject;
using TMPro;

namespace KarenKrill.TheLabyrinth
{
    using Input.Abstractions;

    public class RigidBodyMovement : MonoBehaviour
    {
        [Inject]
        public void Initialize(ILogger logger, IInputActionService inputActionService)
        {
            _logger = logger;
            _inputActionService = inputActionService;
        }

        [SerializeField]
        private TextMeshProUGUI _debugText;
        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private float _speed = 5f;
        [SerializeField]
        private float _jumpSpeed = 5f;

        private ILogger _logger;
        private IInputActionService _inputActionService;
        private bool _isGrounded = false;

        private void Awake()
        {
            _inputActionService.Move += OnMoved;
            _inputActionService.Jump += () => OnJumped(true);
            _inputActionService.JumpCancel += () => OnJumped(false);
        }
        private void FixedUpdate()
        {
            var velocity = new Vector3(_inputActionService.LastMoveDelta.x, 0, _inputActionService.LastMoveDelta.y);
            velocity *= _speed;
            velocity.y = _rigidbody.velocity.y;
            _isGrounded = Mathf.Abs(velocity.y) <= Mathf.Epsilon;
            var oldVelocity = _rigidbody.velocity;
            _rigidbody.velocity = velocity;
            _debugText.text = $"Old Velocity:{oldVelocity}{Environment.NewLine}Velocity:{_rigidbody.velocity}";
        }

        private void OnJumped(bool isButtonClicked)
        {
            if (isButtonClicked && _isGrounded)
            {
                _rigidbody.AddForce(new Vector3(0, _jumpSpeed, 0), ForceMode.Impulse);
            }
        }
        private void OnMoved(Vector2 moveDelta)
        {
            _logger.Log($"Player starts moving with delta {moveDelta}");
        }
    }
}