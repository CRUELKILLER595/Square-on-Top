using UnityEngine;

public class ScoreLine : MonoBehaviour
{
    public Transform cameraTransform;
    public float offsetY = 3f;

    private float maxPlayerY;
    private static float lineY;

    void Start()
    {
        maxPlayerY = cameraTransform.position.y;
        lineY = maxPlayerY + offsetY;
        UpdatePosition();
    }

    void Update()
    {
        // Track highest player position
        if (cameraTransform.position.y > lineY )
        {
            lineY = cameraTransform.position.y + offsetY;
            ScoreManager.AddPlatformScore();
        }

        // Line stays below highest point;

        UpdatePosition();
    }

    void UpdatePosition()
    {
        transform.position = new Vector3(transform.position.x, lineY, 0f);
    }

    public  float GetLineY()
    {
        return lineY;
    }
}