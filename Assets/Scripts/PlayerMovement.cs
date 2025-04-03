using UnityEngine;
using TMPro;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float jumpForce = 15f;
    public float rotationSpeed = 4f;
    public float jumpCooldown = 0.35f;
    private float lastJumpTime = -999f;

    //---Resetting position and level teleportation----
    private Vector2 startPosition;
    private Vector2 level1SpawnPoint;
    public Transform level2SpawnPoint;
    public Transform level3SpawnPoint;

    //--UI Level Text----
    public TextMeshProUGUI levelText;
    public float levelTextDisplayTime = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        startPosition = transform.position;
        level1SpawnPoint = startPosition;

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
            transform.position = startPosition;
        }
        if(collision.gameObject.CompareTag("ToLevel2"))
        {
            transform.position = level2SpawnPoint.position;
            startPosition = level2SpawnPoint.position; //set new spawnPoint
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
            transform.position = level1SpawnPoint;
            startPosition = level1SpawnPoint; 
        }
        
        
    }

    IEnumerator DisplayLevelText(string levelName)
    {
        levelText.text = levelName;
        levelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(levelTextDisplayTime);
        levelText.gameObject.SetActive(false);
    }


}
