using System;
using UnityEngine;
using TMPro;

namespace KarenKrill
{
    public class RigidBodyMovement : MonoBehaviour
    {
        [SerializeField]
        private InputController _inputController;
        [SerializeField]
        private Rigidbody _rigidbody;
        [SerializeField]
        private float _speed = 5f;
        [SerializeField]
        private float _jumpSpeed = 5f;
        private bool _isGrounded = false;
        [SerializeField]
        private TextMeshProUGUI _debugText;
        private void Awake()
        {
            _inputController.Moved += OnMoved;
            _inputController.Jumped += OnJumped;
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
            Debug.Log($"Player starts moving with delta {moveDelta}");
        }
        private void FixedUpdate()
        {
            var velocity = new Vector3(_inputController.MoveDelta.x, 0, _inputController.MoveDelta.y);
            velocity *= _speed;
            velocity.y = _rigidbody.velocity.y;
            _isGrounded = Mathf.Abs(velocity.y) <= Mathf.Epsilon;
            var oldVelocity = _rigidbody.velocity;
            _rigidbody.velocity = velocity;
            _debugText.text = $"Old Velocity:{oldVelocity}{Environment.NewLine}Velocity:{_rigidbody.velocity}";
        }
    }
}