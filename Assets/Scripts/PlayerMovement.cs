// using System.Diagnostics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float rotationSpeed = 4f;

    [Header("Air Pump Settings")]
    [SerializeField] private float pumpDuration = 0.25f;   // Max thrust duration
    [SerializeField] private float pumpCooldown = 0.5f;    // Cooldown before next pump

    // Code review : encapulsate visuals in one or more classes.
    // For example, you could have a SquashAndStretch component,
    // a Shine component, etc etc...
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

    private AudioSource deflate_audio;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        deflate_audio = GetComponent<AudioSource>();

        // Fallback assignment
        if (visualTransform == null && transform.childCount > 0)
        {
            visualTransform = transform.GetChild(0);
            // Debug.LogWarning("VisualTransform was auto-assigned: " + visualTransform.name);
        }

        defaultScale = visualTransform != null ? visualTransform.localScale : Vector3.one;
    }

    private void Update()
    {
        HandleRotation();
        HandlePumpLogic();
        HandleVisualPumpFeedback();
    }

    // Code review : this is probably the only logic that should be in a player movement script
    private void FixedUpdate()
    {
        if (isPumping)
        {
            float elapsed = Time.time - pumpStartTime;
            float taperFactor = 1f - Mathf.Clamp01(elapsed / pumpDuration); // 1 to 0 over duration

            Vector2 direction = transform.up;
            rb.AddForce(direction * jumpForce * Time.fixedDeltaTime, ForceMode2D.Force);
        }
    }

   private void HandlePumpLogic()
{
    float timeNow = Time.time;
    
    // Start pump if space is pressed down (happens only on the frame when pressed)
    if (Input.GetKeyDown(KeyCode.Space) && !isPumping && timeNow >= lastPumpEndTime + pumpCooldown)
    {
        isPumping = true;
        pumpStartTime = timeNow;
        deflate_audio.Play();
    }

    // End pump if space is released
    if (isPumping && Input.GetKeyUp(KeyCode.Space))
    {
        isPumping = false;
        lastPumpEndTime = timeNow;
        deflate_audio.Stop();
    }

    // End pump if max duration is reached
    if (isPumping && timeNow - pumpStartTime > pumpDuration)
    {
        isPumping = false;
        lastPumpEndTime = timeNow;
        // deflate_audio.Stop();
    }
}
    private void HandleRotation()
    {
        // Code review : rotation could also be in its own script.
        // Encapsulating things further will help you improve
        // elements in your game one by one without risking string dependencies.

        // You could add a bit of damping to your rotation to make it will a bit smoother.
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