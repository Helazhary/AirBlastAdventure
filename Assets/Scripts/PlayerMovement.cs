
using UnityEngine;
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float rotationSpeed = 4f;
    [SerializeField] private float jumpCooldown = 0.35f;

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

    private void HandleJump()
    {
        if (input.MoveInput.y > 0.1f && Time.time >= lastJumpTime + jumpCooldown)
        {
            Vector2 direction = transform.up;
            rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
            lastJumpTime = Time.time;
        }
    }
}




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