using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    private Rigidbody2D rb;
    private playermovement1 player;

    private int playerLayer;
    private int platformLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<playermovement1>();

        playerLayer = gameObject.layer;
        platformLayer = LayerMask.NameToLayer("Ground");
    }

    void Update()
    {
        if (player.canPassThroughPlatforms)
        {
            // ⬆️ Going UP → ignore collision
            if (rb.linearVelocity.y > 0)
            {
                Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
            }
            else
            {
                Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
            }
        }
        else
        {
            Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
        }
    }
}