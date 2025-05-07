using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerTeleport : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private Transform levelMenuSpawnPoint;
    [SerializeField] private Transform level0SpawnPoint;
    [SerializeField] private Transform level1SpawnPoint;
    [SerializeField] private Transform level2SpawnPoint;
    [SerializeField] private Transform level3SpawnPoint;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private float levelTextDisplayTime = 4f;

    private Vector2 startPosition;
    private Rigidbody2D rb;

    private const string TAG_HAZARD = "Hazard";
    private const string TAG_LEVEL_0 = "ToLevel0";
    private const string TAG_LEVEL_1 = "ToLevel1";
    private const string TAG_LEVEL_2 = "ToLevel2";
    private const string TAG_LEVEL_3 = "ToLevel3";
    private const string TAG_RESTART = "Restart";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = levelMenuSpawnPoint.position;
        StartCoroutine(DisplayLevelText("Welcome to AirBlastAdventure!"));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        switch (tag)
        {
            case TAG_HAZARD:
                StartCoroutine(ResetAfterDelay(1f));
                break;
            case TAG_LEVEL_0:
                Teleport(level0SpawnPoint, "Level 1: Explore the mechanics then go up to enter the next level!");
                break;
            case TAG_LEVEL_1:
                Teleport(level1SpawnPoint, "TEMPORARY");
                break;
            case TAG_LEVEL_2:
                Teleport(level2SpawnPoint, "Level 2: Avoid the enemies and escape!");
                break;
            case TAG_LEVEL_3:
                Teleport(level3SpawnPoint, "Level 4");
                break;
            case TAG_RESTART:
                Teleport(levelMenuSpawnPoint, "Thanks for playing MVP DEMO!");
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
        rb.isKinematic = true;

        yield return new WaitForSeconds(delay);

        transform.position = startPosition;
        rb.isKinematic = false;
        StartCoroutine(DisplayLevelText("Try AGAIN!"));
    }
}
