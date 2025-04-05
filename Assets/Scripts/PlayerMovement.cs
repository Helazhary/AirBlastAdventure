using UnityEngine;
using TMPro;
using System.Collections;
using Unity.Cinemachine;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jumpForce = 15f;
    public float rotationSpeed = 4f;
    public float jumpCooldown = 0.35f;
    private float lastJumpTime = -999f;

    //---Resetting position and level teleportation----
    private Vector2 startPosition;
    public Transform levelMenuSpawnPoint;
    public Transform level0SpawnPoint;
    public Transform level1SpawnPoint;
    public Transform level2SpawnPoint;
    public Transform level3SpawnPoint;

    //Cinemachine Rotatoin Toggler
    public CinemachineCamera cam;
    



    //--UI Level Text----
    public TextMeshProUGUI levelText;
    public float levelTextDisplayTime = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        startPosition = levelMenuSpawnPoint.position;
        

        StartCoroutine(DisplayLevelText("Welcome to AirBlastAdventure!"));

    }

    // Update is called once per frame
    void Update()
    {
        
        // //Impulse Air Blast Mechanic
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     Vector2 direction = transform.up;
        //     rb.AddForce(direction * jumpForce,ForceMode2D.Impulse);
        // }

       // Rotation
    float rotationInput = 0f;
    if (Input.GetKey(KeyCode.LeftArrow)) rotationInput = 1f;
    if (Input.GetKey(KeyCode.RightArrow)) rotationInput = -1f;
    transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);

    // Jump with cooldown
    if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastJumpTime + jumpCooldown)
    {
        Vector2 direction = transform.up;
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);
        lastJumpTime = Time.time;
    }

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Hazard"))
        {
            StartCoroutine(ResetAfterDelay(1f)); // 1s delay respawn effect
        }

        if(collision.gameObject.CompareTag("ToLevel0"))
        {
            transform.position = level0SpawnPoint.position;
            startPosition = level0SpawnPoint.position; //set new spawnPoint
            StartCoroutine(DisplayLevelText("Level 0"));
        }

        if(collision.gameObject.CompareTag("ToLevel1"))
        {
            transform.position = level1SpawnPoint.position;
            startPosition = level1SpawnPoint.position; 
            StartCoroutine(DisplayLevelText("Level 1"));
        }
        
        if(collision.gameObject.CompareTag("ToLevel2"))
        {
            transform.position = level2SpawnPoint.position;
            startPosition = level2SpawnPoint.position; 
            StartCoroutine(DisplayLevelText("Level 2"));
        }

        if(collision.gameObject.CompareTag("ToLevel3"))
        {
            transform.position = level3SpawnPoint.position;
            startPosition = level3SpawnPoint.position; 
            StartCoroutine(DisplayLevelText("Level 3"));
        }

         if(collision.gameObject.CompareTag("Restart"))
        {
            transform.position = levelMenuSpawnPoint.position;
            startPosition = levelMenuSpawnPoint.position; 
            StartCoroutine(DisplayLevelText("Thanks for playing MVP DEMO!"));
        }
        
        
        
    }

    IEnumerator DisplayLevelText(string levelName)
    {
        levelText.text = levelName;
        levelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(levelTextDisplayTime);
        levelText.gameObject.SetActive(false);
    }


IEnumerator ResetAfterDelay(float delay)
{
    rb.linearVelocity = Vector2.zero; // Stop movement
    rb.isKinematic = true;      // Freeze physics

    yield return new WaitForSeconds(delay);

    transform.position = startPosition;


    rb.isKinematic = false;     // Resume physics
    StartCoroutine(DisplayLevelText("Try AGAIN!"));
}


}
