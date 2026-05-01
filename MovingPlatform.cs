using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float distance = 3f;

    private Vector2 startPos;
    private Rigidbody2D rb;

    private int direction = 1;

    // 👇 Velocity exposed to player
    public float CurrentVelocityX { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // ✅ Ensure correct setup automatically
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        startPos = rb.position;
    }

    void FixedUpdate()
    {
        float targetX = startPos.x + direction * distance;

        float previousX = rb.position.x;

        float newX = Mathf.MoveTowards(
            previousX,
            targetX,
            speed * Time.fixedDeltaTime
        );

        // ✅ Accurate velocity calculation
        CurrentVelocityX = (newX - previousX) / Time.fixedDeltaTime;

        // ✅ Move platform
        rb.MovePosition(new Vector2(newX, rb.position.y));

        // ✅ Reverse direction at ends
        if (Mathf.Abs(newX - targetX) < 0.01f)
        {
            direction *= -1;
        }
    }
}