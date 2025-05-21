using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Enemies : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float collisionCooldown = 0.2f;
    public float bounceAngleVariance = 45f; // how much randomness in bounce angle

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private float lastCollisionTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        PickRandomDirection();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveDirection.normalized * moveSpeed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time - lastCollisionTime >= collisionCooldown)
        {
            ContactPoint2D contact = collision.contacts[0];

            // Get normal from collision and bounce off
            Vector2 normal = contact.normal;
            Vector2 bounceDir = Vector2.Reflect(moveDirection, normal);

            // Add randomness to bounce
            float angle = Random.Range(-bounceAngleVariance, bounceAngleVariance);
            bounceDir = Quaternion.Euler(0, 0, angle) * bounceDir;

            moveDirection = bounceDir.normalized;
            lastCollisionTime = Time.time;
        }
    }

    void PickRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        moveDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;
    }
}
