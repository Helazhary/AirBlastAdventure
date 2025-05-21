using UnityEngine;

public class PumpLogic : MonoBehaviour
{
    [Header("Air Pump Settings")]
    [SerializeField] private float maxStretch = 10f;  // Maximum stretch amount
    [SerializeField] private float stretchSpeed = 5f;  // Speed at which the stretch occurs
    [SerializeField] private float releaseForce = 50f;  // Force applied when released

    [SerializeField] private Transform visualTransform;  // Reference to the visual transform of the balloon
    private float stretchAmount = 0f;  // Current stretch amount (0 to maxStretch)
    private bool isPumping = false;

    private Rigidbody2D rb;
    private AudioSource deflate_audio;

    private Vector3 defaultScale;  // Default scale (normal size)
    private bool isReleasing = false; // Flag to handle smooth scale back
    public bool isDead = false;
    private void Start()
    {
        deflate_audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
        defaultScale = visualTransform.localScale; // Store the default scale to return to it later
    }

    private void Update()
    {
        HandlePumpLogic();
        HandleVisualStretch();
    }

    private void FixedUpdate()
    {
        if (isPumping)
        {
            // Increase the stretch amount while holding down the spacebar
            stretchAmount += stretchSpeed * Time.fixedDeltaTime;
            stretchAmount = Mathf.Clamp(stretchAmount, 0f, maxStretch); // Make sure stretch doesn't go over max
        }
    }

    private void HandlePumpLogic()
    {

        if (isDead) return;
        
        float timeNow = Time.time;

        // Start pumping if space is pressed down
        if (Input.GetKeyDown(KeyCode.Space) && !isPumping)
        {
            isPumping = true;
        }

        // Stop pumping if space is released
        if (isPumping && Input.GetKeyUp(KeyCode.Space))
        {
            isPumping = false;
            ApplyForce(); // Apply the force based on the stretch amount
            deflate_audio.Stop(); // Stop pumping sound if released prematurely
            isReleasing = true; // Start releasing and scale back
            deflate_audio.Play(); // Start releasing sound during air release
        }
    }

    private void ApplyForce()
    {
        // Start velocity at 0
        rb.linearVelocity = Vector2.zero;

        // Apply force when released
        Vector2 direction = transform.up;  // Assuming up is the direction the balloon will shoot
        float forceToApply = stretchAmount * releaseForce; // Multiply by stretch amount to get a varying force

        // Apply the calculated force to the Rigidbody2D
        rb.AddForce(direction * forceToApply, ForceMode2D.Impulse);

        // Reset the stretch amount after release
        stretchAmount = 0f;
    }

    private void HandleVisualStretch()
    {
        if (isPumping && !isDead)
        {
            // Increase the visual stretch of the balloon
            visualTransform.localScale = Vector3.Lerp(visualTransform.localScale, 
                defaultScale * (1 + stretchAmount / maxStretch), Time.deltaTime * stretchSpeed);
        }
        else if (isReleasing && !isDead)
        {
            // Smoothly return to the normal size after release
            visualTransform.localScale = Vector3.Lerp(visualTransform.localScale, defaultScale, Time.deltaTime * stretchSpeed);

            // If the balloon is very close to its normal size, stop the releasing process
            if (Vector3.Distance(visualTransform.localScale, defaultScale) < 0.01f)
            {
                deflate_audio.Stop();
                visualTransform.localScale = defaultScale; // Ensure it's exactly the default size
                isReleasing = false; // Stop the release process
                 // Stop the releasing sound once shrinking is complete
            }
        }
    }
}
