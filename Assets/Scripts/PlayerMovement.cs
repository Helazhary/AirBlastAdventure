using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float rotationSpeed = 4f;

    [Header("Air Pump Settings")]
    [SerializeField] private float pumpDuration = 0.25f;   // Max thrust duration
    [SerializeField] private float pumpCooldown = 0.5f;    // Cooldown before next pump

    [Header("Visual Pump Feedback")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Vector3 pumpScale = new Vector3(1.2f, 0.8f, 1f);
    [SerializeField] private float scaleLerpSpeed = 5f;

    private bool isPumping = false;
    private float pumpStartTime;
    private float lastPumpEndTime = -999f;
    private Vector3 defaultScale;

    private Rigidbody2D rb;
    private PlayerInput input;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();

        // Fallback assignment
        if (visualTransform == null && transform.childCount > 0)
        {
            visualTransform = transform.GetChild(0);
            Debug.LogWarning("VisualTransform was auto-assigned: " + visualTransform.name);
        }

        defaultScale = visualTransform != null ? visualTransform.localScale : Vector3.one;
    }

    private void Update()
    {
        HandleRotation();
        HandlePumpLogic();
        HandleVisualPumpFeedback();
    }

    private void FixedUpdate()
    {
        if (isPumping)
        {
            // float elapsed = Time.time - pumpStartTime;
            // float taperFactor = 1f - Mathf.Clamp01(elapsed / pumpDuration); // 1 to 0 over duration

            Vector2 direction = transform.up;
            rb.AddForce(direction * jumpForce * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }

    private void HandlePumpLogic()
    {
        float timeNow = Time.time;

        // Start pump if input pressed and cooldown done
        if (input.MoveInput.y > 0.1f && !isPumping && timeNow >= lastPumpEndTime + pumpCooldown)
        {
            isPumping = true;
            pumpStartTime = timeNow;
        }

        // End pump early if released
        if (isPumping && input.MoveInput.y <= 0.1f)
        {
            isPumping = false;
            lastPumpEndTime = timeNow;
        }

        // End pump if time runs out
        if (isPumping && timeNow - pumpStartTime > pumpDuration)
        {
            isPumping = false;
            lastPumpEndTime = timeNow;
        }
    }

    private void HandleRotation()
    {
        float rotationInput = -input.MoveInput.x;
        transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);
    }

    private void HandleVisualPumpFeedback()
    {
        if (visualTransform == null) return;

        Vector3 targetScale = isPumping ? pumpScale : defaultScale;
        visualTransform.localScale = Vector3.Lerp(visualTransform.localScale, targetScale, scaleLerpSpeed * Time.deltaTime);
    }
}