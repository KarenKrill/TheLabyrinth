using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public delegate void MovementDelegate(Vector2 moveDelta);
public delegate void ButtonClickDelegate(bool isButtonClicked);
[RequireComponent(typeof(PlayerInput))]
public class InputController : MonoBehaviour
{
    public event MovementDelegate Moved;
    public event ButtonClickDelegate Jumped;
    public Vector2 MoveDelta { get; private set; }
    public bool IsJumpPressed { get; private set; }
    private void OnMovement(InputValue inputValue)
    {
        MoveDelta = inputValue.Get<Vector2>();
        Moved?.Invoke(MoveDelta);
    }
    private void OnJump(InputValue inputValue)
    {
        IsJumpPressed = inputValue.isPressed;
        Jumped?.Invoke(inputValue.isPressed);
    }
}