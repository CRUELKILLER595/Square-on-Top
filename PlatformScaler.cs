using UnityEngine;

[ExecuteAlways]
public class PlatformAutoScaler : MonoBehaviour
{
    [Header("Width Control")]
    [Range(0.2f, 1f)]
    public float widthPercentage = 0.6f; // how much of screen width platform takes

    [Header("Optional Limits")]
    public float minWidth = 2f;
    public float maxWidth = 20f;

    private Camera cam;
    private float originalWidth;

    void Start()
    {
        cam = Camera.main;

        // Get original sprite width (world units)
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalWidth = sr.bounds.size.x / transform.localScale.x;
        }

        ResizePlatform();
    }

    void ResizePlatform()
    {
        if (cam == null) return;

        float camHeight = cam.orthographicSize * 2;
        float camWidth = camHeight * Screen.width / Screen.height;

        float targetWidth = camWidth * widthPercentage;

        // Clamp if needed
        targetWidth = Mathf.Clamp(targetWidth, minWidth, maxWidth);

        float scaleX = targetWidth / originalWidth;

        transform.localScale = new Vector3(
            scaleX,
            transform.localScale.y,
            transform.localScale.z
        );
    }

#if UNITY_EDITOR
    void Update()
    {
        // Live update in editor
        ResizePlatform();
    }
#endif
}