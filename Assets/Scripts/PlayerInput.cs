using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputActionReference move;

    public Vector2 MoveInput { get; private set; }

    void OnEnable()
    {
        if (move != null) move.action.Enable();
    }

    void OnDisable()
    {
        if (move != null) move.action.Disable();
    }

    void Update()
    {
        if (move != null) MoveInput = move.action.ReadValue<Vector2>();
    }
}
