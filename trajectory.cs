using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public LineRenderer line;
    public int points = 20;
    public float timeStep = 0.1f;

    public Rigidbody2D rb;

    void Start()
    {
        line.positionCount = points;
    }

    public void ShowTrajectory(Vector2 startPos, Vector2 velocity)
    {
        for (int i = 0; i < points; i++)
        {
            float time = i * timeStep;

            Vector2 position = startPos +
                               velocity * time +
                               0.5f * Physics2D.gravity * time * time;

            line.SetPosition(i, position);
        }

        line.enabled = true;
    }

    public void HideTrajectory()
    {
        line.enabled = false;
    }
}