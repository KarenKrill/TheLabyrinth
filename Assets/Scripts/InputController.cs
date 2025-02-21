using UnityEngine;
using UnityEngine.InputSystem;

public delegate void MovementDelegate(Vector2 moveDelta);
[RequireComponent(typeof(PlayerInput))]
public class InputController : MonoBehaviour
{
    public event MovementDelegate Moved;
    public Vector2 MoveDelta { get; private set; }
    private void OnMovement(InputValue inputValue)
    {
        Debug.Log($"Moved on {inputValue.Get<Vector2>()}");
        MoveDelta = inputValue.Get<Vector2>();
        Moved?.Invoke(MoveDelta);
    }
}
