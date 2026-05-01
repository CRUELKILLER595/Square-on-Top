using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject platformPrefab;
    public GameObject movingPlatformPrefab;
    public GameObject disappearingPlatformPrefab;

    [Header("Powerups")]
    public GameObject ghostPowerupPrefab;
    public GameObject breakPowerupPrefab;
    public GameObject doubleJumpPowerupPrefab;

    [Range(0f, 1f)] public float ghostChance = 0.1f;
    [Range(0f, 1f)] public float breakChance = 0.08f;
    [Range(0f, 1f)] public float doubleJumpChance = 0.12f;

    public int powerupCooldownMin = 2;
    public int powerupCooldownMax = 5;

    private int powerupCooldown = 0;

    [Header("References")]
    public Transform cameraTransform;

    [Header("Lane Positions")]
    public float leftX = -2f;
    public float rightX = 2f;
    public float movingPlatformX = 0f;
    public float disappearingPlatformX1 = -1f;
    public float disappearingPlatformX2 = 1f;

    [Header("Spawn Control")]
    public float spawnStartY = 0f;
    public float spawnAheadOffset = 10f;
    public float minSpawnDistanceAboveCamera = 3f;

    [Header("Gap Settings")]
    public float minGap = 4f;
    public float maxGap = 8f;

    [Header("Difficulty")]
    public float difficultyIncreaseRate = 0.02f;

    [Header("Moving Platform")]
    public float movingPlatformChance = 0.25f;
    public int movingCooldownLimit = 2;
    private int movingCooldown = 0;

    [Header("Disappearing Platform")]
    [Range(0f, 1f)]
    public float disappearingChance = 0.3f;
    public int minSafePlatforms = 1;
    private int safePlatformCounter = 0;

    [Header("Initial")]
    public int initialPlatforms = 6;

    private float highestY;
    private int platformIndex = 0;
    private float currentDifficulty = 0f;

    void Start()
    {
        float startY = Mathf.Max(spawnStartY, cameraTransform.position.y);

        // Always start above camera
        highestY = startY + 10f;

        for (int i = 0; i < initialPlatforms; i++)
        {
            SpawnNextPlatform();
        }
    }

    void Update()
    {
        float cameraTop = cameraTransform.position.y;

        float spawnStart = cameraTop + minSpawnDistanceAboveCamera;
        float targetY = cameraTop + spawnAheadOffset;

        // Ensure we never fall behind camera
        if (highestY < spawnStart)
        {
            highestY = spawnStart;
        }

        while (highestY < targetY)
        {
            SpawnNextPlatform();
        }
    }

    void SpawnNextPlatform()
    {
        float x;
        GameObject platformToSpawn;

        // 🎯 MOVING PLATFORM
        bool isMoving = false;

        if (movingCooldown <= 0 && Random.value < movingPlatformChance)
        {
            isMoving = true;
            movingCooldown = movingCooldownLimit;
        }
        else
        {
            movingCooldown--;
        }

        if (isMoving)
        {
            x = movingPlatformX;
            platformToSpawn = movingPlatformPrefab;
            safePlatformCounter++;
        }
        else
        {
            // 🎯 DISAPPEARING PLATFORM
            bool canSpawnDisappear = safePlatformCounter >= minSafePlatforms;

            if (canSpawnDisappear && Random.value < disappearingChance)
            {
                platformToSpawn = disappearingPlatformPrefab;
                x = (platformIndex % 2 == 0) ? disappearingPlatformX1 : disappearingPlatformX2;

                safePlatformCounter = 0;
            }
            else
            {
                // 🎯 NORMAL PLATFORM
                platformToSpawn = platformPrefab;
                x = (platformIndex % 2 == 0) ? leftX : rightX;

                safePlatformCounter++;
            }
        }

        // ✅ Spawn platform
        Vector3 spawnPos = new Vector3(x, highestY, 0f);
        GameObject newPlatform = Instantiate(platformToSpawn, spawnPos, Quaternion.identity);

        // 🎯 Spawn powerup ONLY on safe platforms
        if (platformToSpawn == platformPrefab || platformToSpawn == movingPlatformPrefab)
        {
            TrySpawnPowerup(spawnPos);
        }

        // 🎯 Difficulty scaling
        currentDifficulty += difficultyIncreaseRate;
        float difficultyFactor = Mathf.Clamp01(currentDifficulty);

        float currentMaxGap = Mathf.Lerp(minGap, maxGap, difficultyFactor);
        float gap = Random.Range(minGap, currentMaxGap);

        highestY += gap;
        platformIndex++;

        // 🔥 Scale disappearing platforms
        disappearingChance += 0.005f;
        disappearingChance = Mathf.Clamp(disappearingChance, 0.2f, 0.5f);

        // 🔥 Scale powerups slightly over time
        ghostChance += 0.0005f;
        doubleJumpChance += 0.0007f;

        ghostChance = Mathf.Clamp(ghostChance, 0.05f, 0.2f);
        doubleJumpChance = Mathf.Clamp(doubleJumpChance, 0.08f, 0.25f);
    }

    void TrySpawnPowerup(Vector3 platformPos)
    {
        if (powerupCooldown > 0)
        {
            powerupCooldown--;
            return;
        }

        float roll = Random.value;
        GameObject powerupToSpawn = null;

        // 🎯 Priority: rare → common
        if (roll < breakChance)
        {
            powerupToSpawn = breakPowerupPrefab;
        }
        else if (roll < breakChance + ghostChance)
        {
            powerupToSpawn = ghostPowerupPrefab;
        }
        else if (roll < breakChance + ghostChance + doubleJumpChance)
        {
            powerupToSpawn = doubleJumpPowerupPrefab;
        }

        if (powerupToSpawn != null)
        {
            Vector3 spawnPos = platformPos + new Vector3(0f, 1.2f, 0f);
            Instantiate(powerupToSpawn, spawnPos, Quaternion.identity);

            // 🎯 Reset cooldown (prevents spam)
            powerupCooldown = Random.Range(powerupCooldownMin, powerupCooldownMax);
        }
    }
}