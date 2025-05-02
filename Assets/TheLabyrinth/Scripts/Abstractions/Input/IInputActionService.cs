#nullable enable

using System;
using UnityEngine;

namespace TheLabyrinth.Input.Abstractions
{
    public enum ActionMap
    {
        InGame,
        UI
    }

    public delegate void MoveDelegate(Vector2 moveDelta);

    public interface IInputActionService
    {
        public Vector2 LastMoveDelta { get; }

        public event MoveDelegate? Move;
        public event Action? Run;
        public event Action? RunCancel;
        public event Action? MoveCancel;
        public event Action? Jump;
        public event Action? JumpCancel;
        public event Action? Pause;
        public event Action? AutoPlayCheat;

        public event Action? Back;

        public void SetActionMap(ActionMap actionMap);
        public void Disable();
    }
}
