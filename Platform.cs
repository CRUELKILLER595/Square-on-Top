using UnityEngine;

public class Platform : MonoBehaviour
{
    private ScoreManager scoreManager;
    public SFXManager SFXManager;
    public AudioClip breakSound;
    private bool hasScored = false;

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        playermovement1 player = collision.gameObject.GetComponent<playermovement1>();
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (player == null || rb == null)
            return;

        // 💥 BREAKING MODE → break + score
        if (player.isBreakingMode)
        {
            scoreManager.AddBreakBonus();
            Destroy(gameObject);
            SFXManager.PlaySound(breakSound);
            return;
        }

        // 👻 Ghost mode → ignore upward collision
        if (player.canPassThroughPlatforms && rb.linearVelocity.y > 0)
            return;

        // 🔁 prevent duplicate score
        if (hasScored)
            return;

        // 🔽 only when falling
        if (rb.linearVelocity.y <= 0&&player.transform.position.y > transform.position.y+0.7f)
        {
         
        }
    }
}