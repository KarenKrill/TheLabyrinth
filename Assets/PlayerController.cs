using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private InputController _inputController;
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private float _speed = 5f;
    private void Awake()
    {
        _inputController.Moved += OnMoved;
    }
    private void OnMoved(Vector2 moveDelta)
    {
        Debug.Log($"Player starts moving with delta {moveDelta}");
    }
    private void Update()
    {
        var positionDelta = new Vector3(_inputController.MoveDelta.x, 0, _inputController.MoveDelta.y);
        positionDelta *= Time.deltaTime * _speed;
        _playerTransform.position += positionDelta;
    }
}
