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


// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     [Header("Movement Settings")]
//     [SerializeField] private float jumpForce = 15f;
//     [SerializeField] private float rotationSpeed = 4f;




//     [Header("Pump Settings")]
//     [SerializeField] private float pumpDuration = 0.25f; // How long player can thrust
//     [SerializeField] private float pumpCooldown = 0.5f;  // Cooldown after pump

//     private bool isPumping = false;
//     private float pumpStartTime;
//     private float lastPumpEndTime = -999f;




//     [Header("Visual Pump Feedback")]
//     [SerializeField] private Transform visualTransform; // Assign your visible sprite object here
//     [SerializeField] private Vector3 pumpScale = new Vector3(1.2f, 0.8f, 1f); // Slight stretch
//     [SerializeField] private float scaleLerpSpeed = 5f; // Speed of visual smoothing

//     private Vector3 defaultScale;





//     private Rigidbody2D rb;
//     private PlayerInput input;

//     private void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         input = GetComponent<PlayerInput>();
//         defaultScale = visualTransform.localScale;

//     }

//     private void Update()
//     {
//         HandleRotation();

//         float timeNow = Time.time;

//         // Start pumping if not already and cooldown passed
//         if (input.MoveInput.y > 0.1f && !isPumping && timeNow >= lastPumpEndTime + pumpCooldown)
//         {
//             isPumping = true;
//             pumpStartTime = timeNow;
//         }

//         // End pumping if time is up
//         if (isPumping && timeNow - pumpStartTime > pumpDuration)
//         {
//             isPumping = false;
//             lastPumpEndTime = timeNow;
//         }

//         // Smooth scale transition for pumping visual
//         Vector3 targetScale = isPumping ? pumpScale : defaultScale;
//         visualTransform.localScale = Vector3.Lerp(visualTransform.localScale, targetScale, scaleLerpSpeed * Time.deltaTime);

//     }

//     private void FixedUpdate()
//     {
//         // Only apply force in FixedUpdate, while pump is active
//         if (isPumping)
//         {
//             Vector2 direction = transform.up;
//             rb.AddForce(direction * jumpForce * Time.fixedDeltaTime, ForceMode2D.Force);
//         }
//     }

//     private void HandleRotation()
//     {
//         float rotationInput = -input.MoveInput.x;
//         transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);
//     }
// }



/*
//------------------------------------------------------CODE v2---------------------------------------------------------
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float jumpCooldown = 0.35f;


    //----------------------Pump mechanics--------------------
    [SerializeField] private float pumpDuration = 0.25f; // How long player can thrust
    [SerializeField] private float pumpCooldown = 0.5f;  // regen time before next pump

    private bool isPumping = false;
    private float pumpStartTime;
    private float lastPumpEndTime = -999f;
    //--------------------------------------------------------
    private float lastJumpTime = -999f;
    private Rigidbody2D rb;
    private PlayerInput input;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        HandleRotation();
        HandleJump();
    }

    private void HandleRotation()
    {
        float rotationInput = -input.MoveInput.x;
        transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);
    }

    // private void HandleJump()
    // {
    //     if (input.MoveInput.y > 0.1f && Time.time >= lastJumpTime + jumpCooldown)
    //     {
    //         Vector2 direction = transform.up;
    //         rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
    //         lastJumpTime = Time.time;
    //     }
    // }

    private void HandleJump()
{
    float timeNow = Time.time;

    // Start the pump
    if (input.MoveInput.y > 0.1f && !isPumping && timeNow >= lastPumpEndTime + pumpCooldown)
    {
        isPumping = true;
        pumpStartTime = timeNow;
    }

    // Apply thrust while within pumpDuration
    if (isPumping && timeNow - pumpStartTime <= pumpDuration)
    {
        Vector2 direction = transform.up;
        rb.AddForce(direction * jumpForce * Time.deltaTime, ForceMode2D.Force);
    }

    // End the pump after duration
    if (isPumping && timeNow - pumpStartTime > pumpDuration)
    {
        isPumping = false;
        lastPumpEndTime = timeNow;
    }
}

}
*/

//---------------------------------------------------------------------------------------------------


// using UnityEngine;
// using TMPro;
// using System.Collections;
// using Unity.Cinemachine;
// using UnityEngine.InputSystem;

// public class PlayerMovement : MonoBehaviour
// {
//     // --- Movement ---
//     [SerializeField] private float jumpForce = 15f;
//     [SerializeField] private float rotationSpeed = 4f;
//     [SerializeField] private float jumpCooldown = 0.35f;
//     private float lastJumpTime = -999f;
//     private Rigidbody2D rb;


//     //---- New Movement ----
//     private Vector2 moveInput;
//     [SerializeField] private InputActionReference move;

//     // --- Spawn Points ---
//     [SerializeField] private Transform levelMenuSpawnPoint;
//     [SerializeField] private Transform level0SpawnPoint;
//     [SerializeField] private Transform level1SpawnPoint;
//     [SerializeField] private Transform level2SpawnPoint;
//     [SerializeField] private Transform level3SpawnPoint;
//     private Vector2 startPosition;

//     // --- Camera ---
//     [SerializeField] private CinemachineCamera cam;

//     // --- UI ---
//     [SerializeField] private TextMeshProUGUI levelText;
//     [SerializeField] private float levelTextDisplayTime = 2f;


//     // --- Tags ---
//     private const string TAG_HAZARD = "Hazard";
//     private const string TAG_LEVEL_0 = "ToLevel0";
//     private const string TAG_LEVEL_1 = "ToLevel1";
//     private const string TAG_LEVEL_2 = "ToLevel2";
//     private const string TAG_LEVEL_3 = "ToLevel3";
//     private const string TAG_RESTART = "Restart";

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         startPosition = levelMenuSpawnPoint.position;
//         StartCoroutine(DisplayLevelText("Welcome to AirBlastAdventure!"));
//     }

//     void Update()
//     {
//         moveInput = move.action.ReadValue<Vector2>();
//         HandleRotation();
//         HandleJump();
//     }

//     void OnEnable()
//     {
//         move.action.Enable();
//     }

//     void OnDisable()
//     {
//         move.action.Disable();
//     }

//     private void HandleRotation()
//     {
//         float rotationInput = -moveInput.x; // Invert for intuitive left/right
//         transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);
//     }


//     private void HandleJump()
//     {
//         if (moveInput.y > 0.1f && Time.time >= lastJumpTime + jumpCooldown)
//         {
//             Vector2 direction = transform.up;
//             rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
//             lastJumpTime = Time.time;
//         }
//     }


//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         string tag = collision.gameObject.tag;

//         if (tag == TAG_HAZARD)
//         {
//             StartCoroutine(ResetAfterDelay(1f));
//         }
//         else if (tag == TAG_LEVEL_0)
//         {
//             TeleportToLevel(level0SpawnPoint, "Level 0");
//         }
//         else if (tag == TAG_LEVEL_1)
//         {
//             TeleportToLevel(level1SpawnPoint, "Level 1");
//         }
//         else if (tag == TAG_LEVEL_2)
//         {
//             TeleportToLevel(level2SpawnPoint, "Level 2");
//         }
//         else if (tag == TAG_LEVEL_3)
//         {
//             TeleportToLevel(level3SpawnPoint, "Level 3");
//         }
//         else if (tag == TAG_RESTART)
//         {
//             TeleportToLevel(levelMenuSpawnPoint, "Thanks for playing MVP DEMO!");
//         }
//     }

//     private void TeleportToLevel(Transform spawnPoint, string levelName)
//     {
//         transform.position = spawnPoint.position;
//         startPosition = spawnPoint.position;
//         StartCoroutine(DisplayLevelText(levelName));
//     }

//     private IEnumerator DisplayLevelText(string levelName)
//     {
//         levelText.text = levelName;
//         levelText.gameObject.SetActive(true);
//         yield return new WaitForSeconds(levelTextDisplayTime);
//         levelText.gameObject.SetActive(false);
//     }

//     private IEnumerator ResetAfterDelay(float delay)
//     {
//         rb.linearVelocity = Vector2.zero;
//         rb.isKinematic = true;

//         yield return new WaitForSeconds(delay);

//         transform.position = startPosition;
//         rb.isKinematic = false;

//         StartCoroutine(DisplayLevelText("Try AGAIN!"));
//     }
// }