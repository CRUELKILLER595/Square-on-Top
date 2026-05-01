using UnityEngine;

public class Powerup : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playermovement1 player = other.GetComponent<playermovement1>();

            if (player != null)
            {
                player.ActivatePowerup(); // � activate ability
            }

            Destroy(gameObject); // remove power-up
        }
    }
}