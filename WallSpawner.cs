using UnityEngine;
using System.Collections.Generic;

public class WallSpawner : MonoBehaviour
{
    public GameObject wallPrefab;
    public Transform cameraTransform;

    public int maxSegments = 6;
    public float segmentHeight = 6f;
    public float leftX = -3f;
    public float rightX = 3f;
public float bufferY = 10f;
    private float highestY;
    private List<GameObject> walls = new List<GameObject>();

    void Start()
    {
        highestY = transform.position.y;

        for (int i = 0; i < maxSegments; i++)
        {
            SpawnWalls();
        }
    }

    void Update()
{
    float spawnThreshold = cameraTransform.position.y + 10f;
    float minAllowedY = cameraTransform.position.y - bufferY;

    // Only spawn if new walls are ABOVE buffer
    if (spawnThreshold > highestY && highestY > minAllowedY)
    {
        SpawnWalls();
    }

    // Destroy walls below buffer
    if (walls.Count > maxSegments * 2)
    {
        if (walls[0].transform.position.y < minAllowedY)
        {
            Destroy(walls[0]);
            Destroy(walls[1]);

            walls.RemoveAt(0);
            walls.RemoveAt(0);
        }
    }
}
void SpawnWalls()
{
    float height = wallPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

    // place at correct stacked position
    Vector3 leftPos = new Vector3(leftX, highestY + height / 2f, 0);
    Vector3 rightPos = new Vector3(rightX, highestY + height / 2f, 0);

    GameObject leftWall = Instantiate(wallPrefab, leftPos, Quaternion.identity);
    GameObject rightWall = Instantiate(wallPrefab, rightPos, Quaternion.identity);

    walls.Add(leftWall);
    walls.Add(rightWall);

    // move up by exact height
    highestY += height;
}
}