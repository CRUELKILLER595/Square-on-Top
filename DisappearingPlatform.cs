using UnityEngine;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    public float visibleTime = 2f;
    public float invisibleTime = 2f;

    private SpriteRenderer sr;
    private Collider2D col;

    private Coroutine loopCoroutine;
    private bool isActivated = false; // 👈 stops disappearing

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        loopCoroutine = StartCoroutine(LoopPlatform());
    }

    IEnumerator LoopPlatform()
    {
        while (true)
        {
            // SHOW
            sr.enabled = true;
            col.enabled = true;

            yield return new WaitForSeconds(visibleTime);

            // HIDE
            sr.enabled = false;
            col.enabled = false;

            yield return new WaitForSeconds(invisibleTime);
        }
    }

    // 🎯 Detect player landing
 void OnCollisionEnter2D(Collision2D collision)
{
    if (isActivated) return;

    if (collision.gameObject.CompareTag("Player"))
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null && rb.linearVelocity.y <= 0) // falling
        {
            if (collision.transform.position.y > transform.position.y)
            {
                ActivatePlatform();
            }
        }
    }
}
void ActivatePlatform()
{
    isActivated = true;

    if (loopCoroutine != null)
        StopCoroutine(loopCoroutine);

    sr.enabled = true;
    col.enabled = true;
}
}