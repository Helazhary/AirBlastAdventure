using UnityEngine;
using TMPro;
using System.Collections;

// Code review : 
// You could go a bit further and create a Level script, which manages inernal level flow (start, stop, etc...)
// Each level would have a spawn point assigned to it and would encapulsate the teleport logic on start level.
// Then, you would have a GameFlowManager that contains references to said levels.
// This pattern will give you more flexibility, especially if you want to polish a bit and create more complex flows
// when you start and end a level (small cutscenes, camera movements, VFX, etc...)
public class PlayerTeleport : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform levelMenuSpawnPoint;
    [SerializeField] private Transform level0SpawnPoint;
    [SerializeField] private Transform level1SpawnPoint;
    [SerializeField] private Transform level2SpawnPoint;
    [SerializeField] private Transform level3SpawnPoint;
     [SerializeField] private Transform level4SpawnPoint;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private float levelTextDisplayTime = 4f;

    private Vector2 startPosition;
    private Rigidbody2D rb;

    [SerializeField] private PumpLogic pumpLogic; 

    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    private const string TAG_HAZARD = "Hazard";
    private const string TAG_LEVEL_0 = "ToLevel0";
    private const string TAG_LEVEL_1 = "ToLevel1";
    private const string TAG_LEVEL_2 = "ToLevel2";
    private const string TAG_LEVEL_3 = "ToLevel3";
    private const string TAG_LEVEL_4 = "ToLevel4";
    private const string TAG_RESTART = "Restart";
    public bool isDead = false; // Exposed to other scripts

    [SerializeField] private AudioSource pop_audio;
    [SerializeField] private Transform visualTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = levelMenuSpawnPoint.position;
        pumpLogic = GetComponent<PumpLogic>();
        StartCoroutine(DisplayLevelText("Welcome to the Circus!"));
    }

    // Code review : isolate in a script (EndLevelTrigger for instance) that 
    // contains a reference to the next Level
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        switch (tag)
        {
            case TAG_HAZARD:
                pop_audio.Play();
                visualTransform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                
                if (pumpLogic != null) pumpLogic.isDead = true;

                StartCoroutine(ResetAfterDelay(1f));
                break;

            case TAG_LEVEL_0:
                Teleport(level0SpawnPoint, "Level 1: Don't touch the platforms");
                break;
            case TAG_LEVEL_1:
                Teleport(level1SpawnPoint, "Level 2: Avoid the enemies");
                break;
            case TAG_LEVEL_2:
                Teleport(level2SpawnPoint, "Level 3");
                break;
            case TAG_LEVEL_3:
                Teleport(level3SpawnPoint, "Level 4");
                break;
            case TAG_LEVEL_4:
                Teleport(level4SpawnPoint, "Level 5");
                break;
            case TAG_RESTART:
                Teleport(levelMenuSpawnPoint, "Thanks for playing!");
                break;
        }
    }

    private void Teleport(Transform target, string label)
    {
        transform.position = target.position;
        startPosition = target.position;
        StartCoroutine(DisplayLevelText(label));
    }

    private IEnumerator DisplayLevelText(string text)
    {
        levelText.text = text;
        levelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(levelTextDisplayTime);
        levelText.gameObject.SetActive(false);
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; // Code review : deprecated. Set body type instead.
        // Also, if you are trying to disable physics during that delay, I'd suggest just disabling the rigidbody
        // instead of changing it to kinematic. 
    
        yield return new WaitForSeconds(delay);

        transform.position = startPosition;
        visualTransform.localScale = Vector3.one;


        rb.isKinematic = false;

        if (pumpLogic != null) pumpLogic.isDead = false;
        StartCoroutine(DisplayLevelText("Try AGAIN!"));
    }
}
