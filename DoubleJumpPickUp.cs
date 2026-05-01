using UnityEngine;

public class DoubleJumpPowerup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playermovement1 player = other.GetComponent<playermovement1>();

        if (player != null)
        {
            player.ActivateDoubleJump();
        }

        Destroy(gameObject);
    }
}