using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Movement")]
    public float smoothSpeed = 5f;
    public float downwardMultiplier = 0.5f;

    [Header("Offset")]
    public Vector3 offset;

    [Header("Limits")]
    public float maxFallOffset = 5f;

    [Header("Mobile Width Lock")]
    public float targetWidth = 5f;

    private Camera cam;

   void Start()
{
    cam = GetComponent<Camera>();

    float screenRatio = (float)Screen.width / Screen.height;
    cam.orthographicSize = (targetWidth / screenRatio) / 1.525f;

    // 🔥 Align camera horizontally to your world center
    transform.position = new Vector3(-71.6f, -9.32f,-1f);
}

    void LateUpdate()
    {
        if (target == null) return;

        float targetY = target.position.y + offset.y;
        float currentY = transform.position.y;

        float speed = (targetY > currentY) ? smoothSpeed : smoothSpeed * downwardMultiplier;

        float newY = Mathf.Lerp(currentY, targetY, speed * Time.deltaTime);

        float minY = target.position.y - maxFallOffset;
        newY = Mathf.Max(newY, minY);

        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}