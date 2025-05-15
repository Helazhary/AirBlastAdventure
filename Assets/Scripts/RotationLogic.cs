using UnityEngine;

public class RotationLogic : MonoBehaviour
{

    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private PlayerInput input;

    private void Update()
    {
        HandleRotation();
    }

    private void HandleRotation()
    {
        float rotationInput = -input.MoveInput.x;
        transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);
    }





}