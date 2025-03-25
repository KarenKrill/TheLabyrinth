using UnityEngine.InputSystem;

namespace KarenKrill.TheLabyrinth.Input
{
    using Abstractions;
    using System;
    using UnityEngine;
    using Zenject;

    public class InputActionService : IInputActionService, PlayerControls.IInGameActions, PlayerControls.IUIActions
    {
        private PlayerControls _playerControls;

        public Vector2 LastMoveDelta { get; private set; }
#nullable enable
        public event MoveDelegate? Move;
        public event Action? Run;
        public event Action? RunCancel;
        public event Action? MoveCancel;
        public event Action? Jump;
        public event Action? JumpCancel;
        public event Action? Pause;
        public event Action? AutoPlayCheat;
        public event Action? Back;
        [Inject]
        ILogger _logger;
        public void SetActionMap(ActionMap actionMap)
        {
            switch (actionMap)
            {
                case ActionMap.InGame:
                    _playerControls.UI.Disable();
                    _playerControls.InGame.Enable();
                    break;
                case ActionMap.UI:
                    _playerControls.InGame.Disable();
                    _playerControls.UI.Enable();
                    break;
                default:
                    throw new NotImplementedException($"\"{actionMap}\" {nameof(ActionMap)} setting isn't implemented");
            }
            _logger.Log($"{actionMap} {nameof(ActionMap)} enabled");
        }
        public void Disable()
        {
            _playerControls.InGame.Disable();
            _playerControls.UI.Disable();
        }

#nullable restore
        public InputActionService()
        {
            if (_playerControls == null)
            {
                _playerControls = new();
                _playerControls.InGame.SetCallbacks(this);
                _playerControls.UI.SetCallbacks(this);

            }
        }

        #region InGame Actions

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                //context.startTime
                Jump?.Invoke(); 
            }
            else if (context.canceled)
            {
                JumpCancel?.Invoke();
            }
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var moveDelta = context.ReadValue<Vector2>();
                LastMoveDelta = moveDelta;
                Move?.Invoke(moveDelta);
            }
            else if (context.canceled)
            {
                LastMoveDelta = Vector2.zero;
                MoveCancel?.Invoke();
            }
        }

        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Run?.Invoke();
            }
            else if (context.canceled)
            {
                RunCancel?.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Pause?.Invoke();
            }
        }

        public void OnAutoPlayCheat(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                AutoPlayCheat?.Invoke();
            }
        }

        #endregion

        #region UI Actions

        public void OnBack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Back?.Invoke();
            }
        }

        #endregion
    }
}
