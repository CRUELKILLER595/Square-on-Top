using UnityEngine;

public class WallScaler : MonoBehaviour
{
    void Start()
    {
        Camera cam = Camera.main;

        float camHeight = cam.orthographicSize * 2;
        float camWidth = camHeight * Screen.width / Screen.height;

        // Get sprite size
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float spriteHeight = sr.bounds.size.y;

        // Scale to match camera height
        float scaleY = camHeight / spriteHeight;

        transform.localScale = new Vector3(
            transform.localScale.x,
            scaleY,
            transform.localScale.z
        );
    }
}